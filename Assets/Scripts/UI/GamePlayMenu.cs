using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class GamePlayMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentLevelText;
        [SerializeField] private TMP_Text currentLevelSeedText;
        [SerializeField] private TMP_Text currentTimeText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeOutDuration;
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float animationIdleTime;
        [SerializeField] private LevelCompletePopup completePopup;
        [SerializeField] private Transform popupParent;
        
        private StringBuilder levelStringBuilder = new();
        private StringBuilder seedStringBuilder = new();
        private Coroutine animationCoroutine;
        
        private const float Enabled = 1;
        private const float Disabled = 0;

        private void Start()
        {
            canvasGroup.alpha = Disabled;
        }

        public void Show(int level, int seed)
        {
            levelStringBuilder.Clear();
            levelStringBuilder.Append(level);

            seedStringBuilder.Clear();
            seedStringBuilder.Append(seed);
            
            currentLevelSeedText.SetText(seedStringBuilder);
            currentLevelText.SetText(levelStringBuilder);

            DoAnimation();
        }

        public void Hide()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            canvasGroup.alpha = 0;
        }

        public void UpdateTimer(string data)
        {
            currentTimeText.SetText(data);
        }

        public void OnLevelComplete(float distance, Action onContinue, Action onExit)
        {
            var popup = Instantiate(completePopup, popupParent);
            
            popup.Init(distance, onContinue, onExit);
        }

        private void DoAnimation()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            animationCoroutine = StartCoroutine(AnimationCoroutine());
        }

        private IEnumerator AnimationCoroutine()
        {
            yield return FadeCoroutine(Enabled, fadeInDuration);
            yield return new WaitForSeconds(animationIdleTime);
            yield return FadeCoroutine(Disabled, fadeOutDuration);
            
            animationCoroutine = null;
        }

        private IEnumerator FadeCoroutine(float targetAlpha, float duration)
        {
            float startAlpha = canvasGroup.alpha;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
        }
    }
}