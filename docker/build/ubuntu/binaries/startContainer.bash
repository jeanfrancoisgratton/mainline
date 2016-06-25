#!/bin/bash

# Taken from Pierre Fortin Carrier's start.sh, used in fxops

/usr/sbin/sshd -D &

echo "Docker container is starting."

## Give some time for docker daemon to start

sleep 5

echo "Here we go !"

wait
