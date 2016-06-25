#!/bin/sh

echo "cleaning up guest additions"
rm -rf VBoxGuestAdditions_*.iso VBoxGuestAdditions_*.iso*



echo "cleaning up udev rules"
rm -f /etc/udev/rules.d/70-persistent-net.rules
mkdir /etc/udev/rules.d/70-persistent-net.rules
rm -rf /dev/.udev/
rm -f /lib/udev/rules.d/75-persistent-net-generator.rules

#echo "Installing additional packages"
#apt-get -y install curl nfs-common build-essential perl dkms linux-headers-$(uname -r) puppet docker.io
apt-get -y autoremove
apt-get -y clean

echo "sleeping 20 seconds before shutdown"
sleep 20s
