using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSexButtonController : MonoBehaviour
{
    public GameObject ChoiceSex;
    public GameObject maleJob; // Male_Job 오브젝트
    public GameObject femaleJob; // Female_Job 오브젝트
    public Button confirmButton; // 확인 버튼

    private enum Gender { None, Male, Female }
    private Gender selectedGender = Gender.None; // 초기 상태는 None

    void Start()
    {
        // 초기 상태에서 확인 버튼 비활성화
        confirmButton.interactable = false;
    }

    // "남성" 선택 시 호출 메서드
    public void OnMaleSelected()
    {
        selectedGender = Gender.Male;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerSex("남성");
    }

    // "여성" 선택 시 호출 메서드
    public void OnFemaleSelected()
    {
        selectedGender = Gender.Female;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerSex("여성");
    }

    // 확인 버튼을 누를 시 호출 메서드
    public void OnConfirmButtonPressed()
    {
        if (selectedGender == Gender.Male)
        {
            maleJob.SetActive(true);
            femaleJob.SetActive(false);
        }
        else if (selectedGender == Gender.Female)
        {
            maleJob.SetActive(false);
            femaleJob.SetActive(true);
        }
    }

    // 되돌리기 버튼을 누를 시 호출 메서드
    public void OnBackButtonPressed()
    {
        // 패널 비활성화
        ChoiceSex.SetActive(false);
    }

    // 선택에 따라 확인 버튼 활성화 여부 업데이트 메서드
    private void UpdateConfirmButton()
    {
        confirmButton.interactable = (selectedGender != Gender.None);
    }
}
