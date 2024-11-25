## 동양미래대학교 컴퓨터컴퓨터소프트웨어공학과
### 생성형AI활용개발실무 경진대회용 프로그램
#### 작품명 : FantasyChatbot-TRPGGame

#### 개발자 : 한승민

#### 작품 소개
- __장르__ : TRPG
- __개발 도구__ : Unity, ChatGPT-4o, Stable Diffusion, python Flask & spaCy
- __개요__ : 게임 마스터의 역할을 맡는 AI와 함께 채팅을 바탕으로 진행하는 TRPG 게임
- __특징__
  - 원하는 게임 시나리오를 선택 (현재는 1개만 구현)
  - 원하는 캐릭터 특징 이미지(CG)를 선택해 캐릭터 데이터 생성
    - 성별 (남, 여)
    - 직업 (각 성별 당 4개 직업, 총 8개)
    - 직업 별로 HP, MP, 보유 Gold의 개별적인 데이터를 추가 보유
  - 캐릭터의 이름과 상세설명을 기입
  - GPT에게 시나리오의 줄거리와 플레이어 캐릭터의 정보를 전달, 게임 마스터로서 정보를 기억하고 이야기를 전개
  - 이야기 전개에 따른 GPT의 응답에 따라 HP, MP, Gold 등의 가시적인 UI요소가 실시간 변화함 (python SpaCy를 통한 파싱 응용)
  - Stable Diffusion을 이용해 몰입감을 위한 CG 생성

#### 개선 방안
