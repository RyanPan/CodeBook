using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadStaticAttributeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 3; i++)
            {
                Thread thread = new Thread(new ThreadStart(ThreadData.ThreadStaticDemo));
                thread.Start();
            }
        }
    }

    class ThreadData
    {
        [ThreadStatic]
        static int threadSpecificData;

        public static void ThreadStaticDemo()
        {
            // Store the managed thread id for each thread in the static variable.
            threadSpecificData = Thread.CurrentThread.ManagedThreadId;

            // Allow other thread time to execute the same code, to show that the static data is unique to each thread.
            Thread.Sleep(1000);

            // Display the static data.
            Console.WriteLine("Data for managed thread {0}: {1}", Thread.CurrentThread.ManagedThreadId, threadSpecificData);
        }
    }
}
