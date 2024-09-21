/*
 *Description: History
 *Author: Chance.zheng
 *Creat Time: 2024/5/2 10:02:13
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml;
using static System.Net.WebRequestMethods;

namespace ThreeDLotto
{
    /// <summary>
    /// 除去最前面3个数据
    /// </summary>
    public class History
    {
        private static readonly History _instance = new History();
        public static History Instance => _instance;
        static History() { }
        private History() { }

        //所有开奖号码
        private const string AllURL = "https://www.cwl.gov.cn/cwl_admin/front/cwlkj/search/kjxx/findDrawNotice?name=3d";
        //最近4000期开奖号码
        private const string URL = "https://www.cwl.gov.cn/cwl_admin/front/cwlkj/search/kjxx/findDrawNotice?name=3d&issueCount=&issueStart=&issueEnd=&dayStart=&dayEnd=&pageNo=1&pageSize=4000&week=&systemType=PC";

        private Dictionary<string, IList<int>> HistoryData = new Dictionary<string, IList<int>>();
        private SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(0, 1);
        private bool _isStartGuess = false; //是否开始猜测

        private string GetUrl(int pageSize)
            => "https://www.cwl.gov.cn/cwl_admin/front/cwlkj/search/kjxx/findDrawNotice?name=ssq&pageNo=1&pageSize=" + pageSize + "&systemType=PC";
        //=> "https://www.cwl.gov.cn/cwl_admin/front/cwlkj/search/kjxx/findDrawNotice?name=ssq&issueCount=&issueStart=&issueEnd=&dayStart=&dayEnd=&pageNo=1&pageSize=" + pageSize + "&week=&systemType=PC";


        public async void HttpGet(int pageSize = 50)
        {
            HistoryData.Clear();

            using var httpClient = new HttpClient();
            var agent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36 Edg/123.0.0.0";
            //httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(agent);
            httpClient.DefaultRequestHeaders.Add("User-Agent", agent);
            var httpResponseMessage = await httpClient.GetAsync(GetUrl(pageSize));
            //if (!httpResponseMessage.IsSuccessStatusCode)
            //{
            //    await Console.Out.WriteLineAsync("读取历史数据不成功");
            //    return;
            //}
            var resultJson = await httpResponseMessage.Content.ReadAsStringAsync();

            var dataList = JsonNode.Parse(resultJson)!["result"];
            for (int i = 0; i < dataList!.AsArray().Count; i++)
            {
                var key = dataList[i]!["code"]!.ToString();
                var value = dataList[i]!["red"]!.ToString().Split(',').Select(s => int.Parse(s)).ToList();
                HistoryData.Add(key, value);
            }
            HistoryData = new Dictionary<string, IList<int>>(HistoryData.Reverse());

            SemaphoreSlim.Release();
        }

        /// <summary>
        /// History.json文件需要打开链接手动赋值结果到文件中，以此更新文件数据
        /// </summary>
        public async void ResolveJsonFile()
        {
            using var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Assets\\History.json", FileMode.Open);
            using var sw = new StreamReader(fs);
            var resultJson = await sw.ReadToEndAsync();
            var dataList = JsonNode.Parse(resultJson)!["result"];
            for (int i = 0; i < dataList!.AsArray().Count; i++)
            {
                var key = dataList[i]!["code"]!.ToString();
                var value = dataList[i]!["red"]!.ToString().Split(',').Select(s => int.Parse(s)).ToList();
                HistoryData.Add(key, value);
            }

            SemaphoreSlim.Release();
        }

        public void Wait() => SemaphoreSlim.Wait();

        public void StartGuess() => _isStartGuess = true;

        public void EndGuess() => _isStartGuess = false;

        public void GetData(int pageSize, out Dictionary<string, IList<int>> historyData)
        {
            historyData = !_isStartGuess ? new Dictionary<string, IList<int>>(HistoryData.Take(pageSize).Reverse()) 
                : new Dictionary<string, IList<int>>(HistoryData.Skip(1).Take(pageSize).Reverse());
        }
    }
}
