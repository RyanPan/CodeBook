using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TLSDataSlot
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread[] threadArray = new Thread[4];

            for (int i = 0; i < threadArray.Length; i++)
            {
                threadArray[i] = new Thread(new ThreadStart(Slot.SlotTest));
                threadArray[i].Start();
            }
            Console.ReadLine();
        }
    }

    class Slot
    {
        static Random randomGenerator = new Random();

        public static void SlotTest()
        {
            // Set different data in each thread's data slot.
            Thread.SetData(Thread.GetNamedDataSlot("Random"), randomGenerator.Next(1, 20));

            // Write the data from each thread's data slot.
            Console.WriteLine("Data in thread_{0}'s data slot: {1,3}", AppDomain.GetCurrentThreadId(), Thread.GetData(Thread.GetNamedDataSlot("Random")));

            // Allow other thread time to execute SetData to show that a thread's data slot is unique to the thread.
            Thread.Sleep(1000);

            Console.WriteLine("Data in thread_{0}'s data slot it still: {1,3}", AppDomain.GetCurrentThreadId(), Thread.GetData(Thread.GetNamedDataSlot("Random")));

            // Allow time for other threads to show their data, then demonstrate that any code a thread executes has access to the thread's named data slot.
            Thread.Sleep(1000);

            Other o = new Other();
            o.ShowSlotData();
        }
    }

    class Other
    {
        public void ShowSlotData()
        {
            // This method has no access to access the data in the Slot class, but when executed by a thread it can obtain the thread's data from a named slot.
            Console.WriteLine("Other data displays data in thread_{0}'s data slot: {1,3}", AppDomain.GetCurrentThreadId(), Thread.GetData(Thread.GetNamedDataSlot("Random")));
        }
    }
}
