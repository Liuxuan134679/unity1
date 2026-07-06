using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlanetOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private string promptMessage = "是否在此星球降落?";

    [SerializeField] private float hoverScale = 1.25f;
    [SerializeField] private float scaleSpeed = 12f;
    [SerializeField] private RectTransform promptRoot;
    [SerializeField] private Text promptText;
    [SerializeField] private RectTransform promptParent;
    [SerializeField] private Vector2 promptOffset = new Vector2(0f, -200f);
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip clickClip;

    private RectTransform rectTransform;
    private Vector3 baseScale;
    private Vector3 targetScale;
    private AudioSource audioSource;

    public RectTransform RectTransform
    {
        get
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            return rectTransform;
        }
    }

    public void Configure(RectTransform prompt, Text text, RectTransform parent)
    {
        promptRoot = prompt;
        promptText = text;
        promptParent = parent;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        baseScale = rectTransform != null ? rectTransform.localScale : Vector3.one;
        targetScale = baseScale;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (promptRoot != null)
            promptRoot.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        baseScale = rectTransform != null ? rectTransform.localScale : Vector3.one;
        targetScale = baseScale;
    }

    private void Update()
    {
        if (rectTransform == null)
            return;

        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale,
            targetScale,
            Time.unscaledDeltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = baseScale * hoverScale;
        ShowPrompt();

        if (hoverClip != null && audioSource != null)
            audioSource.PlayOneShot(hoverClip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = baseScale;
        HidePrompt();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickClip != null && audioSource != null)
            audioSource.PlayOneShot(clickClip);
    }

    private void ShowPrompt()
    {
        if (promptRoot == null)
            return;

        if (promptText != null)
            promptText.text = promptMessage;

        PositionPrompt();
        promptRoot.gameObject.SetActive(true);
    }

    private void HidePrompt()
    {
        if (promptRoot != null)
            promptRoot.gameObject.SetActive(false);
    }

    private void PositionPrompt()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
            return;

        if (promptParent != null && rectTransform.parent == promptParent)
        {
            promptRoot.anchoredPosition = rectTransform.anchoredPosition + promptOffset;
            return;
        }

        promptRoot.position = rectTransform.position + new Vector3(promptOffset.x, promptOffset.y, 10f);
    }
}
