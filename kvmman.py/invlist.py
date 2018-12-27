#!/usr/bin/env python3

import libvirt
import sys
import platform
from tabulate import tabulate

print("\033c")
conn = None

if platform.system().lower() == 'darwin':
    skt = '?socket=/var/run/libvirt/libvirt-sock'
else:
    skt = ''
if len(sys.argv) > 1 and (sys.argv[1] == '-r' or sys.argv[1] == '--remote'):
    qemuURI = 'qemu+ssh://root@oslo.famillegratton.net:28/system' + skt
else:
    qemuURI = 'qemu+ssh://root@bergen/system' + skt

conn = libvirt.openReadOnly(qemuURI)

if conn==None:
	print("Unable to connect to hypervisor " + qemuURI + ". Bailing out.")
	sys.exit(1)

print("All domains on hypervisor '"+ qemuURI + "':\n")
domains = conn.listAllDomains()
domainlist = []
domainattr = []
for domain in domains:
    if domain.ID() == -1:
        state = 'Powered-off'
        id = '-'
    else:
        state = 'Running'
        id = str(domain.ID())

    domainattr = [id, domain.name(), state]

    domainlist.append(domainattr)

domainlist.sort(key=lambda row: row[0])
print(tabulate(domainlist, headers=['VM ID', 'VM name', 'State']))