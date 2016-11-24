using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppDomainData
{
    class Program
    {
        static void Main(string[] args)
        {
            //Demo1();
            //Demo2();
            Demo3();
        }

        #region Demo1

        static void Demo1()
        {
            Console.WriteLine("Fetching current Domain");

            // Use current AppDomain, and store same data
            AppDomain domain = AppDomain.CurrentDomain;
            Console.WriteLine("Setting AppDomain Data");
            string name = "MyData";
            string value = "Some data to store";
            domain.SetData(name, value);
            Console.WriteLine("Fetching Domain Data");
            Console.WriteLine("The data found for key {0} is {1}", name, domain.GetData(name));
            Console.ReadLine();
        }

        #endregion

        #region Demo2

        private static void Demo2()
        {
            AppDomain domainA = AppDomain.CreateDomain("MyDomainA");
            AppDomain domainB = AppDomain.CreateDomain("MyDomainB");
            domainA.SetData("DomainKey", "Domain A Value");
            domainB.SetData("DomainKey", "Domain B Value");
            OutPutCall();
            domainA.DoCallBack(OutPutCall); // CrossAppDomainDelegate call
            domainB.DoCallBack(OutPutCall); // CrossAppDomainDelegate call
            Console.ReadLine();
        }

        private static void OutPutCall()
        {
            AppDomain domain = AppDomain.CurrentDomain;
            Console.WriteLine("the value {0} was found in {1}, running on thread Id {2}", domain.GetData("DomainKey"), domain.FriendlyName, Thread.CurrentThread.ManagedThreadId);
        }

        #endregion
        
        #region Demo3

        private string message;
        private static Timer timer;
        private static bool completed;

        static void Demo3()
        {
            Program p = new Program();
            Thread workerThread = new Thread(p.DoSomeWork);
            workerThread.Start();

            // create timer with callback
            TimerCallback timerCallback = new TimerCallback(p.GetState);
            timer = new Timer(timerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

            // wait for worker to complete
            do
            {
                // simple wait, do nothing
            } while (!completed);

            Console.WriteLine("exiting main thread");
            Console.ReadLine();
        }

        public void GetState(object state)
        {
            // not done so return
            if (message == string.Empty)
                return;
            Console.WriteLine("Worker is {0}", message);

            // is other thread completed yet, if so signal main thread to stop waiting
            if (message == "Completed")
            {
                timer.Dispose();
                completed = true;
            }
        }

        public void DoSomeWork()
        {
            message = "Processing";

            // simulate doing some work
            Thread.Sleep(3000);
            message = "Completed";
        }

        #endregion
    }
}
