#!/bin/sh


# sudoers
sed -i -e '/Defaults\s\+env_reset/a Defaults\texempt_group=sudo' /etc/sudoers
#sed -i -e 's/%sudo  ALL=(ALL:ALL) ALL/%sudo  ALL=NOPASSWD:ALL/g' /etc/sudoers


# sshd_config
echo "UseDNS no" >> /etc/ssh/sshd_config
sed -i -e 's/PermitRootLogin\ without-password/PermitRootLogin\ no/g' /etc/ssh/sshd_config

# rewrite sources.list
cat << EOF > /etc/apt/sources.list

deb http://debian.mirror.iweb.ca/debian/ jessie main
deb http://security.debian.org/ jessie/updates main
deb http://debian.mirror.iweb.ca/debian/ jessie-updates main
deb http://debian.mirror.iweb.ca/debian/ jessie-backports main
EOF


apt-get update
