/*
 *Description: PrintHelper
 *Author: Chance.zheng
 *Creat Time: 2024/5/2 10:36:19
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDLotto
{
    public class PrintHelper
    {
        public static void PrintForecastResult(string str, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
        }

        public static void PrintVerifyResult(string str, ConsoleColor color = ConsoleColor.DarkYellow)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
        }

        public static void Print(string str, ConsoleColor color = ConsoleColor.DarkRed)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        public static void Print(string str, IEnumerable<int> nums, ConsoleColor color = ConsoleColor.DarkRed)
        {
            Console.WriteLine(str.Replace("[]", nums.Count().ToString()));
            Print(string.Join(' ', nums), color);
        }
    }
}
