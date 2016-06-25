#!/bin/sh

/usr/sbin/addgroup fxops
#useradd -g fxops -c 'FX Innovation account' -d /home/fx -m -s /bin/bash fx
#echo "fx:fxops" | chpasswd

cd /home/fx
grep -v PATH .bashrc > b && echo "export PATH=/bin:/sbin:/usr/bin:/usr/sbin:/usr/local/bin" >> b && mv b .bashrc
/usr/sbin/usermod -g fxops fx

