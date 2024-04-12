/*
 *Description: SuperLottoHistory
 *Author: Chance.zheng
 *Creat Time: 2023/9/11 9:48:33
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SupperLotto
{
    public class SuperLottoHistory
    {
        private static readonly SuperLottoHistory _instance = new SuperLottoHistory();
        public static SuperLottoHistory Instance => _instance;
        static SuperLottoHistory() { }
        private SuperLottoHistory() { }


        private const string HistoryUrl = "https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=85&provinceId=0&pageSize=104&isVerify=1&pageNo=1";

        public Dictionary<int, IList<int>> Datas = new Dictionary<int, IList<int>>();

        public string GetUrl(int pageSize)
            => "https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=85&provinceId=0&pageSize=" + pageSize + "&isVerify=1&pageNo=1";

        public async void HttpGet()
        {
            HttpClient httpClient = new HttpClient();
            var httpResponseMessage = await httpClient.GetAsync(GetUrl(2));
            var resultJson = await httpResponseMessage.Content.ReadAsStringAsync();

            var dataList = JsonNode.Parse(resultJson)!["value"]!["list"];
            for (int i = 0; i < dataList!.AsArray().Count; i++)
            {
                var key = int.Parse(dataList[i]["lotteryDrawNum"].ToString());
                var value = dataList[i]["lotteryDrawResult"].ToString().Split(' ').Select(s => int.Parse(s)).ToList();
                Datas.Add(key, value);
            }
            Datas = new Dictionary<int, IList<int>>(Datas.Reverse());

            Program.SemaphoreSlim.Release();
        }
    }
}
