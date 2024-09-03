import requests
import json
import os
from collections import OrderedDict


def update_three_permutation():
    all_data = list()
    for page_num in range(1, 51):
        url = f"https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=35&provinceId=0&pageSize=100&isVerify=1&pageNo={page_num}"
        response = requests.get(url)
        content = response.text
        data = json.loads(content)["value"]["list"]
        all_data += data

    current_dir = os.path.dirname(os.path.abspath(__file__))
    filename = os.path.join(current_dir, "History-ThreePermutation.json")
    with open(filename, "w", encoding="utf-8") as file:
        json.dump(all_data, file, indent=4, ensure_ascii=False)


def update_supper():
    all_data = list()
    for page_num in range(1, 31):
        url = f"https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=85&provinceId=0&pageSize=100&isVerify=1&pageNo={page_num}"
        response = requests.get(url)
        content = response.text
        data = json.loads(content)["value"]["list"]
        all_data += data

    current_dir = os.path.dirname(os.path.abspath(__file__))
    filename = os.path.join(current_dir, "History-Supper.json")
    with open(filename, "w", encoding="utf-8") as file:
        json.dump(all_data, file, indent=4, ensure_ascii=False)


if __name__ == "__main__":
    update_three_permutation()
    update_supper()
