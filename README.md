## 동양미래대학교 컴퓨터컴퓨터소프트웨어공학과
### 생성형AI활용개발실무 경진대회용 프로그램
#### 작품명 : FantasyChatbot-TRPGGame

#### 개발자 : 한승민

#### 개발기간 : 2024.11.13(수) - 2024.11.24(월)

#### 작품 소개
- __장르__ : TRPG

- __개발 도구__ : Unity, ChatGPT-4o, Stable Diffusion, python Flask & spaCy

- __개요__ : 게임 마스터의 역할을 맡는 AI와 함께 채팅을 바탕으로 진행하는 TRPG 게임

- __특징__
  - 원하는 게임 시나리오를 선택 (현재는 1개만 구현)
    ![image](https://github.com/user-attachments/assets/e12b1841-87e5-4424-aee7-d5b194a281b8)

  - 원하는 캐릭터 특징 이미지(CG)를 선택해 캐릭터 데이터 생성
    - 성별 (남, 여)
    ![image](https://github.com/user-attachments/assets/c2b1dba3-85af-41f7-b9e9-95aea0f4823b)

    - 직업 (각 성별 당 4개 직업, 총 8개)
    ![image](https://github.com/user-attachments/assets/39c9c16b-d3bb-41a2-93fc-e62e86fd24a0)
    ![image](https://github.com/user-attachments/assets/b0b78493-1ca0-4a18-90dd-89f747cdc4b5)

    - 직업 별로 HP, MP, 보유 Gold의 개별적인 데이터를 추가 보유
  
  - 캐릭터의 이름과 상세설명을 기입
    ![image](https://github.com/user-attachments/assets/1a14ecd0-0789-4309-af94-84c97b7780e6)

  - GPT에게 시나리오의 줄거리와 플레이어 캐릭터의 정보를 전달, 게임 마스터로서 정보를 기억하고 이야기를 전개
  - 이야기 전개에 따른 GPT의 응답에 따라 HP, MP, Gold 등의 가시적인 UI요소가 실시간 변화함 (python SpaCy를 통한 파싱 응용)
  - Stable Diffusion을 이용해 몰입감을 위한 CG 생성

#### 개선 예정 요소
  - 캐릭터 생성에 들어가는 특징(종족, 능력치, 스킬, 아이템 등) 세분화 요소 추가
  - 데미지 공식 추가 (현재는 AI의 판단에 맏기고 있음)
  - 아이템 인벤토리 리스트
  - 프롬프트에 입력한 AI 응답 강제 완화 및 파싱 알고리즘의 개선을 통한 다채로운 묘사 유도
  - 대화 흐름에 따른 배경 이미지 교체
  - CG(캐릭터 이미지, 배경이미지 등) 자동생성 기능
