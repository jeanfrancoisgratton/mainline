#! /bin/sh

declare base_dir="/data/"
declare domain="guavus.mtl"

declare srx_dir=""
declare setup=""
declare mgtNode=""

declare -a hostArray
declare -A ipArray
declare -A vcpuArray
declare -A vmemArray
declare arrayIndex=0

echo "127.0.0.1   localhost localhost.localdomain localhost4 localhost4.localdomain4" > hosts
echo "::1         localhost localhost.localdomain localhost6 localhost6.localdomain6" >> hosts

if [ ! $# -eq 1 ]; then
    echo " usage : $0 FILE"
    exit 1;
fi

#virt-sysprep --operations defaults,-tmp-files -d srx-template
while read -r line || [[ -n "$line" ]]; do

    if [ "${line}" == "" ];then
        continue
    fi

    # remove comment
    if [[ ${line} =~ ^#  ]];then
        continue
    fi

    ipaddr=`echo $line | cut -d' ' -f1`
    hostname=`echo $line | cut -d' ' -f2`
    vcpu=`echo $line | cut -d' ' -f3`
    vmem=`echo $line | cut -d' ' -f4`

    if [ ${hostname} = "" ] || [ ${ipaddr} = "" ]; then
        echo "error in srx-setup.txt file: ${line}"
        echo "aborting"
        exit 1
    fi

    if [ ${ipaddr} == "setup" ]; then
        setup=${hostname}
    	srx_dir="${setup}"
    	echo "Making directory ${base_dir}/${srx_dir}"
        virsh pool-info ${srx_dir} > /dev/null 2>&1 
      	if [ $? != 0 ]; then
            mkdir ${base_dir}/${srx_dir}
            virsh pool-define-as ${srx_dir} dir --target ${base_dir}/${srx_dir}
            virsh pool-autostart ${srx_dir}
            virsh pool-start ${srx_dir}
       fi
	continue
    fi

    # setup must be before any host line
    if [ "${srx_dir}" != "" ]; then
        hostArray[${arrayIndex}]=${hostname}
        ipArray[${hostname}]=${ipaddr}
        vcpuArray[${hostname}]=${vcpu}
        vmemArray[${hostname}]=${vmem}
        arrayIndex=$((${arrayIndex} +1))

        if [ ${hostname} == ${setup}-mgt-01 ]; then
            mgtNode=${hostname}
        fi
    else
    	echo "Setup not defined. Aborting"
    	exit 1
    fi

done < $1

for hostname in ${hostArray[@]}; do

    ipaddr=${ipArray[${hostname}]}
    vpcu=${vcpuArray[${hostname}]}
    vmem=${vmemArray[${hostname}]}

    echo "Cloning srx-template to ${base_dir}/${srx_dir}/${hostname}.qcow2"
    os_disk="${base_dir}/${srx_dir}/${hostname}.qcow2"

    virt-clone -o centos73-minimal -n ${hostname} --file ${os_disk}
    virsh setvcpus ${hostname} ${vcpu} --config 
    virsh setmaxmem ${hostname} ${vmem} --config
    virsh setmem ${hostname} ${vmem} --config

    customize_command="virt-customize -a ${os_disk} --hostname ${hostname} \
        --edit \"/etc/sysconfig/network-scripts/ifcfg-eth0:s/IPADDR=[0-9.]*/IPADDR=${ipaddr}/\" "

    if [[ ${hostname} == 'srx-ops-01-mgt-01' ]] ;then
        data_disk="${base_dir}/${srx_dir}/${hostname}-data.qcow2"
        qemu-img create ${data_disk} 900G
        virsh attach-disk --config ${hostname} ${data_disk} vdb --cache none
    fi
    if [[ ${hostname} =~ ${setup}-slv[0-9]* ]] ;then
    	for i in {1..6} ;do
  	    data_disk="${base_dir}/${srx_dir}/${hostname}-data-`printf "%02d" ${i}`.raw"
            echo "Creating disk ${data_disk} for ${hostname}"
            qemu-img create ${data_disk} 100G

            echo "Creating xfs filesystem on ${data_disk}"
            virt-format --filesystem=xfs -a ${data_disk}

            target="vd`printf "\x$(printf %x $(( $i + 97)) )"`"
            echo "Attaching ${data_disk} to ${hostname} as /dev/${target}"
            virsh attach-disk --config ${hostname} ${data_disk} ${target} --cache none

            data_disk_dir="/opt/data`printf "%02d" ${i}`"
            customize_command="${customize_command} \
                --run-command=\"mkdir ${data_disk_dir}\" --run-command=\"echo /dev/${target}1 ${data_disk_dir} xfs defaults 0 0 >> /etc/fstab\" "
        done
    fi

    if [[ ${hostname} = "srx-ops-01-mgt-01.guavus.mtl" ]];then
      echo "Attaching new disk on management"
      qemu-img create /data/srx-ops-01/srx-ops-01-mgt-data.qcow2 900G
      virsh attach-disk --config srx-ops-01-mgt-01.guavus.mtl /data/srx-ops-01/srx-ops-01-mgt-data.qcow2 vdb --cache none
    fi

    eval ${customize_command}

done
