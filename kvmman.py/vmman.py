#!/usr/bin/env python3
import sys
import platform
import vmmanagement
from subprocess import run

# -----------------------------------
# Connection option for all commands:
# -----------------------------------
# vmman.py [-c hypervisor] defaults to qemu:///system

# --------------------
# Generic VM commands:
# --------------------
# vmman.py show : list all vms, with their state
# vmman.py info domain : gives information on domainname
# vmman.py {up|down|reboot|rm} domain : boots, shuts down, reboots or deletes the domain
# --------------------
# Snapshot management:
# --------------------
# vmman.py lssnap domain : list snapshots attached to domain
# vmman.py snapshot domain snapname : create snapshot named snapname on domain
# vmman.py snaprev domain [snapsname] : reverts domain to latest snapshot or to snapname
# vmman.py rmsnap domain [snapname] : removes from the domain the latest snapshot, or snapname
# (snaprev & rmsnap use the current snapshot if none is provided)
# --------------
# Create new VM:
# --------------
# vmman.py domain create -t template -ip ipaddress [-d disk_size] [-vcpu vcpu] [-vmaxcpu vmaxcpu] [-vmxm vmaxmem] [-vmem vmem]
# --------------------
# Resource management:
# --------------------
# vmman.py domain resadd [disk name size] [net iface ipaddr netmask]
# vmman.py domain resdel [disk name] [net iface]
# vmman.py domain resmod [cpu number] [maxmem size] [mem size]


def getConnectURI(args):
    if '-c' in args:
        n = args.index('-c')
    else:
        n = args.index('--connect')
    if platform.system().lower() == 'darwin':
        skt = '?socket=/var/run/libvirt/libvirt-sock'
    else:
        skt = ''
    if len(sys.argv) > n:
        return 'qemu+ssh://root@' + args[n + 1] + '/system' + skt
    else:
        return 'qemu:///system'

def usage():
    pass

def version():
    print("vmman.py, (c) 2018 by J.F.Gratton")
    print("v1.01 (2018.12.29) : folded info() into ls()")
    print("v1.00 (2018.12.28) : initial version, almost feature-complete")

def main():
    qemuURI = 'qemu:///system'
    if '-c' in sys.argv or '--connect' in sys.argv:
        qemuURI = getConnectURI(sys.argv)

    if 'ls' in sys.argv or 'show' in sys.argv:
        vm = vmmanagement.VMManagement(qemuURI)
        vm.inventoryList()
        sys.exit(0)

    if 'version' in sys.argv or '-v' in sys.argv:
        version()
        sys.exit(0)

    if 'help' in sys.argv or '-h' in sys.argv or '-?' in sys.argv:
        usage()
        sys.exit(0)

    if 'up' in sys.argv:
        n = sys.argv.index('up')
        if len(sys.argv) > n + 1:
            vm = vmmanagement.VMManagement(qemuURI)
            vm.vmstartup(sys.argv[n + 1])
        else:
            usage()
        sys.exit(0)

    if 'down' in sys.argv:
        n = sys.argv.index('down')
        if len(sys.argv) > n + 1:
            vm = vmmanagement.VMManagement(qemuURI)
            vm.vmshutdown(sys.argv[n + 1])
        else:
            usage()
        sys.exit(0)

    if 'reset' in sys.argv:
        n = sys.argv.index('reset')
        if len(sys.argv) > n + 1:
            vm = vmmanagement.VMManagement(qemuURI)
            vm.vmreset(sys.argv[n + 1])
        else:
            usage()
        sys.exit(0)

    if 'rm' in sys.argv:
        n = sys.argv.index('rm')
        if len(sys.argv) > n + 1:
            vm = vmmanagement.VMManagement(qemuURI)
            vm.vmremove(sys.argv[n + 1])
        else:
            usage()
        sys.exit(0)

    if 'console' in sys.argv:
        n = sys.argv.index('console')
        if len(sys.argv) > n + 1:
            run(['virsh', '-c', qemuURI, 'console', sys.argv[n + 1]])
        else:
            usage()
        sys.exit(0)

if __name__ == '__main__':
   main()