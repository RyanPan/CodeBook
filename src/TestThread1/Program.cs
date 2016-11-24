using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestThreads1
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerClass serverClass = new ServerClass();
            Thread InstanceCaller = new Thread(serverClass.InstanceMethod);
            InstanceCaller.Start();
            Console.WriteLine("The Main() thread calls this after starting the new InstanceCaller thread.");

            Thread StaticCaller = new Thread(ServerClass.StaticMethod);
            StaticCaller.Start();
            Console.WriteLine("The Main() thread calls this after starting the new StaticCaller thread.");

            Thread newThread = new Thread(Program.DoWork);
            newThread.Start(55);

            Program p = new Program();
            Thread newInstanceThread = new Thread(p.DoMoreWork);
            newInstanceThread.Start("Hello");
        }

        static void DoWork(object obj)
        {
            Console.WriteLine("Static thread procedure. Data='{0}'", obj);
        }

        void DoMoreWork(object obj)
        {
            Console.WriteLine("Instance thread procedure. Data='{0}'", obj);
        }
    }

    public class ServerClass
    {
        public void InstanceMethod()
        {
            Console.WriteLine("ServerClass.InstanceMethod is running on another thread.");
            Thread.Sleep(3000);
            Console.WriteLine("The instance method called by the worker thread has ended.");
        }

        public static void StaticMethod()
        {
            Console.WriteLine("ServerClass.StaticMethod is running on another thread.");
            Thread.Sleep(5000);
            Console.WriteLine("The static method called by the worker thread has ended.");
        }
    }
}
