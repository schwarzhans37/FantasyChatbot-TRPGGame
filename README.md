# 동양미래대학교 컴퓨터컴퓨터소프트웨어공학과
### 생성형AI활용개발실무 경진대회용 프로그램

***

#### 작품명 : FantasyChatbot-TRPGGame

#### 개발자 : 한승민(schwarzhans37)

#### 아이디어
- 시공간 제약과 최소 인원 제한이 있는 TRPG를 내가 원하는 시간, 공간에서 혼자서라도 즐길 수는 없을까?
- 게임 마스터의 역할을 인공지능이 대신할 수 없을까?
- 게임 개발에 있어서 AI가 어디까지 역할을 해낼 수 있을까?

***

#### 작품 소개
- __개요__ : 게임 마스터의 역할을 맡는 AI와 함께 채팅을 바탕으로 진행하는 혼자서도 즐길 수 있는 TRPG 챗봇 게임

- __모티브__ : TRPG, CYOA
  - 정의
    - TPRG : 오프라인상에서 사람들이 테이블에 모여 앉아, 대화를 통해 진행하고, 각자가 분담된 역할을 연기하는(Role playing)게임
    - CYOA : 제작자가 특정한 형식의 룰북이나 템플릿 따위를 만들면, 플레이어들은 지정된 규칙에 따라 선택지를 골라 자신만의 캐릭터를 만드는 텍스트 어드벤처 게임
  - 필요 역할군 : 게임 마스터(사회자), 플레이어(참여자)
  - 필요 정보 : 시나리오, 아이템 테이블 등의 "룰북", 참여자들의 "캐릭터" 정보 등

- __개발 도구__
  -  Unity (게임 엔진)
  -  ChatGPT-4o (개발 테스트, 게임 채팅), Claude 3.5 sonnet (개발 테스트)
  -  Stable Diffusion (게임에 사용할 이미지 생성)
  -  python Flask & spaCy (자연어 NLP를 위한 라이브러리)

- __특징__
  - 원하는 게임 시나리오를 선택 (현재는 1개만 구현)
    ![image](https://github.com/user-attachments/assets/1412d6ad-d15d-4947-8bdb-cb3324d93151)

  - 원하는 캐릭터 특징 이미지(CG)를 선택해 캐릭터 데이터 생성
    - 성별 (남, 여)
    ![image](https://github.com/user-attachments/assets/47c940a8-fd8d-4705-889a-3e67d49235a5)

    - 직업 (각 성별 당 4개 직업, 총 8개)
      - 남성 ![image](https://github.com/user-attachments/assets/86ed14d8-051d-4aaa-b3ca-c21f61a882f2)

      - 여성 ![image](https://github.com/user-attachments/assets/99a51dd3-eec2-4c0f-a197-85dec5c4c99d)


    - 직업 별로 HP, MP, 보유 Gold의 개별적인 데이터를 추가 보유
  
  - 캐릭터의 이름과 상세설명을 기입
    ![image](https://github.com/user-attachments/assets/2326265f-37e8-4008-85ff-a61fc7b6caec)

  - GPT에게 시나리오의 줄거리와 플레이어 캐릭터의 정보를 전달, 게임 마스터로서 정보를 기억하고 이야기를 전개
    ![image](https://github.com/user-attachments/assets/987e1e31-ff78-45f3-a9a4-8de90b9d6364)

  - 이야기 전개에 따른 GPT의 응답에 따라 HP, MP, Gold 등의 가시적인 UI요소가 실시간 변화함 (python SpaCy를 통한 파싱 응용)
  - Stable Diffusion을 이용해 몰입감을 위한 CG 생성

- __결과__
  (사진 첨부 예정) 

***


#### 향후 발전 요소
  - 시나리오 추가
  - 캐릭터 생성에 들어가는 특징(종족, 능력치, 스킬, 아이템 등) 세분화 요소 추가
  - 데미지 공식 추가 (현재는 AI의 판단에 맏기고 있음)
  - 아이템 인벤토리 리스트
  - 프롬프트에 입력한 AI 응답 강제 완화 및 파싱 알고리즘의 개선을 통한 다채로운 묘사 유도
  - 대화 흐름에 따른 배경 이미지 교체
  - CG(캐릭터 이미지, 배경이미지 등) 자동생성 기능
