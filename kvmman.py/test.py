#!/usr/bin/env python3

import sys
import libvirt

#conn = libvirt.open('qemu+ssh://root@'+sys.argv[1]+"'system/")
conn = libvirt.open("qemu+ssh://root@bergen/system/")

for id in conn.listDomainsID():
    dom = conn.lookupByID(id)
    infos = dom.info()

print(infos)