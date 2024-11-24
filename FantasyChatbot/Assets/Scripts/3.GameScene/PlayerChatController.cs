using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;

public class PlayerChatController : MonoBehaviour
{
    public TMP_InputField chatInputField; // 채팅 입력 필드
    public Button sendButton; // 채팅 메세지 전송 버튼
    public TextMeshProUGUI chatLogText; // 채팅 로그 UI (채팅 내용을 표시)
    private string openaiAPIurl = "https://api.openai.com/v1/chat/completions";
    private string openaiAPIkey = ""; // 실제 키로 변경해야 합니다.

    private List<Message> chatHistory = new List<Message>(); // GPT 화를 위한 채팅 히스토리 관리

    private void Start()
    {
        sendButton.onClick.AddListener(OnSendButtonPressed);
    }

    private void OnSendButtonPressed()
    {
        string userMessage = chatInputField.text;

        if (!string.IsNullOrEmpty(userMessage))
        {
            // 채팅 로그에 사용자의 메세지 추가
            AddMessageToChatLog("<color=orange>" + PlayerDataManager.Instance.playerName + ": </color><color=white>" + userMessage + "</color>");

            // 채팅 히스토리에 사용자 메세지 추가
            chatHistory.Add(new Message { role = "user", content = userMessage });

            // GPT에게 메세지를 전송
            StartCoroutine(SendMessageToGPT(userMessage));

            // 입력 필드 초기화
            chatInputField.text = string.Empty;
        }
    }

    private void AddMessageToChatLog(string message)
    {
        // 채팅 로그에 새로운 메세지를 추가
        chatLogText.text += message + "\n";

        // 스크롤 뷰의 스크롤을 맨 아래로 이동
        Canvas.ForceUpdateCanvases(); // UI가 업데이트된 후 스크롤 포지션을 변경
        ScrollRect scrollRect = chatLogText.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 0f; // 맨 아래로 스크롤
        }
    }

    private IEnumerator SendMessageToGPT(string userMessage)
    {
        string requestJson = CreateChatJson();

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

                        // 채팅 히스토리에 GPT 답변 추가
                        chatHistory.Add(new Message { role = "assistant", content = gptResponse });

                        // PythonServerController에게 파싱을 요청
                        // PythonServerController.Instance.ParseAIResponse(gptResponse);
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

    private string CreateChatJson()
    {
        // 플레이어 설정 및 시나리오 프롬프트 유지
        var initialMessages = new List<Message>
        {
            new Message { role = "user", content = "Scenario: " + PlayerDataManager.Instance.senarioPrompt + ", Player Name: " + PlayerDataManager.Instance.playerName + ", Player Sex: " + PlayerDataManager.Instance.playerSex + ", Player Job: " + PlayerDataManager.Instance.playerJob + ", Player Max HP: " + PlayerDataManager.Instance.playerHP + ", Player current HP: " + PlayerDataManager.Instance.currentHP + ", Player Max MP: " + PlayerDataManager.Instance.playerMP + ", Player current MP: " + PlayerDataManager.Instance.currentMP + ", Player Gold: " + PlayerDataManager.Instance.playerGold + ", Player Details: " + PlayerDataManager.Instance.playerDetails }
        };

        // 최근 대화 중 10개만 포함
        var recentMessages = chatHistory.Count > 10 ? chatHistory.GetRange(chatHistory.Count - 10, 10) : new List<Message>(chatHistory);

        var playData = new
        {
            model = "gpt-4o-2024-11-20",
            messages = initialMessages.Union(recentMessages).ToArray(),
            max_tokens = 1000
        };

        return JsonConvert.SerializeObject(playData);
    }

    // GPT의 응답을 담기 위한 클래스 정의
    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class GPTResponse
    {
        public List<Choice> choices { get; set; }

        public class Choice
        {
            public Message message { get; set; }
        }
    }
}