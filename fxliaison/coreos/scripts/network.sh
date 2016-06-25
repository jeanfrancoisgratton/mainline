#!/bin/sh

exit 0

pkill dhclient

cat << EOF > /etc/network/interfaces

auto lo
iface lo inet loopback
iface eth0 inet static
 address 10.128.0.230
 netmask 255.255.255.0
 gateway 10.128.0.254

EOF

echo "nameserver 8.8.8.8" > /etc/resolv.conf
