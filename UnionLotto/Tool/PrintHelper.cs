/*
 *Description: PrintHelper
 *Author: Chance.zheng
 *Creat Time: 2024/4/8 16:13:30
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionLotto
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

        /// <summary>
        /// 打印前言
        /// </summary>
        public static void PrintForeword()
        {
            Console.WriteLine(string.Format("上一期({0}期)开奖号码：{1}", Data.CurrentPeriod - 1, Data.PreRedBlueLotto.ToD2String()));
            //Console.WriteLine(string.Format("上一期({0}期)开奖红色号码：{1}", Data.CurrentPeriod - 1, Data.PreRedLotto.Print()));
            //Console.WriteLine(string.Format("上一期({0}期)开奖蓝色号码：{1}", Data.CurrentPeriod - 1, Data.PreBlueLotto));
            Console.WriteLine(string.Format("本期({0}期)预测号码如下：", Data.CurrentPeriod));
            Console.WriteLine();

            //Print("每期开奖红色号码中以下9个号码至少1个出现(概率很大，但可能不准)：", Data.EveryHaveNums);
        }
    }
}
