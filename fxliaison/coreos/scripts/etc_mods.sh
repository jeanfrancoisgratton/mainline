#!/bin/sh

exit 0

# sudoers
sed -i -e '/Defaults\s\+env_reset/a Defaults\texempt_group=sudo' /etc/sudoers
#sed -i -e 's/%sudo  ALL=(ALL:ALL) ALL/%sudo  ALL=NOPASSWD:ALL/g' /etc/sudoers


# sshd_config
echo "UseDNS no" >> /etc/ssh/sshd_config
sed -i -e 's/PermitRootLogin\ without-password/PermitRootLogin\ no/g' /etc/ssh/sshd_config


