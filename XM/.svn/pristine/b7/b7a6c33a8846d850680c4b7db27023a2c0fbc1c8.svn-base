using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CabinetPager : MonoBehaviour
{
    [Header("Pages")]
    public RectTransform currentPage;
    public RectTransform nextPage;

    [Header("Button")]
    [SerializeField] private Button nextButton;

    [Header("Settings")]
    public float slideDuration = 0.35f;
    public int pageSize = 5;

    [Header("Data")]
    [SerializeField] private ExhibitConfig exhibitConfig;

    private int currentPageIndex = 0;
    private bool isAnimating = false;
    private float pageWidth;

    private static readonly ExhibitTheme[] Themes =
    {
        ExhibitTheme.world1,
        ExhibitTheme.world2,
        ExhibitTheme.world3
    };

    private List<ExhibitData>[] pagesByTheme;

    private void Start()
    {
        pageWidth = ((RectTransform)currentPage.parent).rect.width;

        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextButtonClick);

        LoadCollectedExhibits();
        BuildPage(currentPage, currentPageIndex);

        nextPage.gameObject.SetActive(false);
    }

    private void LoadCollectedExhibits()
    {
        int themeCount = Themes.Length;
        pagesByTheme = new List<ExhibitData>[themeCount];
        for (int i = 0; i < themeCount; i++)
            pagesByTheme[i] = new List<ExhibitData>();

        if (exhibitConfig == null || exhibitConfig.exhibits == null) return;

        string raw = PlayerPrefs.GetString("Player_CollectedExhibits", "");
        var idSet = new HashSet<int>();
        if (!string.IsNullOrEmpty(raw))
        {
            foreach (var p in raw.Split(','))
            {
                if (int.TryParse(p.Trim(), out int id))
                    idSet.Add(id);
            }
        }

        foreach (var exhibit in exhibitConfig.exhibits)
        {
            if (!idSet.Contains(exhibit.id)) continue;
            if (exhibit.fromTeacher) continue;

            int themeIndex = (int)exhibit.theme;
            if (themeIndex < 0 || themeIndex >= themeCount) continue;
            if (pagesByTheme[themeIndex].Count >= pageSize) continue;

            pagesByTheme[themeIndex].Add(exhibit);
        }
    }

    public void OnNextButtonClick()
    {
        if (isAnimating) return;
        if (pagesByTheme == null || pagesByTheme.Length <= 1) return;

        int targetPage = currentPageIndex + 1;
        if (targetPage >= pagesByTheme.Length)
            targetPage = 0;

        StartCoroutine(SlideToPage(targetPage));
    }

    private IEnumerator SlideToPage(int targetPageIndex)
    {
        isAnimating = true;

        BuildPage(nextPage, targetPageIndex);

        currentPage.anchoredPosition = Vector2.zero;
        nextPage.anchoredPosition = new Vector2(pageWidth, 0);

        nextPage.gameObject.SetActive(true);

        float timer = 0f;

        while (timer < slideDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / slideDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            currentPage.anchoredPosition = new Vector2(
                Mathf.Lerp(0, -pageWidth, smoothT), 0);

            nextPage.anchoredPosition = new Vector2(
                Mathf.Lerp(pageWidth, 0, smoothT), 0);

            yield return null;
        }

        currentPage.anchoredPosition = new Vector2(-pageWidth, 0);
        nextPage.anchoredPosition = Vector2.zero;

        currentPageIndex = targetPageIndex;

        RectTransform temp = currentPage;
        currentPage = nextPage;
        nextPage = temp;

        nextPage.gameObject.SetActive(false);
        nextPage.anchoredPosition = Vector2.zero;

        isAnimating = false;
    }

    private void BuildPage(RectTransform pageRoot, int pageIndex)
    {
        ItemSlotUI[] slots = pageRoot.GetComponentsInChildren<ItemSlotUI>(true);

        var items = (pageIndex >= 0 && pageIndex < pagesByTheme.Length)
            ? pagesByTheme[pageIndex]
            : null;

        for (int i = 0; i < slots.Length; i++)
        {
            if (items != null && i < items.Count)
            {
                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
}
