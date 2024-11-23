using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Text.RegularExpressions;

public class PlayerChatController : MonoBehaviour
{
    public TMP_InputField chatInputField; // 채팅 입력 필드
    public Button sendButton; // 채팅 메시지 전송 버튼
    public TextMeshProUGUI chatLogText; // 채팅 로그 UI (채팅 내용을 표시)
    private string pythonServerUrl = "http://localhost:5000/process_response";
    private string openaiAPIurl = "https://api.openai.com/v1/chat/completions";
    private string openaiAPIkey = "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"; // 실제 키로 변경해야 합니다.

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
            AddMessageToChatLog("<color=orange>Player: </color></color=white>" + userMessage + "</color>");
            
            // GPT에게 메시지를 전송
            StartCoroutine(SendMessageToGPT(userMessage));

            // 입력 필드 초기화
            chatInputField.text = string.Empty;
        }
    }

    private void AddMessageToChatLog(string message)
    {
        // 채팅 로그에 새로운 메시지를 추가
        chatLogText.text += message + "\n";
    }

    private IEnumerator SendMessageToGPT(string userMessage)
    {
        string requestJson = CreateChatJson(userMessage);

        using (UnityWebRequest request = new UnityWebRequest(openaiAPIurl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestJson);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + openaiAPIkey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error Sending Chat Message to GPT: " + request.error);
            }
            else
            {
                // 성공적으로 응답을 받았을 때 GPT의 답변을 채팅 로그에 추가
                var responseJson = request.downloadHandler.text;
                Debug.Log("Response Text: " + responseJson); // 응답 로그 출력

                try
                {
                    var responseObject = JsonConvert.DeserializeObject<GPTResponse>(responseJson);

                    if (responseObject != null && responseObject.choices != null && responseObject.choices.Count > 0)
                    {
                        string gptResponse = responseObject.choices[0].message.content;
                        AddMessageToChatLog("<i><color=#808080>GPT: " + gptResponse + "</color></i>");
                        ApplyAIResponse(gptResponse); // AI 응답을 플레이어 상태에 반영
                    }
                    else
                    {
                        Debug.LogWarning("Response object is null or choices are empty.");
                    }
                }
                catch (JsonException jsonEx)
                {
                    Debug.LogError("JSON Parsing Error: " + jsonEx.Message);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Unexpected Error: " + ex.Message);
                }
            }
        }
    }

    private string CreateChatJson(string userMessage)
    {
        var playData = new
        {
            model = "gpt-4o-2024-11-20",
            messages = new[]
            {
                new { role = "user", content = userMessage },
                new { role = "user", content = "Scenario: " + PlayerDataManager.Instance.selectedSenario 
                + ", Player Name: " + PlayerDataManager.Instance.playerName 
                + ", Player Sex: " + PlayerDataManager.Instance.playerSex 
                + ", Player Job: " + PlayerDataManager.Instance.playerJob 
                + ", Player Max HP: " + PlayerDataManager.Instance.playerHP
                + ", Player current HP: " + PlayerDataManager.Instance.currentHP
                + ", Player Max MP: " + PlayerDataManager.Instance.playerMP
                + ", Player current MP: " + PlayerDataManager.Instance.currentMP
                + ", Player Gold: " + PlayerDataManager.Instance.playerGold
                + ", Player Details: " + PlayerDataManager.Instance.playerDetails }
            },
            max_tokens = 1000
        };

        return JsonConvert.SerializeObject(playData);
    }

    // AI의 응답을 받고 이를 처리하는 메서드
    public void ApplyAIResponse(string response)
    {
        // Python 서버로 보내는 코루틴 시작
        StartCoroutine(SendToPythonServer(response)); 
    }

    // AI 응답을 처리하기 위해 Python 서버로 보내는 메서드
    private IEnumerator SendToPythonServer(string aiResponse)
    {
        // 요청할 JSON 데이터를 생성
        var requestData = new Dictionary<string, string>
        {
            { "response", aiResponse }
        };
        string jsonData = JsonConvert.SerializeObject(requestData);

        // UnityWebRequest를 이용하여 POST 요청
        using (UnityWebRequest request = new UnityWebRequest(pythonServerUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청을 보냄
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error Sending Data to Python Server: " + request.error);
            }
            else
            {
                // 서버에서 반환된 응답 처리
                string responseText = request.downloadHandler.text;
                Debug.Log("Received from Python Server: " + responseText);

                try
                {
                    // JSON 응답을 파싱하여 데이터 반영
                    var parsedResponse = JsonConvert.DeserializeObject<Dictionary<string, int>>(responseText);
                    if (parsedResponse != null)
                    {
                        int hpChange = parsedResponse.ContainsKey("hp_change") ? parsedResponse["hp_change"] : 0;
                        int mpChange = parsedResponse.ContainsKey("mp_change") ? parsedResponse["mp_change"] : 0;
                        int goldChange = parsedResponse.ContainsKey("gold_change") ? parsedResponse["gold_change"] : 0;

                        PlayerDataManager.Instance.SetPlayerHP(
                            Mathf.Clamp(PlayerDataManager.Instance.currentHP + hpChange, 0, PlayerDataManager.Instance.playerHP)
                        );
                        PlayerDataManager.Instance.SetPlayerMP(
                            Mathf.Clamp(PlayerDataManager.Instance.currentMP + mpChange, 0, PlayerDataManager.Instance.playerMP)
                        );
                        PlayerDataManager.Instance.SetPlayerGold(
                            Mathf.Max(PlayerDataManager.Instance.playerGold + goldChange, 0)
                        );
                    }
                }
                catch (JsonException jsonEx)
                {
                    Debug.LogError("JSON Parsing Error: " + jsonEx.Message);
                }
            }
        }
    }
}

public class GPTResponse
{
    public List<Choice> choices { get; set; }

    public class Choice
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}
