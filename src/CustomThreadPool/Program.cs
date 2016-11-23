using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace CustomThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadStart[] startArray = {
                new ThreadStart(() => { Console.WriteLine("第一个任务"); }),
                new ThreadStart(() => { Console.WriteLine("第二个任务"); }),
                new ThreadStart(() => { Console.WriteLine("第三个任务"); }),
                new ThreadStart(() => { Console.WriteLine("第四个任务"); }),
                new ThreadStart(() => { Console.WriteLine("第五个任务"); })
            };
            MyThreadPool.SetMaxWorkerThreadCount(2);
            MyThreadPool.MyQueueUserWorkItem(startArray);
            Console.ReadKey();
        }
    }

    /// <summary>
    /// 自定义一个简单的线程池，该线程池实现了默认开启线程数，
    /// 当最大线程数全部在繁忙时，循环等待，直到至少一个线程空闲为止，
    /// 本示例使用 BackgroundWorker 模拟后台线程，任务将自动进入队列和离开队列
    /// </summary>
    static class MyThreadPool
    {
        // 线程锁对象
        private static object lockObj = new object();

        // 任务队列
        private static Queue<ThreadStart> threadStartQueue = new Queue<ThreadStart>();

        // 记录当前工作的任务集合，从中可以判断当前工作线程使用数，
        // 如果使用 int 判断的话可能会有问题，用集合的话还能取得对象的引用，比较好
        private static HashSet<ThreadStart> threadsWorker = new HashSet<ThreadStart>();

        // 当前允许最大工作线程数
        private static int maxThreadWorkerCount = 1;

        // 当前允许最小工作线程数
        private static int minThreadWorkerCount = 0;

        /// <summary>
        /// 设置最大工作线程数
        /// </summary>
        /// <param name="maxThreadCount">数量</param>
        public static void SetMaxWorkerThreadCount(int maxThreadCount)
        {
            maxThreadWorkerCount = maxThreadCount > minThreadWorkerCount ? maxThreadCount : minThreadWorkerCount;
        }

        /// <summary>
        ///  设置最小工作线程数
        /// </summary>
        /// <param name="minThreadCount">数量</param>
        public static void SetMinWorkerThreadCount(int minThreadCount)
        {
            minThreadWorkerCount = minThreadCount > maxThreadWorkerCount ? maxThreadWorkerCount : minThreadCount; 
        }

        /// <summary>
        /// 启动线程池工作
        /// </summary>
        /// <param name="threadStartArray">任务数组</param>
        public static void MyQueueUserWorkItem(ThreadStart[] threadStartArray)
        {
            // 将任务集合都放入到线程池中
            AddAllThreadToPool(threadStartArray);

            // 线程池执行任务
            ExecuteTask();
        }

        /// <summary>
        /// 将单一任务加入队列中
        /// </summary>
        /// <param name="ts">单一任务对象</param>
        private static void AddThreadToPool(ThreadStart ts)
        {
            lock (lockObj)
            {
                threadStartQueue.Enqueue(ts);

            }
        }

        /// <summary>
        /// 将多个任务加入到线程池的任务队列中
        /// </summary>
        /// <param name="threadStartArray">多个任务</param>
        private static void AddAllThreadToPool(ThreadStart[] threadStartArray)
        {
            foreach (var threadStart in threadStartArray)
            {
                AddThreadToPool(threadStart);
            }
        }

        /// <summary>
        /// 执行任务，判断队列中的任务数量是否大于0，
        /// 如果是则判断当前正在使用的工作线程的数量是否大于等于允许的最大工作线程数，
        /// 如果一旦有空闲线程的话就会执行 ExecuteTaskInQueue 方法处理任务
        /// </summary>
        private static void ExecuteTask()
        {
            while (threadStartQueue.Count > 0)
            {
                if (threadsWorker.Count < maxThreadWorkerCount)
                {
                    ExecuteTaskInQueue();
                }
            }
        }

        /// <summary>
        /// 执行出队列的任务，加锁保护
        /// </summary>
        private static void ExecuteTaskInQueue()
        {
            lock (lockObj)
            {
                ExecuteTaskByThread(threadStartQueue.Dequeue());
            }
        }

        /// <summary>
        /// 实现细节，这里使用 BackgroundWorker 来实现后台线程，
        /// 注册 DoWork 和 Completed 事件，当执行一个任务前，将任务加入到工作任务集合（表示工作线程少了一个空闲），
        /// 一旦 RunWorkerCompleted 事件被出发则将任务从工作任务集合中移除（表示工作线程也空闲了一个）
        /// </summary>
        /// <param name="threadStart"></param>
        private static void ExecuteTaskByThread(ThreadStart threadStart)
        {
            threadsWorker.Add(threadStart);
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, e) => { threadStart.Invoke(); };
            worker.RunWorkerCompleted += (o, e) => { threadsWorker.Remove(threadStart); };
            worker.RunWorkerAsync();
        }
    }
}
