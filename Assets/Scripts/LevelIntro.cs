
using UnityEngine;
using TMPro;
using System.Collections;

public class LevelIntro : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public CanvasGroup canvasGroup;

    public float duration = 3f;
    public float moveDistance = 100f;

    private RectTransform textTransform;
    private Vector2 startPosition;

    public float displayDuration = 2f;

    void Start()
    {
        Time.timeScale = 0f;

        textTransform = introText.GetComponent<RectTransform>();
        startPosition = textTransform.anchoredPosition;

        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        canvasGroup.alpha = 1f;

        // Wait for 2 seconds before starting the animation
        yield return new WaitForSecondsRealtime(displayDuration);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = elapsed / duration;

            // Move the text upwards
            textTransform.anchoredPosition =
                startPosition + Vector2.up * moveDistance * progress;

            // Fade the text out
            canvasGroup.alpha = 1f - progress;

            yield return null;
        }

        canvasGroup.alpha = 0f;

        // Restore original position
        textTransform.anchoredPosition = startPosition;

        // Resume gameplay
        Time.timeScale = 1f;
    }
}
