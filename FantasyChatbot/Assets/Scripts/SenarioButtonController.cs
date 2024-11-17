using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SenarioButtonController : MonoBehaviour
{
    public GameObject SenarioSelect; // 시나리오 선택
    public GameObject ChoiceSex; // 성별 선택

    public Button confirmButton; // 확인 버튼

    private enum Senario { None, Arisu }
    private Senario selectedSenario = Senario.None;

    void Start()
    {
        // 초기 상태에서 확인 버튼을 비활성화합니다.
        confirmButton.interactable = false;
    }

    public void OnSenarioSelected()
    {
        selectedSenario = Senario.Arisu;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerSelectedSenario("Arisu RPG");
    }

    // 홈 버튼을 누를 시 호출될 메서드
    public void OnHomeButtonPressed()
    {
        // PlayerDataManager 인스턴스 값 초기화 요청
        PlayerDataManager.Instance.ResetPlayerData();

        // MainMenuScene으로 씬 이동
        SceneManager.LoadScene("MainMenuScene");
    }

    // 확인 버튼을 누를 시 호출될 메서드
    public void OnConfirmButtonPressed()
    {
        ChoiceSex.SetActive(true);
    }

    // 선택에 따라 확인 버튼 활성화 여부를 업데이트하는 메서드
    private void UpdateConfirmButton()
    {
        confirmButton.interactable = (selectedSenario != Senario.None);
    }
}
