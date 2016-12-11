using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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

            Console.WriteLine("Demo TaskWaitAll:");
            TaskWaitAll();
            Console.WriteLine();

            Console.WriteLine("Demo TaskWaitAny:");
            TaskWaitAny();
            Console.WriteLine();

            Console.WriteLine("Demo ParallelTask:");
            ParallelTask();
            Console.WriteLine();

            Console.WriteLine("Demo ParallelTaskBreak:");
            ParallelTaskBreak();
            Console.WriteLine();

            Console.WriteLine("Demo AsyncAndAwait:");
            AsyncAndAwait();
            Console.WriteLine();

            Console.WriteLine("Demo SleepAsyncA:");
            SleepAsyncA(3000);
            SleepAsyncB(3000);
            Console.WriteLine();

            FileStream fs = new FileStream(Path.Combine(Environment.CurrentDirectory, "a.txt"), FileMode.Open);
            byte[] bytes = new byte[100];
            Task<int> task = fs.ReadAsync(bytes, 0, 100);
            int n = task.Result;
            fs.Close();
            Console.WriteLine(n);

            FileInfo fi = new FileInfo(Path.Combine(Environment.CurrentDirectory, "a.txt"));
            StreamReader sr = fi.OpenText();
            //FileStream fss = fi.Open(FileMode.Open);
            //fss.Read(bytes, 0, 100);
            //fss.Close();
            string str = sr.ReadLine();
            Console.WriteLine(str);
        }

        private static Task SleepAsyncB(int millisecondTimeout)
        {
            TaskCompletionSource<bool> tcs = null;
            var t = new Timer(delegate { tcs.TrySetResult(true); }, null, -1, -1);
            tcs = new TaskCompletionSource<bool>(t);
            t.Change(millisecondTimeout, -1);
            return tcs.Task;
        }

        private static Task SleepAsyncA(int millisecondTimeout)
        {
            return Task.Run(() => { Thread.Sleep(millisecondTimeout); });
        }

        private static void AsyncAndAwait()
        {
            //string result = GetStringContentAsync().Result;

            string result = GetStringContentSync();

            Console.WriteLine(result.Substring(0, 100));
        }

        private static async Task<string> GetStringContentAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://www.ifeng.com");
                return result;
            }
        }

        private static string GetStringContentSync()
        {
            using (HttpClient client = new HttpClient())
            {
                Task<string> task = client.GetStringAsync("http://www.ifeng.com");
                string result = task.Result;
                return result;
            }
        }

        private static void ParallelTaskBreak()
        {
            ParallelLoopResult result = Parallel.For(0, 20, (i, state) =>
            {
                if (i == 5)
                {
                    Console.WriteLine("Breaking loop");
                    state.Break();
                }
                Thread.Sleep(i);
                Console.WriteLine("Loop " + i);
                return;
            });
        }

        private static void ParallelTask()
        {
            Parallel.For(0, 10, i => { Thread.Sleep(1000); });

            IEnumerable<int> numbers = Enumerable.Range(0, 10);
            Parallel.ForEach(numbers, i => { Thread.Sleep(1000); });
        }

        private static void TaskWaitAny()
        {
            Task<int>[] tasks = new Task<int>[3];

            tasks[0] = Task<int>.Run(() => { Thread.Sleep(2000); return 1; });
            tasks[1] = Task<int>.Run(() => { Thread.Sleep(1000); return 2; });
            tasks[2] = Task<int>.Run(() => { Thread.Sleep(1000); return 3; });

            while (tasks.Length > 0)
            {
                int i = Task.WaitAny(tasks);
                Task<int> taskCompleted = tasks[i];

                Console.WriteLine(taskCompleted.Result);

                List<Task<int>> temp = tasks.ToList<Task<int>>();
                temp.RemoveAt(i);
                tasks = temp.ToArray();
            }
        }

        private static void TaskWaitAll()
        {
            Task[] taskArr = new Task[3];

            taskArr[0] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine(1);
                return 1;
            });
            taskArr[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine(2);
                return 2;
            });
            taskArr[2] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine(3);
                return 3;
            });

            Task.WaitAll(taskArr);
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
