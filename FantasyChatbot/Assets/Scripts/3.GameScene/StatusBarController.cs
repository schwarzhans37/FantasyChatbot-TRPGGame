using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarController : MonoBehaviour
{
    public Image healthBarImage; // 체력바를 나타내는 Image 오브젝트
    public Image manaBarImage; // 마나바를 나타내는 Image 오브젝트
    
    private void Start()
    {
        // PlayerDataManager 인스턴스 확인 후, 이벤트 구독
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.OnPlayerInfoUpdated += UpdateBars;
            UpdateBars(); // 초기 체력바 및 마나바 설정
        }
        else
        {
            Debug.LogError("PlayerDataManager instance is missing.");
        }
    }

    private void UpdateBars()
    {
        if (PlayerDataManager.Instance != null)
        {
            // 체력바 업데이트
            if (healthBarImage != null)
            {
                float currentHP = PlayerDataManager.Instance.currentHP;
                float maxHP = PlayerDataManager.Instance.playerHP;
                float fillAmountHP = Mathf.Clamp01(currentHP / maxHP);
                healthBarImage.fillAmount = fillAmountHP;
            }

            // 마나바 업데이트
            if (manaBarImage != null)
            {
                float currentMP = PlayerDataManager.Instance.currentMP;
                float maxMP = PlayerDataManager.Instance.playerMP;
                float fillAmountMP = Mathf.Clamp01(currentMP / maxMP);
                manaBarImage.fillAmount = fillAmountMP;
            }
        }
    }

    private void OnDestroy()
    {
        // 스크립트가 파괴될 때 이벤트 구독 해제
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.OnPlayerInfoUpdated -= UpdateBars;
        }
    }
}

