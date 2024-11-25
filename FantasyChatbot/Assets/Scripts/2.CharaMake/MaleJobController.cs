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
        // 초기 상태에서 확인 버튼 비활성화
        confirmButton.interactable = false;
    }

    // 남성 기사 선택 시 호출 메서드
    public void OnMaleKnightSelected()
    {
        selectedJob = MaleJobs.MKnight;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerJob("기사");
        PlayerDataManager.Instance.SetPlayerHP(250);
        PlayerDataManager.Instance.SetCurrentHP(250);
        PlayerDataManager.Instance.SetPlayerMP(100);
        PlayerDataManager.Instance.SetCurrentMP(100);
        PlayerDataManager.Instance.SetPlayerGold(500);
    }

    // 남성 마법사 선택 시 호출 메서드
    public void OnMaleMagicianSelected()
    {
        selectedJob = MaleJobs.MMagician;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerJob("마법사");
        PlayerDataManager.Instance.SetPlayerHP(100);
        PlayerDataManager.Instance.SetCurrentHP(100);
        PlayerDataManager.Instance.SetPlayerMP(250);
        PlayerDataManager.Instance.SetCurrentMP(250);
        PlayerDataManager.Instance.SetPlayerGold(500);
    }

    // 남성 궁수 선택 시 호출 메서드
    public void OnMaleArcherSelected()
    {
        selectedJob = MaleJobs.MArcher;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerJob("궁수");
        PlayerDataManager.Instance.SetPlayerHP(200);
        PlayerDataManager.Instance.SetCurrentHP(200);
        PlayerDataManager.Instance.SetPlayerMP(150);
        PlayerDataManager.Instance.SetCurrentMP(150);
        PlayerDataManager.Instance.SetPlayerGold(500);
    }

    // 남성 성직자 선택 시 호출 메서드
    public void OnMalePriestSelected()
    {
        selectedJob = MaleJobs.MPriest;
        UpdateConfirmButton();
        PlayerDataManager.Instance.SetPlayerHP(150);
        PlayerDataManager.Instance.SetCurrentHP(150);
        PlayerDataManager.Instance.SetPlayerMP(200);
        PlayerDataManager.Instance.SetCurrentMP(200);
        PlayerDataManager.Instance.SetPlayerGold(500);
    }

    // 확인 버튼을 누를 시 호출 메서드
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
            // 기존에 생성된 프리팹이 있다면 삭제
            if (PlayerDataManager.Instance.currentProfilePrefab != null)
            {
                Destroy(PlayerDataManager.Instance.currentProfilePrefab);
            }

            // 새로운 프리팹을 생성하고, 생성된 프리팹을 currentProfilePrefab에 저장
            PlayerDataManager.Instance.currentProfilePrefab = Instantiate(prefabToInstantiate, PlayerInfomations);
        }
    }

    // 되돌리기 버튼을 누를 시 호출 메서드
    public void OnBackButtonPressed()
    {
        // 패널 비활성화
        MaleJob.SetActive(false);
    }

    // 선택에 따라 확인 버튼 활성화 여부 업데이트 메서드
    private void UpdateConfirmButton()
    {
        confirmButton.interactable = (selectedJob != MaleJobs.None);
    }
}