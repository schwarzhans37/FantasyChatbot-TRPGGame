using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaleJobController : MonoBehaviour
{
    public GameObject MaleJob;
    public GameObject PlayerInfo;
    public Transform PlayerInfomations;
    public Button confirmButton; // 확인 버튼

    private enum MaleJobs { None, MKnight, MMagician, MArcher, MPriest }
    private MaleJobs selectedJob = MaleJobs.None; // 초기 상태는 None

    void Start()
    {
        // 초기 상태에서 확인 버튼을 비활성화합니다.
        confirmButton.interactable = false;
    }

    // 남성 기사 선택 시 호출될 메서드
    public void OnMaleKnightSelected()
    {
        selectedJob = MaleJobs.MKnight;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerJob("기사");
    }

    // 남성 마법사 선택 시 호출될 메서드
    public void OnMaleMagicianSelected()
    {
        selectedJob = MaleJobs.MMagician;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerJob("마법사");
    }

    // 남성 궁수 선택 시 호출될 메서드
    public void OnMaleArcherSelected()
    {
        selectedJob = MaleJobs.MArcher;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerJob("궁수");
    }

    // 남성 성직자 선택 시 호출될 메서드
    public void OnMalePriestSelected()
    {
        selectedJob = MaleJobs.MPriest;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerJob("성직자");
    }

    // 확인 버튼을 누를 시 호출될 메서드
    public void OnConfirmButtonPressed()
    {
        if (selectedJob == MaleJobs.MKnight || selectedJob == MaleJobs.MMagician || selectedJob == MaleJobs.MArcher || selectedJob == MaleJobs.MPriest)
        {
            PlayerInfo.SetActive(true);
            GenerateProfilePrefab();
        }
        
    }

    private void GenerateProfilePrefab()
    {
        string sex = PlayerDataManager.Instance.playerSex;
        string job = PlayerDataManager.Instance.playerJob;

        GameObject prefabToInstantiate = null;

        if (sex == "남성")
        {
            if (job == "기사")
            {
                prefabToInstantiate = PlayerDataManager.Instance.MaleKnightPrefab;
            }
            else if (job == "마법사")
            {
                prefabToInstantiate = PlayerDataManager.Instance.MaleMagicianPrefab;
            }
            else if (job == "자객")
            {
                prefabToInstantiate = PlayerDataManager.Instance.MaleAssassinPrefab;
            }
            else if (job == "성직자")
            {
                prefabToInstantiate = PlayerDataManager.Instance.MalePriestPrefab;
            }
        }

        if (prefabToInstantiate != null)
        {
            Instantiate(prefabToInstantiate, PlayerInfomations);
        }
    }

    // 되돌리기 버튼을 누를 시 호출될 메서드
    public void OnBackButtonPressed()
    {
        // 패널 비활성화
        MaleJob.SetActive(false);
    }

    // 선택에 따라 확인 버튼 활성화 여부를 업데이트하는 메서드
    private void UpdateConfirmButton()
    {
        confirmButton.interactable = (selectedJob != MaleJobs.None);
    }
}