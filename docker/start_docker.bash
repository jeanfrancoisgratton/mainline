#!/bin/bash
#docker login -e p2@fxinnovation.com -p geeSeiv8Kooxoht6ieF6AeS7Iesh0bajli5pheeM -u fxops
#docker pull pfcarrier/fxops:latest
#docker pull pfcarrier/bastionfx:latest

#docker rm -f fxops 2>/dev/null && docker rm -f bastionfx 2>/dev/null
docker run -p 2222:22 -v /var/lib/docker:/var/lib/docker -v /sys/fs/cgroup:/sys/fs/cgroup -v /usr/bin/docker:/usr/bin/docker -d --privileged -h deb-devtools --name devtools jfgratton/deb_devtools:latest
ssh -X -p 2222 jfgratton@localhost $1
