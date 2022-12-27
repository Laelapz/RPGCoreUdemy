using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutimmediately()
        {
            canvasGroup.alpha = 1f;
        }

        public IEnumerator FadeOut(float time)
        {
            canvasGroup.alpha = 0f;
            while (canvasGroup.alpha < 1)
            {
               canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            canvasGroup.alpha = 1f;
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }

    }

}