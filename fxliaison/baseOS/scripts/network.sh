#!/bin/sh

echo "cleaning up dhcp leases and killing dhcp client"
pkill dhclient
rm -f /var/lib/dhcp/*

 rewrite sources.list
cat << EOF > /etc/apt/sources.list

deb http://debian.mirror.iweb.ca/debian/ jessie main
deb http://security.debian.org/ jessie/updates main
deb http://debian.mirror.iweb.ca/debian/ jessie-updates main
deb http://debian.mirror.iweb.ca/debian/ jessie-backports main
EOF


apt-get update

cat << EOF > /etc/network/interfaces

auto lo
iface lo inet loopback
iface eth0 inet static
 address 10.128.0.230
 netmask 255.255.255.0
 gateway 10.128.0.254

EOF

echo "nameserver 8.8.8.8" > /etc/resolv.conf
