using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonController : MonoBehaviour
{
    // 게임 시작 버튼이 클릭되었을 때 호출되는 메서드
    public void OnGameStartButtonClicked()
    {
        // 캐릭터 생성 씬 로드
        SceneManager.LoadScene("CharaMakingScene");
    }

    // 종료 버튼이 클릭되었을 때 호출
    public void OnExitButtonClicked()
    {
        // 애플리케이션을 종료
        #if UNITY_EDITOR
        // 에디터에서 실행 중일 경우 플레이 중지
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 빌드된 상태에서는 애플리케이션 종료
        Application.Quit();
        #endif
    }
}

