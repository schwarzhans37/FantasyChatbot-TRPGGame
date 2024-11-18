using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

public class PlayerChatController : MonoBehaviour
{
    public TMP_InputField chatInputField; // 채팅 입력 필드
    public Button sendButton; // 채팅 메시지 전송 버튼
    public TextMeshProUGUI chatLogText; // 채팅 로그 UI (채팅 내용을 표시)

    private string anthropicAPIurl = "https://api.anthropic.com/v1/messages";
    private string anthropicAPIkey = "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"; // 실제 키로 변경해야 합니다.

    private void Start()
    {
        sendButton.onClick.AddListener(OnSendButtonPressed);
    }

    private void OnSendButtonPressed()
    {
        string userMessage = chatInputField.text;

        if (!string.IsNullOrEmpty(userMessage))
        {
            // 채팅 로그에 사용자의 메시지 추가
            AddMessageToChatLog("You: " + userMessage);
            
            // Claude에게 메시지를 전송
            StartCoroutine(SendMessageToClaude(userMessage));

            // 입력 필드 초기화
            chatInputField.text = string.Empty;
        }
    }

    private void AddMessageToChatLog(string message)
    {
        // 채팅 로그에 새로운 메시지를 추가
        chatLogText.text += message + "\n";
    }

    private IEnumerator SendMessageToClaude(string userMessage)
    {
        string requestJson = CreateChatJson(userMessage);

        using (UnityWebRequest request = new UnityWebRequest(anthropicAPIurl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestJson);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("x-api-key", anthropicAPIkey);
            request.SetRequestHeader("anthropic-version", "2023-06-01");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error Sending Chat Message to Claude: " + request.error);
            }
            else
            {
                // 성공적으로 응답을 받았을 때 Claude의 답변을 채팅 로그에 추가
                var responseJson = request.downloadHandler.text;
                Debug.Log("Response Text: " + responseJson); // 응답 로그 출력

                try
                {
                    var responseObject = JsonConvert.DeserializeObject<ClaudeResponse>(responseJson);

                    if (responseObject != null && responseObject.completion == null)
                    {
                        // Claude의 응답이 content 배열 내에 있는 경우 처리
                        var parsedResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
                        if (parsedResponse.ContainsKey("content"))
                        {
                            var contentArray = parsedResponse["content"] as Newtonsoft.Json.Linq.JArray;
                            if (contentArray != null && contentArray.Count > 0)
                            {
                                string claudeResponse = contentArray[0]["text"].ToString();
                                AddMessageToChatLog("Claude: " + claudeResponse);
                            }
                        }
                    }
                    else if (responseObject != null && !string.IsNullOrEmpty(responseObject.completion))
                    {
                        AddMessageToChatLog("Claude: " + responseObject.completion);
                    }
                    else
                    {
                        Debug.LogWarning("Response object is null or completion is empty.");
                    }
                }
                catch (JsonException jsonEx)
                {
                    Debug.LogError("JSON Parsing Error: " + jsonEx.Message);
                }
            }
        }
    }

    private string CreateChatJson(string userMessage)
    {
        var playData = new
        {
            model = "claude-3-5-sonnet-20241022",
            max_tokens = 512,
            messages = new[]
            {
                new { role = "user", content = userMessage },
                new { role = "user", content = "Scenario: " + PlayerDataManager.Instance.selectedSenario + ", Player Name: " + PlayerDataManager.Instance.playerName + ", Player Sex: " + PlayerDataManager.Instance.playerSex + ", Player Job: " + PlayerDataManager.Instance.playerJob + ", Player Details: " + PlayerDataManager.Instance.playerDetails }
            }
        };

        return JsonConvert.SerializeObject(playData);
    }
}

public class ClaudeResponse
{
    public string completion { get; set; }
}
