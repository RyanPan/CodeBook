using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadAttr
{
    public static class Program
    {
        //[ThreadStatic]
        //public static int _field = 10;

        [ThreadStatic]
        public static string str = "haha";

        public static ThreadLocal<int> _field = new ThreadLocal<int>(() => { return 10; });

        public static void Main(string[] args)
        {
            new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _field.Value++;
                    Console.WriteLine("Thread A: {0}", _field.Value);
                    Console.WriteLine("Thread A: {0}", str);
                }
            }).Start();

            new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _field.Value++;
                    Console.WriteLine("Thread B: {0}", _field.Value);
                    Console.WriteLine("Thread B: {0}", str);
                }
            }).Start();

            Console.ReadKey();
        }
    }
}

// 声明为 ThreadStatic 时，不同线程会重新进行初始化
