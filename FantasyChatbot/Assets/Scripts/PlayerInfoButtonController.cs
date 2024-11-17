using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoButtonController : MonoBehaviour
{
    public GameObject PlayerInfo; // 플레이어 정보입력 오브젝트
    public GameObject SenarioSelect; // 시나리오 선택 오브젝트
    public TMP_InputField playerNameInput;
    public TMP_InputField playerDetailsInput;

    public Button confirmButton; // 확인 버튼

    void Start()
    {
        // 초기 상태에서 확인 버튼을 비활성화합니다.
        confirmButton.interactable = false;

        // 입력 필드가 변경될 때 확인 버튼을 활성화하는 이벤트 연결
        playerNameInput.onValueChanged.AddListener(delegate { UpdateConfirmButton(); });
        playerDetailsInput.onValueChanged.AddListener(delegate { UpdateConfirmButton(); });
    }

    // 되돌리기 버튼을 누를 시 호출될 메서드
    public void OnBackButtonPressed()
    {
        // 패널 비활성화
        PlayerInfo.SetActive(false);

        // 현재 생성되있는 프로필 프리팹 삭제
        if (PlayerDataManager.Instance.currentProfilePrefab != null)
        {
            Destroy(PlayerDataManager.Instance.currentProfilePrefab);
            PlayerDataManager.Instance.currentProfilePrefab = null; // 참조 초기화
        }
    }

    // 확인 버튼을 누를 시 호출될 메서드
    public void OnConfirmButtonPressed()
    {
        // 입력 필드의 값을 PlayerDataManager에 전달하여 데이터 갱신
        PlayerDataManager.Instance.SetPlayerName(playerNameInput.text);
        PlayerDataManager.Instance.SetPlayerDetails(playerDetailsInput.text);
        
        // 다음 단계인 시나리오 선택 패널을 활성화합니다.
        SenarioSelect.SetActive(true);
    }

    // 입력 필드에 값이 있을 경우 확인 버튼을 활성화하는 메서드
    private void UpdateConfirmButton()
    {
        confirmButton.interactable = !string.IsNullOrEmpty(playerNameInput.text) && !string.IsNullOrEmpty(playerDetailsInput.text);
    }
}