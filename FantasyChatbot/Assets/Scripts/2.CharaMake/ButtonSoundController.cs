using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundController : MonoBehaviour
{
    public List<Button> confirmButtons; // Confirm 버튼들을 담는 리스트
    public AudioClip confirmSound; // Confirm 버튼 클릭 시 재생할 효과음
    public AudioSource audioSource; // 효과음을 재생할 AudioSource

    private void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing. Please assign an AudioSource component.");
            return;
        }

        if (confirmSound == null)
        {
            Debug.LogError("Confirm sound clip is missing. Please assign an AudioClip.");
            return;
        }

        // 각 Confirm 버튼에 클릭 이벤트 추가
        foreach (Button button in confirmButtons)
        {
            if (button != null)
            {
                button.onClick.AddListener(() => PlayConfirmSound());
            }
            else
            {
                Debug.LogWarning("A button in the confirmButtons list is missing.");
            }
        }
    }

    private void PlayConfirmSound()
    {
        if (audioSource != null && confirmSound != null)
        {
            audioSource.PlayOneShot(confirmSound); // 효과음 재생
        }
    }
}
