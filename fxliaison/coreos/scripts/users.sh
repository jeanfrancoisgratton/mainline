#!/bin/sh

exit 0

/usr/sbin/addgroup fxops
#useradd -g fxops -c 'FX Innovation account' -d /home/fx -m -s /bin/bash fx
#echo "fx:fxops" | chpasswd

/usr/sbin/usermod -g fxops fx

