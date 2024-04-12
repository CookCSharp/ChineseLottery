using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;

namespace SupperLotto
{
    internal class Program
    {
        static Dictionary<int, IList<int>> Datas = default!;
        static Dictionary<int, IList<int>> ForcastDatas = default!;
        public static SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(0, 1);

        static void Main(string[] args)
        {
            SuperLottoHistory.Instance.HttpGet();
            SemaphoreSlim.Wait();

            Datas = new Dictionary<int, IList<int>>(SuperLottoHistory.Instance.Datas);

            var forcastDatas = SuperLottoHistory.Instance.Datas.ToList();
            forcastDatas.RemoveRange(0, 2);
            ForcastDatas = new Dictionary<int, IList<int>>(forcastDatas);

            Calculate();
        }

        static int GetIssue(int i)
        {
            return Datas.First().Key + i;
        }

        static IEnumerable<string> GetSingles(List<string> source)
        {
            return source.GroupBy(x => x).Where(g => g.Count() == 1).Select(y => y.Key);
        }

        static IEnumerable<string> GetDuplicates(List<string> source)
        {
            return source.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key);
        }

        static void Calculate()
        {
            var calStrings = new List<string>();
            var numbers = new List<string>();

            var resultBooleans = new Dictionary<int, string>();
            var resultsDictionary = new Dictionary<int, string>();
            var resultsSinglesDictionary = new Dictionary<int, string>();
            var resultsDuplicatesDictionary = new Dictionary<int, string>();
            var resultsDistinctDictionary = new Dictionary<int, string>();

            for (int i = 0; i < Datas.Count - 1; i++)
            {
                var first = Datas[GetIssue(i)];
                var second = Datas[GetIssue(i + 1)];

                ForEachCal(first, second, ref numbers);
                //ForEachCal(first, first, ref numbers);
                //ForEachCal(second, second, ref numbers);

                var pre = GetIssue(i + 2);

                if (i < Datas.Count - 2 && i + 3 < Datas.Count)
                {
                    var contains = new List<bool>();
                    for (int h = 0; h < ForcastDatas[pre].Count; h++)
                    {
                        contains.Add(numbers.Any(n => n == ForcastDatas[pre].ElementAt(h).ToString("00")));
                    }
                    resultBooleans.Add(pre, string.Join(',', contains));
                }

                numbers.Sort();
                var numberString = string.Join(",", numbers);
                resultsDictionary.Add(pre, numberString);

                var numberSingleString = string.Join(",", GetSingles(numbers));
                resultsSinglesDictionary.Add(pre, numberSingleString);

                var numberDuplicateString = string.Join(",", GetDuplicates(numbers));
                resultsDuplicatesDictionary.Add(pre, numberDuplicateString);

                var numberDistinctString = string.Join(",", numbers.Distinct());
                resultsDistinctDictionary.Add(pre, numberDistinctString);

                numbers.Clear();
            }
        }

        static void ForEachCal(IList<int> first, IList<int> second, ref List<string> numbers, List<string> calStrings = null)
        {
            for (int j = 0; j < first.Count; j++)
            {
                for (int k = 0; k < second.Count; k++)
                {
                    var v1 = first[j] + second[k];
                    var v2 = first[j] - second[k];
                    var v3 = second[k] - first[j];

                    //if (v1 <= 35) calStrings.Add($"{v1},{first[j]}+{second[k]}");
                    //if (v2 > 0) calStrings.Add($"{v2},{first[j]}-{second[k]}");
                    //if (v3 > 0) calStrings.Add($"{v3},{second[k]}-{first[j]}");

                    if (v1 <= 35) numbers.Add(v1.ToString("00"));
                    if (v2 > 0) numbers.Add(v2.ToString("00"));
                    if (v3 > 0) numbers.Add(v3.ToString("00"));
                }
            }
        }
    }
}