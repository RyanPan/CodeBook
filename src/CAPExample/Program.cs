using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
参考：http://www.cnblogs.com/xiaofengfeng/archive/2012/12/21/2828387.html
*/

namespace CAPExample
{
    class Program
    {
        //定义一个委托
        public delegate void DoSomething();

        static void Main(string[] args)
        {
            //1.实例化一个委托，调用者发送一个请求，请求执行该方法体（还未执行）
            DoSomething doSomething = new DoSomething(
                () =>
                {
                    Console.WriteLine("如果委托使用 beginInvoke 的话，这里便是异步方法体");
                    //4，实现完这个方法体后自动触发下面的回调函数方法体
                }
            );

            //3 。调用者（主线程）去触发异步调用，采用异步的方式请求上面的方法体
            IAsyncResult result = doSomething.BeginInvoke(
                //2.自定义上面方法体执行后的回调函数
                new AsyncCallback
                (
                    //5.以下是回调函数方法体
                    //asyncResult.AsyncState其实就是AsyncCallback委托中的第二个参数
                    asyncResult =>
                    {
                        doSomething.EndInvoke(asyncResult);
                        Console.WriteLine(asyncResult.AsyncState.ToString());
                    }
                ),
                "AsyncResult.AsyncState"
            );
            //DoSomething......调用者（主线程）会去做自己的事情
            Console.ReadKey();
        }
    }
}
