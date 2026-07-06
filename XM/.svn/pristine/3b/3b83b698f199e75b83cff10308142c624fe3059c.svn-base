using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StarDialogueController : MonoBehaviour
{
    [Header("Target Scene")]
    public string targetSceneName = "PlanetSelect";

    [Header("Fade")]
    public float fadeDuration = 0.8f;

    [Header("Typewriter")]
    public float charInterval = 0.06f;

    [Header("Dialogue UI References")]
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private GameObject dialogueContainer;
    [SerializeField] private Text lineA;
    [SerializeField] private Text lineB;
    [SerializeField] private Text clickHint;

    [Header("Dialogue Content")]
    [TextArea] public string[] lines = new string[]
    {
        "嘟嘟嘟......星际考古联盟来电，请接通",
        "后续将由你接手你老师的工作，他最后一次进入遗址就失联了",
        "他还活着吗？",
        "信号断了，我们也在等结果。",
        "那我现在要做什么",
        "继续他的工作，收集旧文明遗产，尝试找到他留下的线索。",
        "提醒你，外层有太阳耀斑，每5分钟爆发一轮。",
        "别待太久，必须在窗口期回母舰。"
    };

    private int currentIndex = 0;
    private bool isTyping = false;
    private bool dialogueStarted = false;
    private Coroutine typingCoroutine;

    private static readonly Color TextColor = Color.white;

    private void Start()
    {
        // Hide dialogue UI initially
        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.blocksRaycasts = false;
            fadeGroup.gameObject.SetActive(false);
        }
        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);
        if (clickHint != null)
            clickHint.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleClick();
    }

    private void HandleClick()
    {
        if (!dialogueStarted)
        {
            StartCoroutine(StartDialogueSequence());
            return;
        }

        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            isTyping = false;
            GetCurrentText().text = lines[currentIndex];
            clickHint.gameObject.SetActive(true);
            return;
        }

        currentIndex++;

        if (currentIndex >= lines.Length)
        {
            StartCoroutine(EndDialogueSequence());
            return;
        }

        typingCoroutine = StartCoroutine(TypeLine(currentIndex));
    }

    private Text GetCurrentText()
    {
        return (currentIndex % 2 == 0) ? lineA : lineB;
    }

    private IEnumerator StartDialogueSequence()
    {
        dialogueStarted = true;

        // Hide original button + background image
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().enabled = false;
        transform.Find("Text (Legacy)")?.gameObject.SetActive(false);

        var hostCanvas = GetComponentInParent<Canvas>();

        // Show fade image at full opacity FIRST, then hide background
        fadeGroup.gameObject.SetActive(true);
        fadeGroup.alpha = 1f;
        fadeGroup.blocksRaycasts = true;

        if (hostCanvas != null)
        {
            var bg = hostCanvas.transform.Find("Image");
            if (bg != null) bg.gameObject.SetActive(false);
        }

        // Now fade from visible black to black (instant, no gap)
        // Then show dialogue container
        dialogueContainer.SetActive(true);

        yield return new WaitForSecondsRealtime(0.2f);
        typingCoroutine = StartCoroutine(TypeLine(0));
    }

    private IEnumerator TypeLine(int index)
    {
        isTyping = true;

        Text current = GetCurrentText();

        if (index % 2 == 0 && index > 0)
        {
            lineA.text = "";
            lineB.text = "";
        }

        clickHint.gameObject.SetActive(false);

        string fullText = lines[index];
        current.text = "";
        current.color = TextColor;

        for (int i = 0; i < fullText.Length; i++)
        {
            current.text = fullText.Substring(0, i + 1);
            yield return new WaitForSecondsRealtime(charInterval);
        }

        isTyping = false;
        clickHint.gameObject.SetActive(true);
    }

    private IEnumerator EndDialogueSequence()
    {
        clickHint.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.3f);

        SceneFadeRunner.Create(
            ExhibitTheme.world1,
            targetSceneName,
            fadeDuration,
            9999,
            null
        );
    }
}
