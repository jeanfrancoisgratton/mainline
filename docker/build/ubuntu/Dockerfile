FROM ubuntu:14.10
MAINTAINER J.F.Gratton "jean-francois.gratton@videotron.ca"

# VERSION 1.0 - 2015.01.02 - J.F.Gratton
# Initial setup : standalone container
#
# VERSION 1.2 - 2015.01.03 - J.F.Gratton
# Minor fixes, trying to slim down image
#
# VERSION 1.2.1 - 2015.01.03 - J.F.Gratton
# Added some dev packages
#
#
# Following versions will be attach-enabled (ie might be started from fxops, for instance)
# Other versions will also include a Perforce instance, if needed (so far I'd be the only one toying with this)
#

# Set locale
RUN locale-gen en_CA.UTF-8
ENV LC_ALL en_CA.UTF-8
ENV DEBIAN_FRONTEND noninteractive
ENV REFRESHED_AT 2015-01-02.14
ENV USERNAME jfgratton pierre.fortincarrier yazid.benakmoum fx


# Basic tools as well as some packaging/build tools
#
RUN apt-get update && apt-get install -y apt-transport-https
ADD repos/sources.list /etc/apt/sources.list
# ugly hack to make sure that /etc/apt/sources.list is well updated
RUN apt-get update
RUN apt-get install -y --no-install-recommends kvpnc kgpg wget kdesdk terminator bc openssh-server man git vim sudo net-tools keychain pwgen psmisc procps bash-completion telnet links2 links curl sharutils git-core build-essential pkg-config libtool libevent-dev python monodevelop libncurses-dev zlib1g-dev automake libssh-dev cmake ruby apparmor apparmor-profiles apparmor-utils g++ gcc cpp autoconf zenmap  dh-make dh-autoreconf cli-common-dev debhelper autotools-dev dpkg-dev cdbs devscripts fakeroot diffstat unzip strace dh-translations intltool python-scour pbuilder devscripts lintian

RUN wget -q -O - https://get.docker.io/gpg | apt-key add - && wget -O - https://www.hipchat.com/keys/hipchat-linux.key | apt-key add -
RUN apt-get install -y --force-yes lxc-docker hipchat

RUN mkdir /var/run/sshd && echo "UseDNS no" >> /etc/ssh/sshd_config
#RUN groupadd docker

# Install tmate to share ssh session
#
RUN wget -q https://github.com/nviennot/tmate/archive/1.8.10.tar.gz && \
    tar -xzf 1.8.10.tar.gz && cd tmate-1.8.10 && ./autogen.sh && ./configure  && \
    make && make install && cd .. && rm -rf 1.8.10.tar.gz tmate-1.8.10


# /etc/skel additional configs
#
RUN mkdir -p /etc/skel/.config/terminator
ADD skel/terminator-config /etc/skel/.config/terminator/config
ADD skel/screenrc /etc/skel/.screenrc
ADD skel/vimrc /etc/skel/.vimrc

# Allow ops users to use sudo
#
RUN sed -i 's/^%sudo\tALL=(ALL:ALL) ALL/%sudo  ALL=(ALL:ALL) NOPASSWD: ALL/' /etc/sudoers

# Create ops users
#
RUN mkdir /etc/skel/.ssh
#ADD ssh/known_hosts /etc/skel/.ssh/known_hosts
#ADD ssh/config /etc/skel/.ssh/config
ADD keys/id_rsa.fxops /etc/skel/.ssh/id_rsa.fxops
ADD keys/id_rsa.fxops.pub /etc/skel/.ssh/id_rsa.fxops.pub
#ADD ssh/authorized_keys /etc/skel/.ssh/authorized_keys
ADD users/bashrc-nonjfg /etc/skel/.bashrc
ADD users/profile.jfgratton /etc/skel/.profile

RUN for username in $USERNAME ; do \
      useradd -m -s /bin/bash $username ; \
      echo "$username:fx" | chpasswd || true; \
      addgroup $username sudo ; \
      addgroup $username docker ; \
    done

# LEGACY STUFF FROM P2's FXOPS. LEFT OUT THIS TIME AROUND
# Allow to run docker within docker and pull ubuntu
#
#VOLUME /var/lib/docker - now bindmounting /var/lib/docker instead
#ADD wrapdocker /sbin/wrapdocker
#RUN chmod +x /sbin/wrapdocker

# Holder to have some file persist in the hosts
#
RUN mkdir /work && \
    chmod 0777 /work

# Allow all users to use ping
#
RUN chmod +s /bin/ping

# Install google-chrome
#
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | sudo apt-key add - && \
    wget -q https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb && \
    apt-get -y install libxtst6 xdg-utils libnss3 libxss1 libappindicator1 && \
    dpkg -i google-chrome-stable_current_amd64.deb

# Allow to run skype
#
ADD binaries/skype /usr/bin/skype

# Customization for each ops personal
#
ENV user pierre.fortincarrier
ADD keys/id_rsa.p2 /home/$user/.ssh/id_rsa 
ADD keys/id_rsa.p2.pub /home/$user/.ssh/id_rsa.pub
RUN chown -R $user:$user /home/$user/ && \
    chmod 0700 /home/$user/.ssh && \
    chmod 0600 /home/$user/.ssh/id_rsa && \
    chown -R $user:$user /home/$user/.ssh/id_rsa && \
    echo "[user]" >> /home/$user/.gitconfig && \
    echo "email = pierre.fortincarrier@gmail.com" >> /home/$user/.gitconfig && \
    echo "name = Pierre Fortin Carrier" >> /home/$user/.gitconfig

ENV user yazid.benakmoum
ADD keys/id_rsa.yazid /home/$user/.ssh/id_rsa
ADD keys/id_rsa.yazid.pub /home/$user/.ssh/id_rsa.pub
RUN chown -R $user:$user /home/$user && chmod 0700 /home/$user/.ssh && chmod 0600 /home/$user/.ssh/id_rsa

ENV user jfgratton
ADD keys/id_rsa.docker /home/$user/.ssh/id_rsa
ADD keys/id_rsa.docker.pub /home/$user/.ssh/id_rsa.pub
ADD users/profile.jfgratton .profile
RUN rm -f .bash_profile .bashrc && ln -s .profile .bashrc && ln -s .profile .bash_profile
ADD users/authorized_keys.jfgratton /home/$user/.ssh/authorized_keys
RUN chmod 0700 /home/$user/.ssh && chmod 0600 /home/$user/.ssh/id_rsa && \
    echo "[user]\n  email = grattojf@gmail.com\n  name = Jean-Francois Gratton" > /home/$user/.gitconfig && chown -R $user:$user /home/$user

ENV user _

#
## End of ops user customization

# Export ports and default startup
#
EXPOSE 22 177
ADD binaries/startContainer.bash /containerStart.bash
RUN chmod +x /containerStart.bash
CMD ["/bin/bash", "/containerStart.bash"]

