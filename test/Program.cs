using System;
using System.Collections.Generic;
using System.Threading;

namespace test
{
    public class Matrix
    {
        static readonly object locker = new();
        List<char> chars = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
        Random random;
        public int Column { get; set; }
        public bool IsNeedSecond { get; set; }
        public Matrix(int col, bool isNeedSecond)
        {
            Column = col;
            random = new Random((int)DateTime.Now.Ticks);
            IsNeedSecond = isNeedSecond;
        }
        private char GetChar() => chars[random.Next(0, chars.Count)];
        public void Move()
        {
            int length;
            int count;
            while (true)
            {
                count = random.Next(3, 6);
                length = 0;
                Thread.Sleep(random.Next(20, 200));
                for (int i = 0; i < 40; i++)
                {
                    lock (locker)
                    {
                        Console.CursorTop = 0;
                        Console.ForegroundColor = ConsoleColor.Black;
                        for (int j = 0; j < i; j++)
                        {
                            Console.CursorLeft = Column;
                            Console.WriteLine(" ");
                        }
                        if (length < count) length++;
                        else
                            if (length == count) count = 0;
                        if (IsNeedSecond&&i<20&&i>length+2/*&&random.Next(1,4)==3*/)
                        {
                            new Thread((new Matrix(Column, false)).Move).Start();
                            IsNeedSecond = false;
                        }
                        if (39 - i < length) length--;
                        Console.CursorTop = i - length + 1;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        for (int j = 0; j < length-2; j++)
                        {
                            Console.CursorLeft = Column;
                            Console.WriteLine(GetChar());
                        }
                        if (length >= 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.CursorLeft = Column;
                            Console.WriteLine(GetChar());
                        }
                        if (length >= 1)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.CursorLeft = Column;
                            Console.WriteLine(GetChar());
                        }
                        Thread.Sleep(20);
                    }
                }
            }
        }
    }
    delegate string MyDelegate(string x);
    class Program
    {
        static int deep = 0;
        static void Recursion()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name}, say \"HELLO\"");
            Thread.Sleep(500);
            Thread thread = new(Recursion);
            deep++;
            thread.Name = "Thread " + deep;
            thread.Start();
        }
        static void Main(string[] args)
        {
            Console.SetWindowSize(80, 40);
            Matrix matrix;
            for (int i = 0; i < 30; i++)
            {
                matrix = new(i * 2,true);
                new Thread(matrix.Move).Start();
            }
            Console.ReadKey();
        }
    }
}
