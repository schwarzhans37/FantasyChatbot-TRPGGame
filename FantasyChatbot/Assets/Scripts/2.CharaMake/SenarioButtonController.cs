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
