import sys
import libvirt
from time import sleep
from tabulate import tabulate


class VMManagement():
    def __init__ (self, uri):
        self.qemuURI = uri
        self.conn = None

# connect2hv : connects (rw or ro) to the hypervisor
    def connect2hv(self, readonly = False):
        if readonly == False:
            self.conn = libvirt.open(self.qemuURI)
        else:
            self.conn = libvirt.openReadOnly(self.qemuURI)

        if self.conn == None:
            print("Unable to connect on '" + self.qemuURI + "'. Bailing out, you're on your own.")

        return self.conn

# getdomainstate() : get the VM state
    def getDomainState(self, state, reason):
        if state == libvirt.VIR_DOMAIN_NOSTATE:
            return 'VIR_DOMAIN_NOSTATE'
        elif state == libvirt.VIR_DOMAIN_RUNNING:
            return 'Running'
        elif state == libvirt.VIR_DOMAIN_BLOCKED:
            return 'Blocked'
        elif state == libvirt.VIR_DOMAIN_PAUSED:
            return 'Paused'
        elif state == libvirt.VIR_DOMAIN_SHUTDOWN:
            return 'Shut down'
        elif state == libvirt.VIR_DOMAIN_SHUTOFF:
            return 'Shutoff'
        elif state == libvirt.VIR_DOMAIN_CRASHED:
            return 'Crashed'
        elif state == libvirt.VIR_DOMAIN_PMSUSPENDED:
            return 'Suspended'
        else:
            return 'Unknown. Reason code: ' + str(reason)

# inventorylist: kvmman.py ls
    def inventoryList(self):
        conn = self.connect2hv(True)
        print("All domains on hypervisor '" + self.qemuURI + "':\n")
        domains = conn.listAllDomains()
        domainlist = []
        for domain in domains:
            state, maxmem, mem, cpus, cput = domain.info()
            state, reason = domain.state()
            domainstate = self.getDomainState(state, reason)

            if domain.ID() == -1:
                id = '-'
            else:
                id = str(domain.ID())

            domainattr = [id, domain.name(), domainstate, int(mem/1024), int(maxmem/1024), cpus]
            domainlist.append(domainattr)

        domainlist.sort(key=lambda row: row[0], reverse=True)
        print(tabulate(domainlist, headers=['ID', 'VM name', 'State', 'Current mem', 'Max mem', '# CPUs']))
        conn.close()


# start, stop, reboot and remove the vms:
    def vmstartup(self, vmname):
        conn = self.connect2hv()
        vm = conn.lookupByName(vmname)
        if vm.isActive() == 1:
            print(vmname + " is already running")
        else:
            vm.create()
        conn.close()

    def vmshutdown(self, vmname):
        conn = self.connect2hv()
        vm = conn.lookupByName(vmname)
        if vm.isActive() == 0:
            print(vmname + " is not running")
        else:
            vm.destroy()
        conn.close()

    def vmreset(self, vmname):
        conn = self.connect2hv()
        vm = conn.lookupByName(vmname)
        if vm.isActive():
            vm.destroy()
            while vm.isActive():
                sleep(1)
        vm.create()
        conn.close()

    def vmremove(self, vmname):
        conn = self.connect2hv()
        vm = conn.lookupByName(vmname)
        vm.destroy()
        while vm.isActive():
            print("Awaiting " + vmname + " to shut down...")
            sleep(1)
        print(vmname + " is down. Now removing from inventory.")
        vm.undefine()
        conn.close()
