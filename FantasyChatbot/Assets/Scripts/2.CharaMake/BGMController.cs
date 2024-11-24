using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource bgmAudioSource; // 배경음악을 재생할 AudioSource

    private void Start()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.loop = true; // 배경음악이 반복되도록 설정
            bgmAudioSource.Play(); // 배경음악 재생
        }
        else
        {
            Debug.LogError("AudioSource is missing. Please assign an AudioSource component.");
        }
    }
}
