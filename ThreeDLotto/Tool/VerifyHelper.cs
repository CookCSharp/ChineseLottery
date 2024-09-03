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
        private static readonly bool _isViewDetail = false; //是否查看详细信息
        private static readonly int[] _historyPeriodsViewDetail = new int[] { 100 }; //查看详细信息
        private static readonly int PrintPeriodIndex = 201; //打印100期以内的详细信息
        private static readonly int[] _historyPeriods = new int[] { 10, 50, 100, 200, 1000 }; //查看统计信息


        /// <summary>
        /// 通过大批量历史数据验证各种模型
        /// </summary>
        /// <param name="isContainRecentLotto">是否验证包含最近一次开奖结果</param>
        public static void VerifyPastResults()
        {
            History.Instance.ResolveJsonFile();
            History.Instance.Wait();

            //VerifyAllIncludeByTwoPowerful();
            //VerifyExcludeByPower();
            //VerifyExcludeBySum();
            //VerifyExcludeBySpanSumMantissa();
            //VerifyExcludeByPeriodTwoMantissa();
            //VerifyExcludeByPeriodAndSumMantissa();
            //VerifyExcludeByLastPeriodCompostion();

            //VerifyExcludeByPeriodMantissa();
            //VerifyExcludeByRelation();


            //VerifyIncludeByDistance5();
            //VerifyIncludeByPreLotto();

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
            InternalVerifyInclude(() => SelectHelper.CalculateDistance5(false), "差5定胆法");
        }

        /// <summary>
        /// 验证上期号码中下期法
        /// </summary>
        public static void VerifyIncludeByPreLotto()
        {
            InternalVerifyInclude(() => SelectHelper.CalculateHundredsTensUnits(false), "上期号码中下期法");
        }

        /// <summary>
        /// 验证个位计算胆号法
        /// </summary>
        public static void VerifyIncludeByUnits()
        {
            InternalVerifyInclude(() => SelectHelper.CalculateIncludeNumberByUnits(false), "个位计算胆号法");
        }

        /// <summary>
        /// 验证十位计算胆号法
        /// </summary>
        public static void VerifyIncludeByTens()
        {
            InternalVerifyInclude(() => SelectHelper.CalculateIncludeNumberByTens(false), "十位计算胆号法");
        }

        /// <summary>
        /// 验证百位计算胆号法
        /// </summary>
        public static void VerifyIncludeByHundreds()
        {
            InternalVerifyInclude(() => SelectHelper.CalculateIncludeNumberByHundreds(false), "百位计算胆号法");
        }

        /// <summary>
        /// 定胆法
        /// </summary>
        public static void VerifyIncludeByProbableNumbers()
        {
            History.Instance.GetData(2, out var historyData);
            int count = 0;
            for (int j = 0; j < historyData.Count - 1; j++)
            {
                var lastPeriodLotto = historyData.ElementAt(j).Value;
                var nextPeriodLotto = historyData.ElementAt(j + 1).Value;

                Data.SetLotto(lastPeriodLotto);
                var relation_number = SelectHelper.CalculateProbableNumbers(lastPeriodLotto.ToArray());
                var relation_count = nextPeriodLotto.Count(n => relation_number.Contains(n));
                var isContain = relation_count >= 1;
                if (isContain) count++;
            }

            PrintHelper.Print(string.Format("定胆法准确率为{0}%", (double)count / (historyData.Count - 1) * 100), ConsoleColor.DarkGreen);
        }

        /// <summary>
        /// 十位杀胆号法
        /// </summary>
        public static void VerifyExcludeByTens()
        {
            InternalVerifyExclude(() => [SelectHelper.CalculateExcludeNumberByTens(false)]);
        }

        public static void VerifyAllIncludeByTwoPowerful()
        {
            History.Instance.GetData(2000, out var historyData);
            int count = 0;
            for (int j = 0; j < historyData.Count; j++)
            {
                var lastPeriodLotto = historyData.ElementAt(j).Value;

                var isContain = Data.AllTwoLottos.Any(two => lastPeriodLotto.Count(n => two.Contains(n)) == 2);
                if (isContain) count++;
            }

            PrintHelper.Print(string.Format("万能2码准确率为{0}%", (double)count / historyData.Count * 100), ConsoleColor.DarkGreen);
        }

        public static void VerifyExcludeByPower()
        {
            History.Instance.GetData(2001, out var historyData);
            int count = 0;
            for (int j = 0; j < historyData.Count - 1; j++)
            {
                var lastPeriodLotto = historyData.ElementAt(j).Value;
                var nextPeriodLotto = historyData.ElementAt(j + 1).Value;
                var nextPeriod = historyData.ElementAt(j + 1).Key;

                Data.SetLotto(lastPeriodLotto);
                var res = SelectHelper.CalculateIncludeNumbers(lastPeriodLotto.ToArray());
                var isContain = res.Any(g => g[0] == nextPeriodLotto[0] && g[1] == nextPeriodLotto[1] && g[2] == nextPeriodLotto[2]);
                if (isContain) count++;
            }

            PrintHelper.Print(string.Format("上期开奖号平方杀号法准确率为{0}%", (double)count / (historyData.Count - 1) * 100), ConsoleColor.DarkGreen);
        }

        public static void VerifyExcludeBySum()
        {
            History.Instance.GetData(2001, out var historyData);
            int count = 0;
            for (int j = 0; j < historyData.Count - 1; j++)
            {
                var lastPeriodLotto = historyData.ElementAt(j).Value;
                var nextPeriodLotto = historyData.ElementAt(j + 1).Value;

                Data.SetLotto(lastPeriodLotto);
                var notEqual = lastPeriodLotto.Sum() != nextPeriodLotto.Sum();
                if (notEqual) count++;
            }

            PrintHelper.Print(string.Format("上期和值杀号法准确率为{0}%", (double)count / (historyData.Count - 1) * 100), ConsoleColor.DarkGreen);
        }

        public static void VerifyExcludeByPeriodMantissa()
        {
            History.Instance.GetData(2000, out var historyData);
            int count = 0;
            for (int j = 0; j < historyData.Count; j++)
            {
                var lastPeriodLotto = historyData.ElementAt(j).Value;
                var lastPeriod = historyData.ElementAt(j).Key;

                var isContain = lastPeriodLotto.Any(n => int.Parse(lastPeriod) % 10 == n);
                if (!isContain) count++;
            }

            PrintHelper.Print(string.Format("期数尾号杀号法准确率为{0}%", (double)count / historyData.Count * 100), ConsoleColor.DarkGreen);
        }

        public static void VerifyExcludeByRelation()
        {
            History.Instance.GetData(2001, out var historyData);
            int count = 0;
            for (int j = 0; j < historyData.Count - 1; j++)
            {
                var lastPeriodLotto = historyData.ElementAt(j).Value;
                var nextPeriodLotto = historyData.ElementAt(j + 1).Value;

                Data.SetLotto(lastPeriodLotto);
                var relation_number = SelectHelper.CalculateRelationNumbers(lastPeriodLotto.ToArray());
                var relation_count = relation_number.Count(n => nextPeriodLotto.Contains(n));
                var isContain = relation_count < 2;
                if (isContain) count++;
            }

            PrintHelper.Print(string.Format("对应号杀号法准确率为{0}%", (double)count / (historyData.Count - 1) * 100), ConsoleColor.DarkGreen);
        }

        public static void VerifyExcludeBySpanSumMantissa()
        {
            History.Instance.GetData(2001, out var historyData);
            int count = 0;
            for (int j = 0; j < historyData.Count - 1; j++)
            {
                var lastPeriodLotto = historyData.ElementAt(j).Value;
                var nextPeriodLotto = historyData.ElementAt(j + 1).Value;

                Data.SetLotto(lastPeriodLotto);
                var relation_number = SelectHelper.CalculateSpanSumMantissaNumbers(lastPeriodLotto.ToArray());
                var relation_count = relation_number.Count(n => nextPeriodLotto.Contains(n));
                var isContain = relation_count != 2;
                if (isContain) count++;
            }

            PrintHelper.Print(string.Format("跨度和值尾组合杀号法准确率为{0}%", (double)count / (historyData.Count - 1) * 100), ConsoleColor.DarkGreen);
        }

        public static void VerifyExcludeByPeriodTwoMantissa()
        {
            History.Instance.GetData(2001, out var historyData);
            int count = 0;
            for (int j = 0; j < historyData.Count - 1; j++)
            {
                var lastPeriodLotto = historyData.ElementAt(j).Value;
                var lastPeriod = historyData.ElementAt(j).Key;

                var twoMantissa = int.Parse(lastPeriod) % 100;
                var numbers = new int[2] { twoMantissa / 10, twoMantissa % 10 };

                var isContain = lastPeriodLotto.Count(n => numbers.Contains(n)) < 2;
                if (isContain) count++;
            }

            PrintHelper.Print(string.Format("期数后2位组合杀号法准确率为{0}%", (double)count / (historyData.Count - 1) * 100), ConsoleColor.DarkGreen);
        }

        public static void VerifyExcludeByPeriodAndSumMantissa()
        {
            History.Instance.GetData(2001, out var historyData);
            int count = 0;
            for (int j = 0; j < historyData.Count - 1; j++)
            {
                var lastPeriodLotto = historyData.ElementAt(j).Value;
                var nextPeriodLotto = historyData.ElementAt(j + 1).Value;
                var nextPeriod = historyData.ElementAt(j + 1).Key;

                var numbers = new int[2] { int.Parse(nextPeriod) % 10, lastPeriodLotto.Sum() % 10 };
                var isContain = nextPeriodLotto.Count(n => numbers.Contains(n)) < 2;
                if (isContain) count++;
            }

            PrintHelper.Print(string.Format("期数尾与上期和值尾组合杀号法准确率为{0}%", (double)count / (historyData.Count - 1) * 100), ConsoleColor.DarkGreen);
        }

        public static void VerifyExcludeByLastPeriodCompostion()
        {
            History.Instance.GetData(2001, out var historyData);
            int count = 0;
            for (int j = 0; j < historyData.Count - 1; j++)
            {
                var lastPeriodLotto = historyData.ElementAt(j).Value;
                var nextPeriodLotto = historyData.ElementAt(j + 1).Value;
                var nextPeriod = historyData.ElementAt(j + 1).Key;

                var lastPeriodSpan = lastPeriodLotto.Max() - lastPeriodLotto.Min();
                var equal1 = lastPeriodSpan != nextPeriodLotto.Sum() && lastPeriodSpan != nextPeriodLotto.Sum() % 10; //89%
                var equal2 = lastPeriodLotto.Sum() % 10 + 4 != nextPeriodLotto[2]; //百位、十位、个位 皆为94%
                var equal3 = lastPeriodSpan != nextPeriodLotto[2]; //88%
                var equal4 = lastPeriodLotto[1] != nextPeriodLotto[0]; //90%
                var equal5 = (lastPeriodLotto.Sum() % 10 + lastPeriodSpan) % 10 != nextPeriodLotto[1]; //90%
                var equal6 = int.Parse(nextPeriod) % 10 + 4 != nextPeriodLotto[0]; //百位、十位、个位 皆为94%
                var equal7 = lastPeriodLotto[0] != nextPeriodLotto[2]; //88%
                var equal8 = lastPeriodLotto.Sum() % 10 - 3 != nextPeriodLotto[1]; //百位、十位、个位 皆为93%
                var equal9 = lastPeriodLotto[2] != nextPeriodLotto[0]; //百位、十位92%
                var equal10 = int.Parse(nextPeriod) % 10 != nextPeriodLotto[0]; //90%
                var equal11 = (int.Parse(nextPeriod) % 10 * 3 + 3) % 10 != nextPeriodLotto[0]; //90%

                var isContain = equal11;
                if (isContain) count++;
            }

            PrintHelper.Print(string.Format("上期杀下期各种组合杀法准确率为{0}%", (double)count / (historyData.Count - 1) * 100), ConsoleColor.DarkGreen);
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
