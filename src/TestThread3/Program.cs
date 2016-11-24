using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestThread3
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread sleepingThread = new Thread(Program.SleepIndefinitely);
            sleepingThread.Name = "Sleeping";
            sleepingThread.Start();
            Thread.Sleep(2000);
            sleepingThread.Interrupt();

            Thread.Sleep(2000);

            sleepingThread = new Thread(Program.SleepIndefinitely);
            sleepingThread.Name = "Sleeping2";
            sleepingThread.Start();
            Thread.Sleep(2000);
            sleepingThread.Abort();
        }

        private static void SleepIndefinitely()
        {
            Console.WriteLine("Thread '{0}' about to sleep indefinitely.", Thread.CurrentThread.Name);
            try
            {
                Thread.Sleep(5000);
                Console.WriteLine("HHaha");
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Thread '{0}' awoken.", Thread.CurrentThread.Name);
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Thread '{0}' aborted.", Thread.CurrentThread.Name);
                Thread.ResetAbort();
            }
            finally
            {
                Console.WriteLine("Thread '{0}' executing finally block.", Thread.CurrentThread.Name);
            }

            Console.WriteLine("Thread '{0}' finishing normal execution.", Thread.CurrentThread.Name);
            Console.WriteLine();
        }
    }
}
