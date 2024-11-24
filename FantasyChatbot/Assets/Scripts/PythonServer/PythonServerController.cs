using System.Diagnostics;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

public class PythonServerController : MonoBehaviour
{
    public static PythonServerController Instance; // Singleton Instance

    private string pythonServerUrl = "http://127.0.0.1:5000/parse_ai_response"; // Python 서버의 주소

    private void Awake()
    {
        // Singleton 패턴 적용
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartPythonServer(); // Unity가 시작될 때 Python 서버를 실행
    }

    void StartPythonServer()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = "Perse_Response_Server.py", // Python 서버 파일 이름 (같은 경로에 있기 때문에 파일명만 지정)
            WorkingDirectory = Application.dataPath + "/Scripts/PythonServer", // 현재 경로를 기준으로 Python 파일 위치 지정
            CreateNoWindow = true, // 창을 만들지 않음
            UseShellExecute = false // 명령 프롬프트 없이 실행
        };

        Process process = Process.Start(startInfo);
    }

    public void ParseAIResponse(string responseText)
    {
        StartCoroutine(SendToPythonServer(responseText));
    }

    private IEnumerator SendToPythonServer(string responseText)
    {
        // 요청 데이터를 JSON으로 준비
        var requestData = new
        {
            response = responseText
        };

        string json = JsonConvert.SerializeObject(requestData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        // UnityWebRequest를 이용한 POST 요청
        using (UnityWebRequest request = new UnityWebRequest(pythonServerUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                UnityEngine.Debug.LogError("Error Sending Response to Python Server: " + request.error);
            }
            else
            {
                // 서버로부터 받은 응답을 처리
                var responseJson = request.downloadHandler.text;
                try
                {
                    var parsedResponse = JsonConvert.DeserializeObject<Dictionary<string, int>>(responseJson);

                    if (parsedResponse != null)
                    {
                        if (parsedResponse.ContainsKey("hp_change"))
                        {
                            int hpChange = parsedResponse["hp_change"];
                            PlayerDataManager.Instance.SetPlayerHP(Mathf.Clamp(PlayerDataManager.Instance.currentHP + hpChange, 0, PlayerDataManager.Instance.playerHP));
                        }

                        if (parsedResponse.ContainsKey("mp_change"))
                        {
                            int mpChange = parsedResponse["mp_change"];
                            PlayerDataManager.Instance.SetPlayerMP(Mathf.Clamp(PlayerDataManager.Instance.currentMP + mpChange, 0, PlayerDataManager.Instance.playerMP));
                        }

                        if (parsedResponse.ContainsKey("gold_change"))
                        {
                            int goldChange = parsedResponse["gold_change"];
                            PlayerDataManager.Instance.SetPlayerGold(Mathf.Max(PlayerDataManager.Instance.playerGold + goldChange, 0));
                        }
                    }
                }
                catch (JsonException jsonEx)
                {
                    UnityEngine.Debug.LogError("JSON Parsing Error while processing Python server response: " + jsonEx.Message);
                }
            }
        }
    }
}
