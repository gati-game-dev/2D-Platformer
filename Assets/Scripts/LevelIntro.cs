
using UnityEngine;
using TMPro;
using System.Collections;

public class LevelIntro : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public CanvasGroup canvasGroup;

    public float duration = 3f;
    public float moveDistance = 100f;
    public float displayDuration = 2f;

    private RectTransform textTransform;
    private Vector2 startPosition;

    // Remembers whether the intro has already played
    private static bool introAlreadyPlayed = false;

    void Start()
    {
        // If intro has already played, hide it and continue gameplay
        if (introAlreadyPlayed)
        {
            canvasGroup.alpha = 0f;
            Time.timeScale = 1f;
            return;
        }

        // Mark intro as played
        introAlreadyPlayed = true;

        Time.timeScale = 0f;

        textTransform = introText.GetComponent<RectTransform>();
        startPosition = textTransform.anchoredPosition;

        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        canvasGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(displayDuration);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = elapsed / duration;

            textTransform.anchoredPosition =
                startPosition + Vector2.up * moveDistance * progress;

            canvasGroup.alpha = 1f - progress;

            yield return null;
        }

        canvasGroup.alpha = 0f;

        textTransform.anchoredPosition = startPosition;

        Time.timeScale = 1f;
    }
}
