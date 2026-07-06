using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderButton : MonoBehaviour
{
    [Header("Scene")]
    public string targetSceneName = "";

    [Header("Fade Settings")]
    public float fadeDuration = 0.45f;
    public int fadeSortingOrder = 9999;

    public ExhibitTheme exhibitTheme;

    private static bool isLoading = false;

    public void LoadTargetScene()
    {
        if (isLoading) return;

        if (string.IsNullOrWhiteSpace(targetSceneName))
        {
            Debug.LogWarning("Target scene name is empty.");
            return;
        }

        isLoading = true;

        SceneFadeRunner.Create(
            exhibitTheme,
            targetSceneName,
            fadeDuration,
            fadeSortingOrder,
            () => isLoading = false
        );
    }
}

public class SceneFadeRunner : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private string sceneName;
    private float duration;
    private Action onFinished;

    public static void Create(ExhibitTheme exhibitTheme, string sceneName, float duration, int sortingOrder, Action onFinished)
    {
        PlayerPrefs.SetInt("Player_Theme", (int)exhibitTheme);
        GameObject canvasObject = new GameObject("Scene Fade Canvas");
        UnityEngine.Object.DontDestroyOnLoad(canvasObject);

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;

        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        CanvasGroup group = canvasObject.AddComponent<CanvasGroup>();
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = true;

        GameObject blackImageObject = new GameObject("Black Fade Image");
        blackImageObject.transform.SetParent(canvasObject.transform, false);

        Image blackImage = blackImageObject.AddComponent<Image>();
        blackImage.color = Color.black;
        blackImage.raycastTarget = true;

        RectTransform rect = blackImageObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        SceneFadeRunner runner = canvasObject.AddComponent<SceneFadeRunner>();
        runner.canvasGroup = group;
        runner.sceneName = sceneName;
        runner.duration = Mathf.Max(0.01f, duration);
        runner.onFinished = onFinished;

        runner.StartCoroutine(runner.Run());
    }

    private IEnumerator Run()
    {
        AsyncOperation operation = null;

        try
        {
            operation = SceneManager.LoadSceneAsync(sceneName);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load scene: " + sceneName + "\n" + e.Message);
            Finish();
            yield break;
        }

        if (operation == null)
        {
            Debug.LogError("Failed to load scene: " + sceneName);
            Finish();
            yield break;
        }

        operation.allowSceneActivation = false;

        // 当前场景渐黑
        yield return Fade(0f, 1f);

        // 等待目标场景加载到可激活状态
        // allowSceneActivation 为 false 时，Unity 的加载进度会停在 0.9 附近等待激活
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // 激活新场景
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        // 等一帧，确保新场景初始化完成
        yield return null;

        // 新场景渐亮
        yield return Fade(1f, 0f);

        Finish();
    }

    private IEnumerator Fade(float from, float to)
    {
        float timer = 0f;
        canvasGroup.alpha = from;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / duration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            canvasGroup.alpha = Mathf.Lerp(from, to, smoothT);

            yield return null;
        }

        canvasGroup.alpha = to;
    }

    private void Finish()
    {
        onFinished?.Invoke();
        Destroy(gameObject);
    }
}