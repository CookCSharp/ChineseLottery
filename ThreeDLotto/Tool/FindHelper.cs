/*
 *Description: FindHelper
 *Author: Chance.zheng
 *Creat Time: 2024/5/2 22:41:29
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDLotto
{
    public class FindHelper
    {
        private static int PeriodIndex = 3500; //3500

        /// <summary>
        /// 根据给出的单个球012路找出历史相同的规律
        /// </summary>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void Find012Path(int index, int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group => group[index - 1] % 3).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足{0}号球012路规律：{1} 的有以下几期", index, string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的单个球大小找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为大，0为小
        /// </remarks>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void FindBigSmall(int index, int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group =>
            {
                if (group[index - 1] >= 5)
                    return 1;
                else
                    return 0;
            }).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足{0}号球大小规律：{1} 的有以下几期", index, string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的单个球奇偶找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为奇数，0为偶数
        /// </remarks>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void FindOddEven(int index, int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group =>
            {
                if (group[index - 1] % 2 == 0)
                    return 0;
                else
                    return 1;
            }).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足{0}号球奇偶规律：{1} 的有以下几期", index, string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的单个球质合找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为质数，0为合数
        /// </remarks>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void FindPrimeComposite(int index, int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group =>
            {
                bool isComposite;
                if (group[index - 1] == 0)
                {
                    return 0;
                }
                else if (group[index - 1] >= 1 && group[index - 1] <= 2)
                {
                    return 1;
                }
                else
                {
                    isComposite = Enumerable.Range(2, (int)Math.Sqrt(group[index - 1])).Any(v => group[index - 1] % v == 0);
                    return isComposite ? 0 : 1;
                }
            }).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足{0}号球质合规律：{1} 的有以下几期", index, string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的最大值012路找出历史相同的规律
        /// </summary>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void FindMax012Path(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group => group.Max() % 3).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足最大值012路规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的最小值012路找出历史相同的规律
        /// </summary>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void FindMin012Path(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group => group.Min() % 3).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足最小值012路规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的中间值012路找出历史相同的规律
        /// </summary>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void FindMid012Path(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group => SelectHelper.CaculateMiddleNumber(group.ToArray()) % 3).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足中间值012路规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的和值012路找出历史相同的规律
        /// </summary>
        /// <param name="values"></param>
        public static void FindSum012Path(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group => group.Sum() % 3).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足和值012路规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的和值奇偶找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为奇数，0为偶数
        /// </remarks>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void FindSumOddEven(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group =>
            {
                if (group.Sum() % 2 == 0)
                    return 0;
                else
                    return 1;
            }).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足和值奇偶规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的和值尾012路找出历史相同的规律
        /// </summary>
        /// <param name="values"></param>
        public static void FindSumMantissa012Path(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group => group.Sum() % 10 % 3).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足和值尾012路规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的和值尾奇偶找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为奇数，0为偶数
        /// </remarks>
        /// <param name="values"></param>
        public static void FindSumMantissaOddEven(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group =>
            {
                if (group.Sum() % 10 % 2 == 0)
                    return 0;
                else
                    return 1;
            }).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足和值尾奇偶规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的和值尾大小找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为大，0为小
        /// </remarks>
        /// <param name="values"></param>
        public static void FindSumMantissaBigSmall(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group =>
            {
                if (group.Sum() % 10 >= 5)
                    return 1;
                else
                    return 0;
            }).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足和值尾大小规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的和值尾质合找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为质数，0为合数
        /// </remarks>
        /// <param name="values"></param>
        public static void FindSumMantissaPrimeComposite(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group =>
            {
                var sumMantissa = group.Sum() % 10;
                bool isComposite;
                if (sumMantissa == 0)
                {
                    return 0;
                }
                else if (sumMantissa >= 1 && sumMantissa <= 2)
                {
                    return 1;
                }
                else
                {
                    isComposite = Enumerable.Range(2, (int)Math.Sqrt(sumMantissa)).Any(v => sumMantissa % v == 0);
                    return isComposite ? 0 : 1;
                }
            }).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足和值尾质合规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的跨度012路找出历史相同的规律
        /// </summary>
        /// <param name="values"></param>
        public static void FindSpan012Path(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group => (group.Max() - group.Min()) % 3).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足跨度012路规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的跨度大小找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为大，0为小
        /// </remarks>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void FindSpanBigSmall(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group =>
            {
                if (group.Max() - group.Min() >= 5)
                    return 1;
                else
                    return 0;
            }).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足跨度大小规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的跨度奇偶找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为奇数，0为偶数
        /// </remarks>
        /// <param name="values"></param>
        public static void FindSpanOddEven(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group =>
            {
                if ((group.Max() - group.Min()) % 2 == 0)
                    return 0;
                else
                    return 1;
            }).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足跨度尾奇偶规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        /// <summary>
        /// 根据给出的跨度质合找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为质数，0为合数
        /// </remarks>
        /// <param name="values"></param>
        public static void FindSpanPrimeComposite(int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyData);

            var path012 = historyData.Values.Select(group =>
            {
                var span = group.Max() - group.Min();
                bool isComposite;
                if (span == 0)
                {
                    return 0;
                }
                else if (span >= 1 && span <= 2)
                {
                    return 1;
                }
                else
                {
                    isComposite = Enumerable.Range(2, (int)Math.Sqrt(span)).Any(v => span % v == 0);
                    return isComposite ? 0 : 1;
                }
            }).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足跨度质合规律：{0} 的有以下几期", string.Join(" ", values)));
            PrintHelper.PrintForecastResult(string.Join(" ", periodIndexes.Select(x => $"{historyData.ElementAt(x).Key}期")));
        }

        private static void Wait()
        {
            History.Instance.ResolveJsonFile();
            History.Instance.Wait();
        }

        private static void IsSubset(List<int> list1, List<int> list2, out List<int> index)
        {
            index = new List<int>();

            for (int i = 0; i <= list1.Count - list2.Count; i++)
            {
                if (list1.Skip(i).Take(list2.Count).SequenceEqual(list2))
                {
                    index.Add(i);
                }
            }
        }
    }
}
