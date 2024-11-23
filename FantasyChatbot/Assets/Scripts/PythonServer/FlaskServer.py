from flask import Flask, request, jsonify
import re

app = Flask(__name__)

# 체력, 마나, 골드 변화를 추출하는 함수
def extract_changes(response):
    changes = {
        "hp_change": 0,
        "mp_change": 0,
        "gold_change": 0
    }

    # 체력, 마나, 골드의 변화를 파싱하기 위한 정규식
    hp_match = re.search(r"(?:HP|체력)\s*([-+]?\d+)", response)
    if hp_match:
        changes["hp_change"] = int(hp_match.group(1))

    hp_match = re.search(r"(?:HP|체력)\s*(\d+)\s*→\s*(\d+)", response)
    if hp_match:
        changes["hp_change"] = int(hp_match.group(1))

    mp_match = re.search(r"(?:MP|마나)\s*([-+]?\d+)", response)
    if mp_match:
        changes["mp_change"] = int(mp_match.group(1))

    mp_match = re.search(r"(?:MP|마나)\s*(\d+)\s*→\s*(\d+)", response)
    if mp_match:
        changes["mp_change"] = int(mp_match.group(1))

    gold_match = re.search(r"(?:Gold|골드)\s*([-+]?\d+)", response)
    if gold_match:
        changes["gold_change"] = int(gold_match.group(1))

    gold_match = re.search(r"(?:Gold|골드)\s*(\d+)\s*→\s*(\d+)", response)
    if gold_match:
        changes["gold_change"] = int(gold_match.group(1))

    return changes

@app.route('/process_response', methods=['POST'])
def process_response():
    # 요청 데이터 확인
    data = request.json
    print("Received from Unity: ", data)  # 요청 받은 데이터 확인

    # 응답으로 보낼 JSON 생성
    response = {
        "hp_change": 10,
        "mp_change": -5,
        "gold_change": 0
    }
    return jsonify(response)
