using System;
using System.Collections.Generic;
using System.Text;

namespace prjShotMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            CActionManager actionManager = new CActionManager();
            actionManager.Start();

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
            actionManager.Stop();
        }
    }
}