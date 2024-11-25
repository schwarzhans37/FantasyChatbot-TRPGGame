using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance; // 싱글톤 패턴

    // 플레이어의 정보 저장
    public string selectedSenario; // 선택한 시나리오
    public string senarioPrompt;  // 생성형 AI에게 보낼 시나리오 정보 및 요구사항
    public string playerName; // 플레이어 캐릭터 이름
    public string playerSex; // 플레이어 캐릭터 성별
    public string playerJob; // 플레이어 캐릭터 직업
    public int playerHP; // 플레이어 캐릭터 최대체력
    public int currentHP; // 현재 플레이어 체력
    public int playerMP; // 플레이어 캐릭터 최대마나
    public int currentMP; // 현재 플레이어 마나
    public int playerGold; // 플레이어 소유 골드
    public string playerDetails; // 플레이어 캐릭터 상세정보

    public GameObject MaleKnightPrefab;
    public GameObject MaleMagicianPrefab;
    public GameObject MaleAssassinPrefab;
    public GameObject MalePriestPrefab;

    public GameObject FemaleKnightPrefab;
    public GameObject FemaleMagicianPrefab;
    public GameObject FemaleAssassinPrefab;
    public GameObject FemalePriestessPrefab;

    [HideInInspector] public GameObject currentProfilePrefab; //현재 생성되있는 프로필 프리팹 참조

    public event Action OnPlayerInfoUpdated; // 플레이어 정보가 업데이트될 때 발생하는 이벤트

    private void Awake()
    {
        // Singleton 인스턴스 설정해 전역 접근 허가
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 플레이어 정보 설정 메서드들
    public void SetPlayerSelectedSenario(string senario)
    {
        selectedSenario = senario;
    }
    public void SetSenarioPrompt(string prompt)
    {
        senarioPrompt = prompt;
    }
    public void SetPlayerName(string name)
    {
        playerName = name;
        OnPlayerInfoUpdated?.Invoke(); // 이름이 변경되면 이벤트 발생
    }

    public void SetPlayerSex(string sex)
    {
        playerSex = sex;
        OnPlayerInfoUpdated?.Invoke(); // 성별이 변경되면 이벤트 발생
    }

    public void SetPlayerJob(string job)
    {
        playerJob = job;
        OnPlayerInfoUpdated?.Invoke(); // 직업이 변경되면 이벤트 발생
    }

    public void SetPlayerHP(int hp)
    {
        playerHP = hp;
        OnPlayerInfoUpdated?.Invoke(); // 최대체력이 변경되면 이벤트 발생
    }

    public void SetCurrentHP(int chp)
    {
        currentHP = chp;
        OnPlayerInfoUpdated?.Invoke(); // 현재체력이 변경되면 이벤트 발생
    }

    public void SetPlayerMP(int mp)
    {
        playerMP = mp;
        OnPlayerInfoUpdated?.Invoke(); // 최대마나가 변경되면 이벤트 발생
    }

    public void SetCurrentMP(int cmp)
    {
        currentMP = cmp;
        OnPlayerInfoUpdated?.Invoke(); // 현재마나가 변경되면 이벤트 발생
    }

    public void SetPlayerGold(int gold)
    {
        playerGold = gold;
        OnPlayerInfoUpdated?.Invoke(); // 보유골드가 변경되면 이벤트 발생
    }

    public void SetPlayerDetails(string details)
    {
        playerDetails = details;
        OnPlayerInfoUpdated?.Invoke(); // 세부 정보가 변경되면 이벤트 발생
    }

    public void ResetPlayerData()
    {
        selectedSenario = null;
        playerName = null;
        playerSex = null;
        playerJob = null;
        playerDetails = null;

        // 현재 생성되 있는 프로필 이미지 프리팹이 있는지 확인 후 삭제
        if (currentProfilePrefab != null)
        {
            Destroy(currentProfilePrefab);
            currentProfilePrefab = null;
        }

        // 플레이어 정보 업데이트 이벤트 호출
        OnPlayerInfoUpdated?.Invoke();
    }
}
