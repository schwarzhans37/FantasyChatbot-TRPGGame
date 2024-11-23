using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class SenarioButtonController : MonoBehaviour
{
    public GameObject SenarioSelect; // 시나리오 선택
    public GameObject ChoiceSex; // 성별 선택

    public Button confirmButton; // 확인 버튼

    public List<GameObject> SenarioList = new List<GameObject>(); // 시나리오 리스트

    public GameObject SenarioLists; // 시나리오 리스트들을 포함하는 오브젝트
    public Button L_Button; // 왼쪽 이동 버튼
    public Button R_Button; // 오른쪽 이동 버튼

    public TextMeshProUGUI Pages; // 페이지 수를 표시할 TextMeshPro UI

    private enum Senario { None, Arisu }
    private Senario selectedSenario = Senario.None;

    private int currentIndex = 0;
    private bool isAnimating = false;

    void Start()
    {
        // 초기 상태에서 확인 버튼을 비활성화합니다.
        confirmButton.interactable = false;

        // 버튼 초기화
        L_Button.onClick.AddListener(() => MoveSenarioList(-1));
        R_Button.onClick.AddListener(() => MoveSenarioList(1));

        UpdateButtonInteractable();
        UpdatePageText();
    }

    public void OnSenarioSelected()
    {
        selectedSenario = Senario.Arisu;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerSelectedSenario(" 시나리오 : ArisuRPG\n" +
            "--------------------------------------------------------------------\n"
        );
        PlayerDataManager.Instance.SetSenarioPrompt(
            "[Scenario Name]: ArisuRPG Story\n" +
            "[Synopsis]: " + PlayerDataManager.Instance.playerName + "은 에리스 왕국의 평화를 위협하는 마왕 벨제부브와 그의 부하들을 쓰러뜨려 세계의 평화를 되찾기 위한 여정길에 오릅니다.\n" +
            "ArisuRPG는 캐릭터가 아닙니다. 이 스토리의 진행자로서 " + PlayerDataManager.Instance.playerName + "의 이야기를 진행시켜 주세요.\n" +
            "[Character Information]:\n" +
            "Name: " + PlayerDataManager.Instance.playerName + "\n" +
            "Sex: " + PlayerDataManager.Instance.playerSex + "\n" +
            "Job: " + PlayerDataManager.Instance.playerJob + "\n" +
            "Max HP: " + PlayerDataManager.Instance.playerHP + "\n" +
            "Current HP: " + PlayerDataManager.Instance.currentHP + "\n" +
            "Max MP: " + PlayerDataManager.Instance.playerMP + "\n" +
            "Current MP: " + PlayerDataManager.Instance.currentMP + "\n" +
            "Gold: " + PlayerDataManager.Instance.playerGold + "\n" +
            "Details: " + PlayerDataManager.Instance.playerDetails + "\n" +
            "이 정보를 기반으로, " + PlayerDataManager.Instance.playerName + "의 체력, 마나, 소지금을 적절히 사용하여 스토리를 진행시켜 주세요.\n" +
            "**플레이어의 상황에 따라 반드시 아래 명시된 형식을 사용해서만 응답해 주세요. 다른 형식은 사용하지 말아 주세요**:\n\n" +
            "**플레이어의 게임 내 상태를 정확하게 추적하고 데이터를 파싱하기 위해 반드시 필요한 양식입니다. 응답 형식을 반드시 지켜야 합니다.**\n" +
            "**체력 변화:** '체력 <변화량>', 예: '체력 -20'\n" +
            "**마나 변화:** '마나 <변화량>', 예: '마나 +15'\n" +
            "**골드 변화:** '골드 <변화량>', 예: '골드 -50'\n\n" +
            "**각 변화는 반드시 개별적으로 한 줄씩 표현해 주세요. 예시**:\n" +
            "- 체력이 10 감소하고 마나가 5 감소하는 경우:\n" +
            "  체력 -10\n" +
            "  마나 -5\n" +
            "- 상점에서 물건을 산 경우:\n" +
            "  골드 -50\n\n" +
            "**중요:**\n" +
            "- 플레이어의 체력(HP)이 0 이하가 된다면, 플레이어가 사망 혹은 행동 불능 상태로 판단하세요.\n" +
            "- 마나(MP)가 0 이하라면, 스킬을 사용할 수 없다고 판단하세요.\n" +
            "- 각 변화량은 최대 체력/마나를 초과하지 않도록 제한해 주세요.\n" +
            "- 체력, 마나, 골드의 변화는 명확하게 기술되어야 하며, 해당하는 수치 외에 다른 설명이나 장황한 묘사는 하지 말아 주세요.\n" +
            "- 응답 후, 자신이 제공한 정보가 요청된 형식을 따르는지 점검해 주세요. 형식이 맞지 않을 경우 다시 작성해 주세요.\n"
        );
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

    // 시나리오 리스트를 이동시키는 메서드
    private void MoveSenarioList(int direction)
    {
        if (isAnimating) return;

        int newIndex = currentIndex + direction;
        if (newIndex >= 0 && newIndex < SenarioList.Count)
        {
            currentIndex = newIndex;
            StartCoroutine(AnimateSenarioList(direction * -1600));
            UpdateButtonInteractable(); // 버튼 상태 업데이트를 애니메이션 시작 시 호출
            UpdatePageText();
        }
    }

    // 시나리오 리스트를 부드럽게 이동시키는 코루틴
    private IEnumerator AnimateSenarioList(float targetOffset)
    {
        isAnimating = true;

        Vector3 startPosition = SenarioLists.transform.localPosition;
        Vector3 targetPosition = startPosition + new Vector3(targetOffset, 0, 0);

        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            SenarioLists.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SenarioLists.transform.localPosition = targetPosition;
        isAnimating = false;
        UpdateButtonInteractable(); // 애니메이션이 끝난 후 버튼 상태 업데이트
    }

    // 이동 버튼 활성화 여부 업데이트
    private void UpdateButtonInteractable()
    {
        L_Button.interactable = (currentIndex > 0);
        R_Button.interactable = (currentIndex < SenarioList.Count - 1);
    }

    // 페이지 수 업데이트
    private void UpdatePageText()
    {
        if (SenarioList.Count > 0)
        {
            Pages.text = (currentIndex + 1) + " / " + SenarioList.Count;
        }
        else
        {
            Pages.text = "0 / 0";
        }
    }
}
