/*
 *Description: VerifyHelper
 *Author: Chance.zheng
 *Creat Time: 2024/5/2 10:14:16
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
    public class VerifyHelper
    {
        private static bool _isViewDetail = false; //是否查看详细信息
        private static int[] _historyPeriodsViewDetail = new int[] { 100 }; //查看详细信息
        private static int PrintPeriodIndex = 101; //打印100期以内的详细信息
        private static int[] _historyPeriods = new int[] { 10, 20, 50, 100, 1000 }; //查看统计信息


        /// <summary>
        /// 通过大批量历史数据验证各种模型
        /// </summary>
        /// <param name="isContainRecentLotto">是否验证包含最近一次开奖结果</param>
        public static void VerifyPastResults()
        {
            History.Instance.ResolveJsonFile();
            History.Instance.Wait();

            VerifyIncludeByDistance5();
            VerifyIncludeByPreLotto();

            //VerifyIncludeByUnits();
            //VerifyIncludeByTens();
            //VerifyIncludeByHundreds();

            //VerifyExcludeByTens();

            //PrintHelper.PrintVerifyResult("综合测试结果得出结论如下：", ConsoleColor.Green);
            //Console.WriteLine();
            //Summary.ToList().ForEach(x =>
            //{
            //    PrintHelper.PrintVerifyResult(x, ConsoleColor.Green);
            //    Console.WriteLine();
            //});
        }

        /// <summary>
        /// 验证差5定胆法
        /// </summary>
        public static void VerifyIncludeByDistance5()
        {
            InternalVerifyInclude(() => SelectHelper.CaculateDistance5(false), "差5定胆法");
        }

        /// <summary>
        /// 验证上期号码中下期法
        /// </summary>
        public static void VerifyIncludeByPreLotto()
        {
            InternalVerifyInclude(() => SelectHelper.CaculateHundredsTensUnits(false), "上期号码中下期法");
        }

        /// <summary>
        /// 验证个位计算胆号法
        /// </summary>
        public static void VerifyIncludeByUnits()
        {
            InternalVerifyInclude(() => SelectHelper.CaculateIncludeNumberByUnits(false), "个位计算胆号法");
        }

        /// <summary>
        /// 验证十位计算胆号法
        /// </summary>
        public static void VerifyIncludeByTens()
        {
            InternalVerifyInclude(() => SelectHelper.CaculateIncludeNumberByTens(false), "十位计算胆号法");
        }

        /// <summary>
        /// 验证百位计算胆号法
        /// </summary>
        public static void VerifyIncludeByHundreds()
        {
            InternalVerifyInclude(() => SelectHelper.CaculateIncludeNumberByHundreds(false), "百位计算胆号法");
        }

        /// <summary>
        /// 十位杀胆号法
        /// </summary>
        public static void VerifyExcludeByTens()
        {
            InternalVerifyExclude(() => [SelectHelper.CaculateExcludeNumberByTens(false)]);
        }

        private static void InternalVerifyInclude(Func<IList<int>> func, string methodName)
        {
            var historyPeriods = _isViewDetail ? _historyPeriodsViewDetail : _historyPeriods;
            for (int i = 0; i < historyPeriods.Length; i++)
            {
                History.Instance.GetData(historyPeriods[i] + 1, out var historyData);

                var resultDetails = new LinkedList<string>();
                var resultCount = new LinkedList<int>();

                for (int j = 0; j < historyData.Count - 1; j++)
                {
                    var lastPeriodLotto = historyData.ElementAt(j).Value;
                    var nextPeriodLotto = historyData.ElementAt(j + 1).Value;
                    var nextPeriod = historyData.ElementAt(j + 1).Key;

                    Data.SetLotto(lastPeriodLotto);
                    var res = func();

                    var contains_Result = nextPeriodLotto.Where(n => res.Contains(n));
                    resultCount.AddLast(contains_Result.Count());
                }

                if (_isViewDetail)
                {
                    //以下为打印详细结果所用
                    resultDetails.ToList().ForEach(r => PrintHelper.Print(r, ConsoleColor.DarkGreen));
                }
                else
                {
                    var startPeriod = historyData.ElementAt(1).Key;
                    var endPeriod = historyData.Last().Key;
                    var groupDictionary = new Dictionary<string, double>();
                    PrintHelper.PrintVerifyResult($"{startPeriod}-{endPeriod}总共{resultCount.Count}组数据，{methodName}测试的结果如下：");
                    for (int k = 0; k <= 3; k++)
                    {
                        var count = resultCount.Where(n => n == k).Count();
                        var message = string.Format("命中{0}个：共{1}组，概率：{2}%", k, count, Math.Round((double)count / resultCount.Count() * 100D, 2));
                        groupDictionary.Add($"{historyPeriods[i]}-{k}", Math.Round((double)count / resultCount.Count() * 100D, 2));
                        PrintHelper.PrintVerifyResult(message);
                    }

                    if (historyData.Count <= PrintPeriodIndex)
                        PrintHelper.PrintVerifyResult(string.Format("命中个数依次为：{0}", string.Join(" ", resultCount)));
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void InternalVerifyExclude(Func<IList<int>> func)
        {
            var historyPeriods = _isViewDetail ? _historyPeriodsViewDetail : _historyPeriods;
            for (int i = 0; i < historyPeriods.Length; i++)
            {
                History.Instance.GetData(historyPeriods[i] + 1, out var historyData);

                var resultDetails = new LinkedList<string>();
                var resultCount = new LinkedList<int>();

                for (int j = 0; j < historyData.Count - 1; j++)
                {
                    var lastPeriodLotto = historyData.ElementAt(j).Value;
                    var nextPeriodLotto = historyData.ElementAt(j + 1).Value;
                    var nextPeriod = historyData.ElementAt(j + 1).Key;

                    Data.SetLotto(lastPeriodLotto);
                    var res = func();

                    if (res.Count > 0)
                    {
                        var isContain = nextPeriodLotto.Any(n => res.Contains(n)) ? "错误" : "正确";
                        resultDetails.AddLast($"{nextPeriod}期开奖号码为：{string.Join(" ", nextPeriodLotto)}，排除号码：{string.Join(" ", res)}，是否正确：{isContain}");
                    }
                }

                if (_isViewDetail)
                {
                    //以下为打印详细结果所用
                    resultDetails.ToList().ForEach(r => PrintHelper.Print(r, ConsoleColor.DarkGreen));
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void InternalVerify(Func<IList<int>> func, string methodName)
        {
            var historyPeriods = _isViewDetail ? _historyPeriodsViewDetail : _historyPeriods;
            for (int i = 0; i < historyPeriods.Length; i++)
            {
                History.Instance.GetData(historyPeriods[i] + 1, out var historyData);

                var resultDetails = new LinkedList<string>();
                var resultCount = new LinkedList<int>();

                for (int j = 0; j < historyData.Count - 1; j++)
                {
                    var lastPeriodLotto = historyData.ElementAt(j).Value;
                    var nextPeriodLotto = historyData.ElementAt(j + 1).Value;
                    var nextPeriod = historyData.ElementAt(j + 1).Key;

                    Data.SetLotto(lastPeriodLotto);
                    var res = func();

                    var isContain = nextPeriodLotto.Any(n => res.Contains(n)) ? "错误" : "正确";
                    resultDetails.AddLast($"{nextPeriod}期开奖号码为：{string.Join(" ", nextPeriodLotto)}，排除号码：{string.Join(" ", res)}，是否正确：{isContain}");

                    //var contains_Result = nextPeriodLotto.Where(n => res.Contains(n));
                    //resultCount.AddLast(contains_Result.Count());
                    //resultDetails.AddLast($"{nextPeriod}期开奖号码为：{string.Join(" ", correct)}，共命中{contains_Result.Count()}个，分别是：{string.Join(" ", contains_Result)}");
                }

                if (_isViewDetail)
                {
                    //以下为打印详细结果所用
                    resultDetails.ToList().ForEach(r => PrintHelper.Print(r, ConsoleColor.DarkGreen));
                }
                else
                {
                    var startPeriod = historyData.ElementAt(1).Key;
                    var endPeriod = historyData.Last().Key;
                    var groupDictionary = new Dictionary<string, double>();
                    PrintHelper.PrintVerifyResult($"{startPeriod}-{endPeriod}总共{resultCount.Count}组数据，{methodName}测试的结果如下：");
                    for (int k = 0; k <= 6; k++)
                    {
                        var count = resultCount.Where(n => n == k).Count();
                        var message = string.Format("命中{0}个：共{1}组，概率：{2}%", k, count, Math.Round((double)count / resultCount.Count() * 100D, 2));
                        groupDictionary.Add($"{historyPeriods[i]}-{k}", Math.Round((double)count / resultCount.Count() * 100D, 2));
                        PrintHelper.PrintVerifyResult(message);
                    }

                    if (historyData.Count <= PrintPeriodIndex)
                        PrintHelper.PrintVerifyResult(string.Format("命中个数依次为：{0}", string.Join(" ", resultCount)));
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
