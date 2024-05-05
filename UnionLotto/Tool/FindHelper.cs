/*
 *Description: FindHelper
 *Author: Chance.zheng
 *Creat Time: 2024/5/5 14:43:36
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
    public class FindHelper
    {
        private static int PeriodIndex = 3500; //3500

        /// <summary>
        /// 根据给出的奇偶找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为奇数，0为偶数
        /// </remarks>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void FindOddEven(int index, int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyRedData, out var historyData);

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
        /// 根据给出的质合找出历史相同的规律
        /// </summary>
        /// <remarks>
        /// 规定1为质数，0为合数
        /// </remarks>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void FindPrimeComposite(int index, int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyRedData, out var historyData);

            var path012 = historyData.Values.Select(group =>
            {
                bool isComposite;
                if (group[index - 1] >= 1 && group[index - 1] <= 2)
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
        /// 根据给出的单个球012路找出历史相同的规律
        /// </summary>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public static void Find012Path(int index, int[] values)
        {
            Wait();

            History.Instance.GetData(PeriodIndex, out var historyRedData, out var historyData);

            var path012 = historyData.Values.Select(group => group[index - 1] % 3).ToList();
            IsSubset(path012, values.ToList(), out List<int> periodIndexes);

            PrintHelper.PrintForecastResult(string.Format("满足{0}号球012路规律：{1} 的有以下几期", index, string.Join(" ", values)));
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
