#!/usr/bin/env python3

from __future__ import print_function
import libvirt
import sys

conn = None
if len(sys.argv) > 1 and (sys.argv[1] == '-r' or sys.argv[1] == '--remote'):
	conn = libvirt.open('qemu+ssh://root@oslo.famillegratton.net:28/system?socket=/var/run/libvirt/libvirt-sock')
else:
	conn = libvirt.open('qemu+ssh://root@oslo/system')

if conn==None:
	print("Unable to connect to hypervisor.")
	sys.exit(1)

print("All domains:")
domains = conn.listAllDomains()
if len(domains) != 0:
    for domain in domains:
        print(str(domain.ID())+"  " + domain.name() + "  state: " + str(domain.state()))
conn.close()