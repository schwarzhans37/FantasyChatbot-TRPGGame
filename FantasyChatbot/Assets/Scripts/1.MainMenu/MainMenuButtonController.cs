using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonController : MonoBehaviour
{
    // 게임 시작 버튼이 클릭되었을 때 호출되는 메서드
    public void OnGameStartButtonClicked()
    {
        // 캐릭터 생성 씬을 로드합니다
        SceneManager.LoadScene("CharaMakingScene");
    }

    // 종료 버튼이 클릭되었을 때 호출되는 메서드
    public void OnExitButtonClicked()
    {
        // 애플리케이션을 종료합니다
        #if UNITY_EDITOR
        // 에디터에서 실행 중일 경우 플레이를 중지합니다
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 빌드된 상태에서는 애플리케이션을 종료합니다
        Application.Quit();
        #endif
    }
}

