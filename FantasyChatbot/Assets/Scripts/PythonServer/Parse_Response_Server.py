from flask import Flask, request, jsonify
import spacy

app = Flask(__name__)

# 영어 모델 로드
nlp = spacy.load("en_core_web_sm")

@app.route('/parse_ai_response', methods=['POST'])
def parse_ai_response():
    data = request.get_json()
    response_text = data.get('response', '')
    
    # 응답을 spaCy로 파싱
    doc = nlp(response_text)

    # 변화량 추출을 위한 초기화
    hp_change = 0
    mp_change = 0
    gold_change = 0

    # 토큰들을 순회하면서 변화량 파싱
    for token in doc:
        if token.text.lower() in ["hp", "체력"]:
            if token.nbor(1).like_num:
                hp_change = int(token.nbor(1).text)
        elif token.text.lower() in ["mp", "마나"]:
            if token.nbor(1).like_num:
                mp_change = int(token.nbor(1).text)
        elif token.text.lower() in ["gold", "골드"]:
            if token.nbor(1).like_num:
                gold_change = int(token.nbor(1).text)

    # 결과 반환
    return jsonify({
        'hp_change': hp_change,
        'mp_change': mp_change,
        'gold_change': gold_change
    })

if __name__ == '__main__':
    app.run(port=5000)
