#!/usr/bin/env python3

import sys
import argparse
import libvirt

#hlp = Helpers()
#hl = hlp.hypervisorlist()

def setConnectionString(args):
    print('Arg = %s' % args.cstring)

parser = argparse.ArgumentParser()
subparsers = parser.add_subparsers()

parserConnect = subparsers.add_parser('ConnectionURI')
parserConnect.set_defaults(func=setConnectionString)
parserConnect.add_argument('-c', '--connect', dest='host')

args = parser.parse_args()
args.func(args)
print("COMPLETED")
#subparsers = parser.add_subparsers()

#infoparser = subparsers.addparser('info')
#reboot_parser = subparsers.add_parser('reboot')
#shutdown_parser = subparsers.add_parser('shutdown')
#start_parser = subparsers.add_parser('up')
#del_parser = subparsers.add_parser('rm')

