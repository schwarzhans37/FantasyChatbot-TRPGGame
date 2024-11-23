using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoUpdater : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_InputField playerSexInput;
    public TMP_InputField playerJobInput;
    public TMP_InputField playerHPInput;
    public TMP_InputField playerMPInput;
    public TMP_InputField playerGoldInput;
    public TMP_InputField playerDetailsInput;

    private void OnEnable()
    {
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.OnPlayerInfoUpdated += UpdatePlayerInfo;
        }
        
        // 초기 UI 값을 반영합니다.
        UpdatePlayerInfo();
    }

    private void OnDisable()
    {
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.OnPlayerInfoUpdated -= UpdatePlayerInfo;
        }
    }

    // 플레이어 정보 설정 부분의 성별과 직업을 미리 갱신해주는 코드
    public void UpdatePlayerInfo()
    {
        if (PlayerDataManager.Instance != null)
        {
            playerSexInput.text = PlayerDataManager.Instance.playerSex;
            playerJobInput.text = PlayerDataManager.Instance.playerJob;
            playerHPInput.text = PlayerDataManager.Instance.playerHP.ToString();
            playerMPInput.text = PlayerDataManager.Instance.playerMP.ToString();
            playerGoldInput.text = PlayerDataManager.Instance.playerGold.ToString();
        }
    }
}
