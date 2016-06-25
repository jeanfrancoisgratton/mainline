using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMware.Vim;

namespace JFG.FX.VMWare
{
    internal class vmspecs
    {
        private static void Main(string[] args)
        {
            Console.Clear();
            //Console.WriteLine("TEST1 : connect = ip (esx)");
            //Console.WriteLine("TEST2 : connect = ip (vcenter)");
            Console.WriteLine("TEST3 : connect = ip (esx), null, null");
            VimClient vmc = new VimClientImpl();
            vmc.Connect("10.128.12.52", null, null);
            vmc.Login("root", "M3ridian");
            DiagnosticManager diagMgr = (DiagnosticManager) vmc.GetView(vmc.ServiceContent.DiagnosticManager, null);
            DiagnosticManagerLogHeader log = diagMgr.BrowseDiagnosticLog(null, "hostd", 999999999, null);
            int lineEnd = log.LineEnd;
            // Get the last 5 lines of the log first
            int start = lineEnd - 15;
            log = diagMgr.BrowseDiagnosticLog(null, "hostd", start, null);
            foreach (string line in log.LineText)
            {
                Console.WriteLine(line);
            }
        }
    }
}
