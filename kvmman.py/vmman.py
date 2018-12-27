#!/usr/bin/env python3

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

