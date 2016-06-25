#!/bin/bash

exit 0

echo "Installing vmware tools"
#make sure perl installed so we can run the script
#fuse-libs for vmware-block-fuse, this was breaking tools from starting
apt-get install -y perl fuse-libs

#perform tools install
cd /tmp
mkdir -p /mnt/cdrom 2>/dev/null
mount -o loop /root/linux.iso /mnt/cdrom
tar zxf /mnt/cdrom/VMwareTools-*.tar.gz -C /tmp/
/tmp/vmware-tools-distrib/vmware-install.pl -d
umount /mnt/cdrom
rm /root/linux.iso
