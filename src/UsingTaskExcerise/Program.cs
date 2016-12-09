using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingTaskExcerise
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Demo WriteStarTask:");
            WriteStarTask();
            Console.WriteLine();

            Console.WriteLine("Demo ReturnValueTask:");
            ReturnValueTask();
            Console.WriteLine();

            Console.WriteLine("Demo AddingContinuation:");
            AddingContinuation();
            Console.WriteLine();

            Console.WriteLine("Demo SchedulingDifferentContinuationTask:");
            SchedulingDifferentContinuationTask();
            Console.WriteLine();

            Console.WriteLine("Demo AttachingChildTasks:");
            AttachingChildTasks();
            Console.WriteLine();

            Console.WriteLine("Demo TaskFactoryDemo:");
            TaskFactoryDemo();
            Console.WriteLine();
        }

        private static void TaskFactoryDemo()
        {
            Task<Int32[]> parent = Task.Run(() =>
            {
                Int32[] results = new Int32[3];

                TaskFactory factory = new TaskFactory(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously);

                factory.StartNew(() => results[0] = 1);
                factory.StartNew(() => results[1] = 2);
                factory.StartNew(() => results[2] = 3);

                return results;
            });

            Task finalTask = parent.ContinueWith((parentTask) =>
            {
                foreach (int i in parentTask.Result)
                {
                    Console.WriteLine(i);
                }
            });

            finalTask.Wait();
        }

        private static void AttachingChildTasks()
        {
            Task<Int32[]> parent = Task.Run(() =>
            {
                Int32[] results = new Int32[3];

                new Task(() => results[0] = 1, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = 2, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = 3, TaskCreationOptions.AttachedToParent).Start();

                return results;
            });

            Task finalTask = parent.ContinueWith((i) =>
            {
                foreach (int r in i.Result)
                {
                    Console.WriteLine(r);
                }
            });
            finalTask.Wait();
        }

        private static void SchedulingDifferentContinuationTask()
        {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });

            t.ContinueWith((i) =>
            {
                Console.WriteLine("Canceled");
            }, TaskContinuationOptions.OnlyOnCanceled);

            t.ContinueWith((i) =>
            {
                Console.WriteLine("Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task completedTask = t.ContinueWith((i) =>
            {
                Console.WriteLine("Completed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            completedTask.Wait();
        }

        private static void AddingContinuation()
        {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            }).ContinueWith((i) =>
            {
                return i.Result * 2;
            });
            Console.WriteLine(t.Result);
        }

        private static void ReturnValueTask()
        {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });
            Console.WriteLine(t.Result);
        }

        private static void WriteStarTask()
        {
            Task t = Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.Write("*");
                }
            });

            t.Wait();
        }
    }
}
