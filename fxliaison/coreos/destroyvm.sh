#!/bin/sh

ssh root@esx vim-cmd vmsvc/getallvms|grep coreOS|awk '{print "ssh root@esx vim-cmd vmsvc/destroy "$1}'
