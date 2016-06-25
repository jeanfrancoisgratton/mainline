using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vestris.VMWareLib;

namespace vestrisTests
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            VMWareVirtualHost vhost;
            VMWareVirtualMachine vmachine;
            Console.WriteLine("STARTING"); Console.WriteLine(Environment.NewLine);
            vhost = new VMWareVirtualHost();
            Console.WriteLine("CONNECTING"); Console.WriteLine(Environment.NewLine);
            vhost.ConnectToVMWareVIServer("10.128.12.52", "root", "M3ridian");
            Console.WriteLine("OPENING VM"); Console.WriteLine(Environment.NewLine);
            vmachine = vhost.Open("[MtlEsxVmfs4-Lab] jfg-mono-test-1/jfg-mono-test-1.vmx");
            Console.WriteLine("SHUTTING DOWN"); Console.WriteLine(Environment.NewLine);
            vmachine.PowerOff();
            Console.WriteLine("CLOSING"); Console.WriteLine(Environment.NewLine);
        }
    }
}