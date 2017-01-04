/*
 * Reference: http://www.cnblogs.com/xuyi/archive/2012/10/25/2738911.html
 * http://pear.php.net/manual/en/package.console.console-progressbar.php
 */
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UpdateFolderFile
{
    public class ConsoleProgressBar
    {
        int left = 0;
        int backgroundLength = 50;

        ConsoleColor colorBack = Console.BackgroundColor;
        ConsoleColor colorFore = Console.ForegroundColor;

        private const int STD_OUTPUT_HANDLE = -11;
        private int mHConsoleHandle;
        COORD barCoord;

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
            public COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public int wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "GetConsoleScreenBufferInfo", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetConsoleScreenBufferInfo(int hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport("kernel32.dll", EntryPoint = "SetConsoleCursorPosition", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetConsoleCursorPosition(int hConsoleOutput, COORD dwCursorPosition);

        public void SetConsolePos(short x, short y)
        {
            SetConsoleCursorPosition(mHConsoleHandle, new COORD(x, y));
        }

        public COORD GetCursorPos()
        {
            CONSOLE_SCREEN_BUFFER_INFO res;
            GetConsoleScreenBufferInfo(mHConsoleHandle, out res);
            return res.dwCursorPosition;
        }

        public ConsoleProgressBar(string title, int left = 10)
        {
            Console.WriteLine();

            // 获取当前窗口句柄
            mHConsoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);

            // 获取当前窗体偏移量
            barCoord = this.GetCursorPos();

            this.left = left;

            // 获取字符长度
            int len = GetStringLength(title);

            // 设置标题的相对居中位置
            Console.SetCursorPosition(left + (backgroundLength / 2 - len), barCoord.Y);
            Console.Write(title);

            // 写入进度条背景
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(left, barCoord.Y + 1);

            for (int i = 0; ++i <= backgroundLength;)
            {
                Console.Write(" ");
            }

            Console.WriteLine();
            Console.BackgroundColor = colorBack;
        }

        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="current">当前进度</param>
        /// <param name="total">总进度</param>
        /// <param name="message">说明文字</param>
        public void Update(int current, int total, string message)
        {
            // 计算百分比
            int i = (int)Math.Ceiling(current / (double)total * 100);

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(left, barCoord.Y + 1);

            // 写进度条
            StringBuilder bar = new StringBuilder();

            // 当前百分比 * 进度条总长度 = 要输出的进度最小单位数量
            int count = (int)Math.Ceiling((double)i / 100 * backgroundLength);

            for (int n = 0; n < count; n++)
            {
                bar.Append(" ");
            }

            Console.Write(bar);

            // 设置和写入百分比
            Console.BackgroundColor = colorBack;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(left + backgroundLength, barCoord.Y + 1);
            Console.Write("{0}%", i);
            Console.ForegroundColor = colorFore;

            // 获取字符长度
            int len = GetStringLength(message);

            // 获取相对居中的 message 偏移量
            Console.SetCursorPosition(left + (backgroundLength / 2 - len), barCoord.Y + 2);
            Console.Write(message);

            // 进度完成另起新行最为输出
            if (i >= 100)
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 获取字符长度
        /// </summary>
        /// <param name="message">中文和全角占长度1，英文和半角字符2个字母占长度1</param>
        /// <returns></returns>
        private int GetStringLength(string message)
        {
            int len = Encoding.ASCII.GetBytes(message).Count(b => b == 63);
            return (message.Length - len) / 2 + len;
        }
    }
}
