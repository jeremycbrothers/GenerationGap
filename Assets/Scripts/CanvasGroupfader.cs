using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupfader : MonoBehaviour
{
    public CanvasGroup uiElement;

    public void fadeIn()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 1));
    }
    public void fadeOut()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float fadeTime = 0.5f)
    {
        float timeStart = Time.time;
        float timeSinceStart = Time.time - timeStart;
        float percentComplete = timeSinceStart / fadeTime;

        while (true)
        {
            timeSinceStart = Time.time - timeStart;
            percentComplete = timeSinceStart / fadeTime;

            float currentValue = Mathf.Lerp(start, end, percentComplete);
            cg.alpha = currentValue;

            if (percentComplete >= 1) break;
            yield return new WaitForEndOfFrame();
        }
        print("Done");
    }
}
