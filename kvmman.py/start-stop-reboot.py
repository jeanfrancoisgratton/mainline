#!/usr/bin/env python3

import libvirt
import sys
import platform
from time import sleep


def vmstartup(connection, vmname):
	vm = connection.lookupByName(vmname)
	if vm.isActive() == 1:
		print(vmname + " is already running")
	else:
		vm.create()


def vmshutdown(connection, vmname):
	vm = connection.lookupByName(vmname)
	if vm.isActive() == 0:
		print(vmname + " is not running")
	else:
		vm.destroy()


def vmreset(connection, vmname):
	vm = connection.lookupByName(vmname)
	vmshutdown(connection, vmname)
	while vm.isActive():
		sleep(1)
	vmstartup(connection, vmname)


def vmremove(conn, vmname):
	return None

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

if 'up' in sys.argv:
	vmstartup(conn, sys.argv[sys.argv.index('up') + 1])

if 'down' in sys.argv:
	vmshutdown(conn, sys.argv[sys.argv.index('down') + 1])

if 'reset' in sys.argv:
	vmreset(conn, sys.argv[sys.argv.index('reset') + 1])

if 'rm' in sys.argv:
	vmremove(conn, sys.argv[sys.argv.index('rm') + 1])
