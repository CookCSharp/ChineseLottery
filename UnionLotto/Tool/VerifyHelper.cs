/*
 *Description: ResultHelper
 *Author: Chance.zheng
 *Creat Time: 2024/4/8 19:42:33
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnionLotto.Tool
{
    /// <summary>
    /// 开奖结果验证
    /// </summary>
    /// <remarks>
    /// 对于某一规律或模型，在大批量历史开奖结果中进行概率统计与验证
    /// </remarks>
    public class VerifyHelper
    {
        private static int PrintPeriodIndex = 51; //打印50期以内的详细信息
        private static bool _isViewDetail = false; //是否查看详细信息
        private static int[] _historyPeriodsViewDetail = new int[] { 10 }; //查看详细信息
        private static int[] _historyPeriods = new int[] { 10, 20, 50, 100, 1000 }; //查看统计信息
        private static LinkedList<string> Summary = new LinkedList<string>();

        /// <summary>
        /// 通过大批量历史数据验证各种模型
        /// </summary>
        /// <param name="isContainRecentLotto">是否验证包含最近一次开奖结果</param>
        public static void VerifyPastResults(bool isContainRecentLotto)
        {
            if (!isContainRecentLotto)
                History.Instance.StartGuess();
            else
                History.Instance.EndGuess();

            History.Instance.ResolveJsonFile();
            History.Instance.Wait();

            VerifyPlusAndSubtract();
            VerifySumDivision();
            Verify9And11();
            VerifyEveryPeriodHave();
            VerifyPrimeNum();

            VerifyMantissa();
            VerifyMiddle();
            VerifyGoldedCut();

            //VerifyPassword();

            //PrintHelper.PrintVerifyResult("综合测试结果得出结论如下：", ConsoleColor.Green);
            //Console.WriteLine();
            //Summary.ToList().ForEach(x =>
            //{
            //    PrintHelper.PrintVerifyResult(x, ConsoleColor.Green);
            //    Console.WriteLine();
            //});
        }

        /// <summary>
        /// 验证两两加减法
        /// </summary>
        public static void VerifyPlusAndSubtract()
        {
            InternalVerify(() => SelectHelper.CalculatePlusAndSubtractLotto(false), "两两加减计算法");
        }

        /// <summary>
        /// 验证和值取商计算法
        /// </summary>
        public static void VerifySumDivision()
        {
            InternalVerify(() => SelectHelper.CalculateSumDivisionLotto(false), "和值取商计算法");
        }

        /// <summary>
        /// 验证9、11进制法
        /// </summary>
        public static void Verify9And11()
        {
            //InternalVerifyZero(() => SelectHelper.Calculate9And11Lotto(9, false), "9进制计算法");
            //InternalVerifyZero(() => SelectHelper.Calculate9And11Lotto(11, false), "11进制计算法");

            InternalVerify(() => SelectHelper.Calculate9And11Lotto(9, false), "9进制计算法");
            InternalVerify(() => SelectHelper.Calculate9And11Lotto(11, false), "11进计算制法");
        }

        /// <summary>
        /// 验证每期必有号码法
        /// </summary>
        public static void VerifyEveryPeriodHave()
        {
            //InternalVerifyZero(() => Data.EveryHaveNums, "每期必有号码法");

            InternalVerify(() => Data.EveryHaveNums, "每期必有号码法");
        }

        /// <summary>
        /// 验证质数法
        /// </summary>
        public static void VerifyPrimeNum()
        {
            //InternalVerifyZero(() => Data.AllPrimeNums, "质数法");

            InternalVerify(() => Data.AllPrimeNums, "质数法");
        }

        /// <summary>
        /// 验证红色号码尾数定胆计算法
        /// </summary>
        public static void VerifyMantissa()
        {
            //IList<int> GetProbableMantissa()
            //{
            //    var data = new List<int>();
            //    var mantissa = SelectHelper.CalculateProbableMantissa(false);
            //    foreach (var n in mantissa)
            //    {
            //        Get(data, n);
            //    }

            //    void Get(List<int> data, int n)
            //    {
            //        if (n < 33)
            //        {
            //            data.Add(n);
            //            n += 10;
            //            Get(data, n);
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }

            //    return data;
            //}

            //InternalVerifyZero(() => SelectHelper.CalculateProbableMantissa(false), "尾数定胆法", true);
            InternalVerify(() => SelectHelper.CalculateProbableMantissa(false), "尾数定胆法", true);
        }

        /// <summary>
        /// 验证红色号码中间数定胆计算法
        /// </summary>
        public static void VerifyMiddle()
        {
            //InternalVerifyZero(() => SelectHelper.CalulateProbableMiddle(false), "中间数定胆法");
            InternalVerify(() => SelectHelper.CalulateProbableMiddle(false), "中间数定胆法");
        }

        /// <summary>
        /// 验证红色号码黄金分割定胆计算法
        /// </summary>
        public static void VerifyGoldedCut()
        {
            //InternalVerifyZero(() => SelectHelper.CalulateProbableGoldedCut(false), "黄金分割定胆法");
            InternalVerify(() => SelectHelper.CalulateProbableGoldedCut(false), "黄金分割定胆法");
        }

        /// <summary>
        /// 验证明暗点计算法
        /// </summary>
        public static void VerifyPassword()
        {
            InternalVerifyPassword(() => SelectHelper.CalulatePassword(false), "明暗点计算法");
            //InternalVerify(() => SelectHelper.CalulatePassword(false), "明暗点计算法");
        }

        private static void InternalVerifyZero(Func<IList<int>> func, string methodName, bool isCalculateMantissa = false)
        {
            var historyPeriods = _isViewDetail ? _historyPeriodsViewDetail : _historyPeriods;
            for (int i = 0; i < historyPeriods.Length; i++)
            {
                History.Instance.GetData(historyPeriods[i] + 1, out var historyRedData, out var historyData);

                var resultDetails = new LinkedList<string>();
                var resultCount = new LinkedList<int>();
                var resultDictionary = new HashSet<string>();

                for (int j = 0; j < historyData.Count - 1; j++)
                {
                    var lastPeriodLotto = historyData.ElementAt(j).Value;
                    var nextPeriodLotto = historyRedData.ElementAt(j + 1).Value;
                    var nextPeriod = historyData.ElementAt(j + 1).Key;
                    var correct = historyData.ElementAt(j + 1).Value;

                    Data.SetRedBlueLotto(lastPeriodLotto);
                    var res = func();

                    IEnumerable<int> contains_Result;
                    if (isCalculateMantissa)
                    {
                        contains_Result = nextPeriodLotto.Where(n => res.Contains(n % 10));
                        resultCount.AddLast(contains_Result.DistinctBy(n => n % 10).Count());
                        resultDetails.AddLast($"{nextPeriod}期开奖号码为：{string.Join(" ", correct.Select(n => n.ToString("D2")))}，共命中{contains_Result.DistinctBy(n => n % 10).Count()}个，分别是：{string.Join(" ", contains_Result.Select(n => n.ToString("D2")))}");
                    }
                    else
                    {
                        contains_Result = nextPeriodLotto.Where(n => res.Contains(n));
                        resultCount.AddLast(contains_Result.Count());
                        resultDetails.AddLast($"{nextPeriod}期开奖号码为：{string.Join(" ", correct.Select(n => n.ToString("D2")))}，共命中{contains_Result.Count()}个，分别是：{string.Join(" ", contains_Result.Select(n => n.ToString("D2")))}");
                    }

                    if (contains_Result.Count() == 0 && historyData.Count <= PrintPeriodIndex)
                        resultDictionary.Add(nextPeriod);
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
                    var zeroCount = resultCount.Where(n => n == 0).Count();
                    var message = string.Format("一次都未命中的共{0}组，概率为{1}%", zeroCount, Math.Round((double)zeroCount / resultCount.Count() * 100D, 2));
                    PrintHelper.PrintVerifyResult($"{startPeriod}-{endPeriod}总共{resultCount.Count}组数据，{methodName}" + message);

                    if (historyData.Count <= PrintPeriodIndex)
                        PrintHelper.PrintVerifyResult(string.Format("命中个数依次为：{0}", string.Join(" ", resultCount)));

                    if (resultDictionary.Count() > 0 && historyData.Count <= PrintPeriodIndex)
                        PrintHelper.PrintVerifyResult(string.Format("未命中期数依次为：{0}", string.Join("、", resultDictionary.OrderByDescending(s => s))));
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void InternalVerify(Func<IList<int>> func, string methodName, bool isCalculateMantissa = false)
        {
            var historyPeriods = _isViewDetail ? _historyPeriodsViewDetail : _historyPeriods;
            var resultDictionary = new List<string>();
            for (int i = 0; i < historyPeriods.Length; i++)
            {
                History.Instance.GetData(historyPeriods[i] + 1, out var historyRedData, out var historyData);

                var resultDetails = new LinkedList<string>();
                var resultCount = new LinkedList<int>();

                for (int j = 0; j < historyData.Count - 1; j++)
                {
                    var lastPeriodLotto = historyData.ElementAt(j).Value;
                    var nextPeriodLotto = historyRedData.ElementAt(j + 1).Value;
                    var nextPeriod = historyData.ElementAt(j + 1).Key;
                    var correct = historyData.ElementAt(j + 1).Value;

                    Data.SetRedBlueLotto(lastPeriodLotto);
                    var res = func();

                    IEnumerable<int> contains_Result;
                    if (isCalculateMantissa)
                    {
                        contains_Result = nextPeriodLotto.Where(n => res.Contains(n % 10));
                        resultCount.AddLast(contains_Result.DistinctBy(n => n % 10).Count());
                        resultDetails.AddLast($"{nextPeriod}期开奖号码为：{string.Join(" ", correct.Select(n => n.ToString("D2")))}，共命中{contains_Result.DistinctBy(n => n % 10).Count()}个，分别是：{string.Join(" ", contains_Result.Select(n => n.ToString("D2")))}");
                    }
                    else
                    {
                        contains_Result = nextPeriodLotto.Where(n => res.Contains(n));
                        resultCount.AddLast(contains_Result.Count());
                        resultDetails.AddLast($"{nextPeriod}期开奖号码为：{string.Join(" ", correct.Select(n => n.ToString("D2")))}，共命中{contains_Result.Count()}个，分别是：{string.Join(" ", contains_Result.Select(n => n.ToString("D2")))}");
                    }
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

                    //统计信息中同概率的数未列举出来???
                    var max = groupDictionary.MaxBy(x => x.Value);
                    resultDictionary.Add(historyPeriods[i] + $"组测试数据命中{max.Key.Split('-')[1]}个的概率最高{max.Value}%");
                }
            }

            Summary.AddLast($"对于{methodName}:" + Environment.NewLine + string.Join(Environment.NewLine, resultDictionary));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void InternalVerifyPassword(Func<IList<int>> func, string methodName)
        {
            var historyPeriods = _isViewDetail ? _historyPeriodsViewDetail : _historyPeriods;
            for (int i = 0; i < historyPeriods.Length; i++)
            {
                History.Instance.GetData(historyPeriods[i] + 1, out var historyRedData, out var historyData);

                var resultDetails = new List<string>();
                var resultCounts = new List<int>();
                var resultDictionary = new HashSet<string>();

                for (int j = 0; j < historyData.Count - 1; j++)
                {
                    var lastPeriodLotto = historyData.ElementAt(j).Value;
                    var nextPeriodRedLotto = historyRedData.ElementAt(j + 1).Value;
                    var nextPeriod = historyData.ElementAt(j + 1).Key;
                    var correct = historyData.ElementAt(j + 1).Value.Select(n => n.ToString("D2"));

                    Data.SetRedBlueLotto(lastPeriodLotto);
                    var res = func();

                    //var contains_Result = nextPeriodRedLotto.Where(n => res.Contains(n));
                    //resultCounts.Add(contains_Result.Count());

                    var num1 = nextPeriodRedLotto.Take(3).Sum() % 10;
                    var num2 = nextPeriodRedLotto.Skip(3).Sum() % 10;

                    resultDictionary.Add(string.Join("/", res.Take(2)));

                    //if (contains_Result.Count() == 0 && historyData.Count <= PrintPeriodIndex)
                    //    resultDictionary.Add(nextPeriod);


                    //以下为打印详细结果所用
                    //resultDetails.Add($"{nextPeriod}期开奖号码为：{string.Join(" ", correct.Select(n => n.ToString("D2")))}，共命中{contains_Result.Count()}个，明暗点：{string.Join("/", res.Take(2))}，明暗点尾数：{string.Join(" ", res.Skip(2))}");
                    //resultDetails.Add($"{nextPeriod}期开奖号码为：{string.Join(" ", correct.Select(n => n.ToString("D2")))}，合值分别为：{num1}、{num2}");
                    resultDetails.Add(string.Format("{0}期开奖号码：{1}，合值分别为：{2}、{3}，明暗点：{4}，明暗点尾数：{5}", nextPeriod, string.Join(" ", correct), num1, num2, string.Join("/", res.Take(2)), string.Join(" ", res.Skip(2))));
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
                    var zeroCount = resultCounts.Where(n => n == 0).Count();
                    var message = string.Format("一次都未命中的共{0}组，概率为{1}%", zeroCount, Math.Round((double)zeroCount / resultCounts.Count() * 100D, 2));
                    PrintHelper.PrintVerifyResult($"{startPeriod}-{endPeriod}总共{resultCounts.Count}组数据，{methodName}" + message);

                    if (historyData.Count <= PrintPeriodIndex)
                        PrintHelper.PrintVerifyResult(string.Format("命中明暗点数依次为：{0}", string.Join(" ", resultCounts.Select(g => string.Join("/", g)))));

                    if (resultDictionary.Count() > 0 && historyData.Count <= PrintPeriodIndex)
                        PrintHelper.PrintVerifyResult(string.Format("未命中期数依次为：{0}", string.Join("、", resultDictionary.OrderByDescending(s => s))));
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
