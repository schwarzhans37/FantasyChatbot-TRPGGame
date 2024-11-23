using System.Diagnostics;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PythonServerController : MonoBehaviour
{
    private string pythonServerUrl = "http://127.0.0.1:5000/process_response";

    void Start()
    {
        StartPythonServer(); // Unity가 시작될 때 Python 서버를 실행
    }

    void StartPythonServer()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = "FlaskServer.py", // Python 서버 파일 이름 (같은 경로에 있기 때문에 파일명만 지정)
            WorkingDirectory = Application.dataPath + "/Scripts/PythonServer", // 현재 경로를 기준으로 Python 파일 위치 지정
            CreateNoWindow = true, // 창을 만들지 않음
            UseShellExecute = false // 명령 프롬프트 없이 실행
        };

        Process process = Process.Start(startInfo);
    }

    public IEnumerator SendMessageToPythonServer(string message)
    {
        // POST 요청으로 데이터를 Python 서버에 전송
        WWWForm form = new WWWForm();
        form.AddField("message", message);

        using (UnityWebRequest www = UnityWebRequest.Post(pythonServerUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                UnityEngine.Debug.LogError("Error Sending Message to Python Server: " + www.error);
            }
            else
            {
                // Python 서버로부터의 응답 처리
                string responseText = www.downloadHandler.text;
                UnityEngine.Debug.Log("Response from Python Server: " + responseText);

                // Python 서버 응답을 파싱하여 상태 변화 반영 (여기서는 다른 클래스에서 처리해야 합니다)
                PlayerChatController playerChatController = FindObjectOfType<PlayerChatController>();
                if (playerChatController != null)
                {
                    playerChatController.ApplyAIResponse(responseText);
                }
                else
                {
                    UnityEngine.Debug.LogError("PlayerChatController not found in the scene.");
                }
            }
        }
    }
}
