import requests
import json
import os
from collections import OrderedDict


def update_three_d():
    all_data = list()
    url = "https://www.cwl.gov.cn/cwl_admin/front/cwlkj/search/kjxx/findDrawNotice?name=3d"
    response = requests.get(url)
    content = response.text
    all_data = json.loads(content)["result"]

    current_dir = os.path.dirname(os.path.abspath(__file__))
    filename = os.path.join(current_dir, "history_threed.json")
    with open(filename, "w", encoding="utf-8") as file:
        json.dump(all_data, file, indent=4, ensure_ascii=False)


def update_three_union():
    all_data = list()
    url = "https://www.cwl.gov.cn/cwl_admin/front/cwlkj/search/kjxx/findDrawNotice?name=ssq"
    response = requests.get(url)
    content = response.text
    all_data = json.loads(content)["result"]

    current_dir = os.path.dirname(os.path.abspath(__file__))
    filename = os.path.join(current_dir, "history_union.json")
    with open(filename, "w", encoding="utf-8") as file:
        json.dump(all_data, file, indent=4, ensure_ascii=False)


def update_three_permutation():
    all_data = list()
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36 Edg/130.0.0.0",
    }
    for page_num in range(1, 51):
        url = f"https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=35&provinceId=0&pageSize=100&isVerify=1&pageNo={page_num}"
        response = requests.get(url, headers=headers)
        content = response.text
        data = json.loads(content)["value"]["list"]
        all_data += data

    current_dir = os.path.dirname(os.path.abspath(__file__))
    filename = os.path.join(current_dir, "history_threepermutation.json")
    with open(filename, "w", encoding="utf-8") as file:
        json.dump(all_data, file, indent=4, ensure_ascii=False)


def update_seven_permutation():
    all_data = list()
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36 Edg/130.0.0.0",
    }
    for page_num in range(1, 51):
        url = f"https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=04&provinceId=0&pageSize=100&isVerify=1&pageNo={page_num}"
        response = requests.get(url, headers=headers)
        content = response.text
        data = json.loads(content)["value"]["list"]
        all_data += data

    current_dir = os.path.dirname(os.path.abspath(__file__))
    filename = os.path.join(current_dir, "history_sevenpermutation.json")
    with open(filename, "w", encoding="utf-8") as file:
        json.dump(all_data, file, indent=4, ensure_ascii=False)


def update_supper():
    all_data = list()
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36 Edg/130.0.0.0",
    }
    for page_num in range(1, 31):
        url = f"https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=85&provinceId=0&pageSize=100&isVerify=1&pageNo={page_num}"
        response = requests.get(url, headers=headers)
        content = response.text
        data = json.loads(content)["value"]["list"]
        all_data += data

    current_dir = os.path.dirname(os.path.abspath(__file__))
    filename = os.path.join(current_dir, "history_supper.json")
    with open(filename, "w", encoding="utf-8") as file:
        json.dump(all_data, file, indent=4, ensure_ascii=False)


if __name__ == "__main__":
    update_three_d()
    update_three_union()
    update_three_permutation()
    update_seven_permutation()
    update_supper()
