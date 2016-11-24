using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestThread2
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadWithState threadWithState = new ThreadWithState(
                "This report displays the number: {0}.", 42, new ExampleCallback(Program.TheCallbackMethod));

            Thread t = new Thread(threadWithState.ThreadProc);
            t.Start();
            Console.WriteLine("Main thread does some work, then waits.");

            t.Join();
            Console.WriteLine("Independent task has completed; main thread ends.");
        }

        public static void TheCallbackMethod(int n)
        {
            Console.WriteLine("Independent task printed {0} lines.", n);
        }
    }

    public delegate void ExampleCallback(int num);

    public class ThreadWithState
    {
        private string boilerplate;
        private int value;
        private ExampleCallback callback;

        public ThreadWithState(string text, int number, ExampleCallback ec)
        {
            boilerplate = text;
            value = number;
            callback = ec;
        }

        public void ThreadProc()
        {
            Console.WriteLine(boilerplate, value);
            if (callback != null)
            {
                callback(2);
            }
        }
    }
}
