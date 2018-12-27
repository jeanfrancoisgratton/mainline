#!/usr/bin/env python3

import libvirt
import sys
import platform
from time import sleep

def vmremove(conn, vmname):
	vm = conn.lookupByName(vmname)
	vm.destroy()
	while vm.isActive():
		print("Awaiting " + vmname + " to shut down...")
		sleep(1)
	print(vmname + " is now down. Removing from inventory.")
	vm.undefine()

print("\033c") #clears screen
conn = None

if platform.system().lower() == 'darwin':
	skt = '?socket=/var/run/libvirt/libvirt-sock'
else:
	skt = ''
if len(sys.argv) > 1 and (sys.argv[1] == '-r' or sys.argv[1] == '--remote'):
	qemuURI = 'qemu+ssh://root@oslo.famillegratton.net:28/system' + skt
else:
	qemuURI = 'qemu+ssh://root@bergen/system' + skt

conn = libvirt.open(qemuURI)

if 'rm' in sys.argv:
	vmremove(conn, sys.argv[sys.argv.index('rm') + 1])
