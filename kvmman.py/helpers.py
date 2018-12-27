import sys
import libvirt

class CommandValue(Enum):
	none = auto()
	connect = auto()    # useless, really
	show = auto()
	info = auto()
	up = auto()
	down = auto()
	reboot = auto()
	rm = auto()
	lssnap = auto()
	snap = auto()
	snaprev = auto()
	rmsnap = auto()
	create = auto()
	resadd = auto()
	resdel = auto()
	resedt = auto()

class Helpers:
	def _init_(self):
		self.connectURI = 'qemu:///system'
		self.cval = CommandValue()

	def missingArg(self, commande, number):
		print("Command " + commande + " is missing " + str(number) + " argument. Exiting.")
		sys.exit(-1)

	def parseCLI(self, args):
		if "-c" in args or "--connect" in args:
			if "-c" in args:
				n = args.index('-c')
			else:
				n = args.index('--connect')
			if n + 1 < len(sys.argv):
				self.connectURI = 'qemu+ssh://root@' + args[n + 1] + '/system'
		else:
			self.connectURI='qemu:///system'

		if "show" in args or "ls" in args:
			self.cval = CommandValue.show

		if "info" in args:
			if args.index('info') + 1 < len(sys.argv):
				self.commande = "info " + args[args.index('info') + 1]
				self.cval = CommandValue.info
			else:
				self.missingArg('info', 1)
