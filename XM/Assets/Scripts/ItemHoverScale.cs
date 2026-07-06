using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform visual;
    public float normalScale = 1f;
    public float hoverScale = 1.15f;
    public float duration = 0.12f;

    private Coroutine scaleRoutine;

    private void Awake()
    {
        if (visual == null)
            visual = transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayScale(hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayScale(normalScale);
    }

    private void PlayScale(float targetScale)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScaleTo(targetScale));
    }

    private IEnumerator ScaleTo(float targetScale)
    {
        Vector3 start = visual.localScale;
        Vector3 end = Vector3.one * targetScale;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            visual.localScale = Vector3.Lerp(start, end, smoothT);

            yield return null;
        }

        visual.localScale = end;
    }
}
