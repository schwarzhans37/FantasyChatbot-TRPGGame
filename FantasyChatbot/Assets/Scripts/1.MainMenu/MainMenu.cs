using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextBlink : MonoBehaviour
{
    public TextMeshProUGUI tmpText;   // TextMeshPro 오브젝트 연결
    public float blinkDuration = 2.0f; // 알파값 점멸 주기 (1초)
    public float fadeOutDuration = 0.5f; // 입력 후 알파값이 서서히 줄어드는 시간 (0.5초)
    public List<Button> MainMenuButtons; // 버튼 리스트

    private bool isBlinking = true;

    private void Start()
    {
        StartCoroutine(BlinkText()); // 점멸 애니메이션 시작
    }

    private void Update()
    {
        // 마우스 좌클릭, Enter 키, Space 키 입력 체크
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isBlinking)
            {
                isBlinking = false; // 점멸 중단
                StartCoroutine(FadeOutText()); // 텍스트 서서히 사라짐
            }
        }
    }

    private IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < blinkDuration)
            {
                elapsedTime += Time.deltaTime;
                // 알파값을 0에서 1까지 반복
                float alpha = Mathf.PingPong(elapsedTime / (blinkDuration / 2), 1.0f);
                tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, alpha);
                yield return null;
            }
        }
    }

    private IEnumerator FadeOutText()
    {
        float startAlpha = tmpText.color.a;
        float elapsedTime = 0.0f;

        // 텍스트 서서히 사라지는 효과
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0, elapsedTime / fadeOutDuration);
            tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, alpha);
            yield return null;
        }

        tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0); // 완전히 투명화

        // 버튼들을 서서히 나타내는 코루틴 시작
        StartCoroutine(FadeInButtons());
    }

    private IEnumerator FadeInButtons()
    {
        float elapsedTime = 0.0f;
        float fadeInDuration = 1.0f; // 버튼들이 서서히 나타나는 시간 (1초)

        // 모든 버튼의 Image 및 텍스트 알파값을 0으로 설정
        foreach (Button button in MainMenuButtons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                Color color = buttonImage.color;
                buttonImage.color = new Color(color.r, color.g, color.b, 0);
            }

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                Color textColor = buttonText.color;
                buttonText.color = new Color(textColor.r, textColor.g, textColor.b, 0);
            }

            // 버튼을 비활성화
            button.interactable = false;
        }

        // 버튼의 Image 및 텍스트 알파값을 서서히 1로 증가시키기
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration);

            foreach (Button button in MainMenuButtons)
            {
                Image buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    Color color = buttonImage.color;
                    buttonImage.color = new Color(color.r, color.g, color.b, alpha);
                }

                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    Color textColor = buttonText.color;
                    buttonText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
                }
            }

            yield return null;
        }

        // 모든 버튼의 알파값을 1로 설정하고 인터랙션 활성화
        foreach (Button button in MainMenuButtons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                Color color = buttonImage.color;
                buttonImage.color = new Color(color.r, color.g, color.b, 1);
            }

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                Color textColor = buttonText.color;
                buttonText.color = new Color(textColor.r, textColor.g, textColor.b, 1);
            }

            button.interactable = true;
        }
    }
}
