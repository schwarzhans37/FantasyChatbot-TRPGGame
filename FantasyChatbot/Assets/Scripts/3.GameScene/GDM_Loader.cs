using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GDM_Loader : MonoBehaviour
{
    public GameObject playerCharacterCGParent; // PlayerCharacterCG 오브젝트의 부모
    public TMP_Text playerNameText; // 플레이어 이름을 표시
    public TMP_Text playerSexText; // 플레이어 성별을 표시
    public TMP_Text playerHP; // 플레이어 체력을 표사
    public TMP_Text playerMP; // 플레이어 마나를 표시
    public TMP_Text playerGold; // 플레이어 소유 골드
    public TMP_Text SenarioText; // 시나리오 설정을 표시
    public TMP_Text playerDetails; // 플레이어 세부정보
    public GameObject playerDetailsBox; // PlayerDetails 박스 부모 오브젝트
    public GameObject helpButton; // Help 버튼 오브젝트

    private void Start()
    {
        // PlayerDataManager 인스턴스 확인
        if (PlayerDataManager.Instance != null)
        {
            DataManagerLoad();
            UpdatePlayerInfoUI();
        }
        else
        {
            Debug.LogError("PlayerDataManager instance is missing.");
        }

        // Help 버튼에 마우스가 호버될 때 이벤트 추가
        EventTrigger trigger = helpButton.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = helpButton.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnHelpButtonHoverEnter(); });
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { OnHelpButtonHoverExit(); });
        trigger.triggers.Add(entryExit);
    }

    private void DataManagerLoad()
    {
        // 플레이어의 성별과 직업 정보 가져오기
        string playerSex = PlayerDataManager.Instance.playerSex;
        string playerJob = PlayerDataManager.Instance.playerJob;
        int playerHP = PlayerDataManager.Instance.playerHP;
        int playerMP = PlayerDataManager.Instance.playerMP;
        int playerGold = PlayerDataManager.Instance.playerGold;
        string senarioText = PlayerDataManager.Instance.selectedSenario;
        string playerDetails = PlayerDataManager.Instance.playerDetails;

        GameObject prefabToSpawn = null;

        // 성별과 직업에 맞는 프리팹 설정
        if (playerSex == "남성")
        {
            if (playerJob == "기사")
            {
                prefabToSpawn = PlayerDataManager.Instance.MaleKnightPrefab;
            }
            else if (playerJob == "마법사")
            {
                prefabToSpawn = PlayerDataManager.Instance.MaleMagicianPrefab;
            }
            else if (playerJob == "궁수")
            {
                prefabToSpawn = PlayerDataManager.Instance.MaleAssassinPrefab;
            }
            else if (playerJob == "성직자")
            {
                prefabToSpawn = PlayerDataManager.Instance.MalePriestPrefab;
            }

        }
        else if (playerSex == "여성")
        {
            if (playerJob == "기사")
            {
                prefabToSpawn = PlayerDataManager.Instance.FemaleKnightPrefab;
            }
            else if (playerJob == "마법사")
            {
                prefabToSpawn = PlayerDataManager.Instance.FemaleMagicianPrefab;
            }
            else if (playerJob == "궁수")
            {
                prefabToSpawn = PlayerDataManager.Instance.FemaleAssassinPrefab;
            }
            else if (playerJob == "성직자")
            {
                prefabToSpawn = PlayerDataManager.Instance.FemalePriestessPrefab;
            }
        }
        else
        {
            Debug.LogWarning("Unknown player sex: " + playerSex);
        }

        // 프리팹을 PlayerCharacterCG의 자녀로 생성
        if (prefabToSpawn != null)
        {
            GameObject playerCharacterCG = Instantiate(prefabToSpawn, playerCharacterCGParent.transform);
            playerCharacterCG.transform.localPosition = Vector3.zero; // 부모의 위치에 맞게 배치
            PlayerDataManager.Instance.currentProfilePrefab = playerCharacterCG; // 생성된 프리팹 참조 저장
        }
        else
        {
            Debug.LogError("Failed to spawn player character CG. Prefab is null.");
        }
    }

    private void UpdatePlayerInfoUI()
    {
        // PlayerDataManager에서 정보 불러오기 및 UI 업데이트
        if (PlayerDataManager.Instance != null)
        {
            SenarioText.text = PlayerDataManager.Instance.selectedSenario;
            playerNameText.text = PlayerDataManager.Instance.playerName;
            playerSexText.text = PlayerDataManager.Instance.playerSex;
            playerHP.text = PlayerDataManager.Instance.currentHP.ToString() + " / " + PlayerDataManager.Instance.playerHP.ToString();
            playerMP.text = PlayerDataManager.Instance.currentMP.ToString() + " / " + PlayerDataManager.Instance.playerMP.ToString();
            playerGold.text = PlayerDataManager.Instance.playerGold.ToString() + "<color=orange> Gold</color>";
            playerDetails.text = PlayerDataManager.Instance.playerDetails;
        }
        else
        {
            Debug.LogError("PlayerDataManager instance is missing while updating UI.");
        }
    }

    private void OnHelpButtonHoverEnter()
    {
        // 마우스가 Help 버튼 위에 있을 때 PlayerDetails 박스 보이기
        playerDetailsBox.SetActive(true);
    }

    private void OnHelpButtonHoverExit()
    {
        // 마우스가 Help 버튼을 떠날 때 PlayerDetails 박스 숨기기
        playerDetailsBox.SetActive(false);
    }
}
