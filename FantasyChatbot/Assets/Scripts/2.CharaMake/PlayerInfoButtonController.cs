using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.SceneManagement;

public class PlayerInfoButtonController : MonoBehaviour
{
    public GameObject PlayerInfo; // 플레이어 정보입력 오브젝트
    public GameObject SenarioSelect; // 시나리오 선택 오브젝트
    public TMP_InputField playerNameInput;
    public TMP_InputField playerDetailsInput;

    public Button confirmButton; // 확인 버튼

    private string anthropicAPIurl = "https://api.anthropic.com/v1/messages";
    private string anthropicAPIkey = "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";

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
        
        // Claude에게 보낼 데이터를 전송하는 코루틴 실행
        StartCoroutine(SendDataToClaude());

        // 메인 게임 씬 로드
        SceneManager.LoadScene("GameScene");
    }

    // 입력 필드에 값이 있을 경우 확인 버튼을 활성화하는 메서드
    private void UpdateConfirmButton()
    {
        confirmButton.interactable = !string.IsNullOrEmpty(playerNameInput.text) && !string.IsNullOrEmpty(playerDetailsInput.text);
    }
    
    // Claude로 데이터를 전송하는 코루틴
    private IEnumerator SendDataToClaude()
    {        
        string playInfoJson = CreatePlayInfoJson();

        using (UnityWebRequest request = new UnityWebRequest(anthropicAPIurl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(playInfoJson);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("x-api-key", anthropicAPIkey);
            request.SetRequestHeader("Anthropic-Version", "2023-06-01");

            // 인증서 확인 무시 (테스트용)
            request.certificateHandler = new AcceptAllCertificatesSignedWithNoCheck();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error Sending Play Data to Claude: " + request.error);
            }
            else
            {
                Debug.Log("Successfully sent player data to Claude: " + request.downloadHandler.text);
            }
        }
    }

    public class AcceptAllCertificatesSignedWithNoCheck : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    // 플레이 정보를 메시지로 구성하는 메서드
    private string CreatePlayInfoMessage()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Senario: " + PlayerDataManager.Instance.selectedSenario);
        sb.AppendLine("Player Name: " + PlayerDataManager.Instance.playerName);
        sb.AppendLine("Player Sex: " + PlayerDataManager.Instance.playerSex);
        sb.AppendLine("Player Job: " + PlayerDataManager.Instance.playerJob);
        sb.AppendLine("Player Details: " + PlayerDataManager.Instance.playerDetails);

        return sb.ToString();
    }

    // 플레이어 정보를 Json 형태로 생성하는 메서드
    private string CreatePlayInfoJson()
    {
        var playData = new
        {
            model = "claude-3-5-sonnet-20241022",
            max_tokens = 1000,
            temperature = 0,
            system = "Respond based on the player details provided.",
            messages = new[]
            {
                new 
                { 
                    role = "user", 
                    content = new[]
                    {
                        new { type = "text", text = "Scenario: " + PlayerDataManager.Instance.selectedSenario + ", Player Name: " + PlayerDataManager.Instance.playerName + ", Player Sex: " + PlayerDataManager.Instance.playerSex + ", Player Job: " + PlayerDataManager.Instance.playerJob + ", Player Details: " + PlayerDataManager.Instance.playerDetails }
                    }
                }
            }
        };

        return JsonConvert.SerializeObject(playData);
    }
}
