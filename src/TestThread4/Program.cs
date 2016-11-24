using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestThread4
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.WriteLine("Press 'C' to terminate the application...\n");
            Thread t1 = new Thread(() => { if (Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant() == "C") cts.Cancel(); });
            Thread t2 = new Thread(new ParameterizedThreadStart(ServerClass.StaticMethod));
            t1.Start();
            t2.Start(cts.Token);
            t2.Join();
            cts.Dispose();
        }
    }

    public class ServerClass
    {
        public static void StaticMethod(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            Console.WriteLine("ServerClass.StaticMethod is running on another thread.");

            while(!token.IsCancellationRequested)
            {
                Thread.SpinWait(5000);
            }
            Console.WriteLine("The worker thread has been canceled. Press any key to exit.");
            Console.ReadKey(true);
        }
    }
}
