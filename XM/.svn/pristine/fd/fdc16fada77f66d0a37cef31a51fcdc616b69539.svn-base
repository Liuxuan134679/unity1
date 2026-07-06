using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class PlanetSelectSceneBuilder
{
    private const string ScenePath = "Assets/Scenes/PlanetSelect.scene";
    private const string CircleSpritePath = "Assets/Textures/UI/MapGameRadarCircle.png";
    private const string SolidSpritePath = "Assets/Textures/UI/WhiteSprite.png";

    [MenuItem("Tools/XM/Build Planet Select Scene")]
    public static void Build()
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        Sprite circleSprite = AssetDatabase.LoadAssetAtPath<Sprite>(CircleSpritePath);
        Sprite solidSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SolidSpritePath);
        if (solidSprite == null)
            solidSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

        CreateCamera();
        CreateEventSystem();

        Canvas canvas = CreateCanvas();
        RectTransform root = CreatePanel(
            "PlanetSelectRoot",
            canvas.transform,
            Vector2.zero,
            Vector2.zero,
            new Color(0.025f, 0.045f, 0.09f, 1f)).GetComponent<RectTransform>();
        Stretch(root);

        CreateBackground(root, circleSprite, solidSprite);
        CreateText(root, "Title", "选择降落星球", new Vector2(0f, 442f), new Vector2(620f, 76f), 48, Color.white, TextAnchor.MiddleCenter);
        CreateText(root, "Hint", "将鼠标移到星球上，确认是否在此星球降落", new Vector2(0f, 384f), new Vector2(820f, 48f), 25, new Color(0.72f, 0.82f, 0.94f, 1f), TextAnchor.MiddleCenter);

        CreateSun(root, circleSprite);
        CreateRouteLines(root, solidSprite);
        CreateShip(root, solidSprite);

        PlanetOption planet1 = CreatePlanet(root, circleSprite, "Planet_1", "星球1", new Vector2(-130f, -25f), 82f, new Color(0.95f, 0.28f, 0.19f, 1f));
        PlanetOption planet2 = CreatePlanet(root, circleSprite, "Planet_2", "星球2", new Vector2(230f, 90f), 72f, new Color(1.00f, 0.42f, 0.30f, 1f));
        PlanetOption planet3 = CreatePlanet(root, circleSprite, "Planet_3", "星球3", new Vector2(660f, 270f), 96f, new Color(0.98f, 0.20f, 0.16f, 1f));

        RectTransform promptRoot;
        Text promptText;
        CreateLandingPrompt(root, solidSprite, out promptRoot, out promptText);

        planet1.Configure(promptRoot, promptText, root);
        planet2.Configure(promptRoot, promptText, root);
        planet3.Configure(promptRoot, promptText, root);

        promptRoot.SetAsLastSibling();
        promptRoot.gameObject.SetActive(false);

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), ScenePath);
        AddSceneToBuildSettings();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("PlanetSelect scene created: " + ScenePath);
    }

    public static void BuildFromBatch()
    {
        Build();
        EditorApplication.Exit(0);
    }

    private static void CreateCamera()
    {
        GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        Camera camera = cameraObject.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.025f, 0.045f, 0.09f, 1f);
        camera.orthographic = true;
        camera.orthographicSize = 5f;
        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(0f, 0f, -10f);
    }

    private static Canvas CreateCanvas()
    {
        GameObject canvasObject = new GameObject("PlanetSelectCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        return canvas;
    }

    private static void CreateEventSystem()
    {
        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
    }

    private static void CreateBackground(RectTransform root, Sprite circleSprite, Sprite solidSprite)
    {
        int count = 58;
        for (int i = 0; i < count; i++)
        {
            float x = -900f + ((i * 173) % 1800);
            float y = -470f + ((i * 281) % 940);
            float size = 3f + (i % 4) * 2f;
            float alpha = 0.25f + (i % 5) * 0.12f;

            GameObject star = CreateImage(
                "Star_" + i,
                root,
                circleSprite != null ? circleSprite : solidSprite,
                new Vector2(x, y),
                new Vector2(size, size),
                new Color(0.78f, 0.88f, 1f, alpha));
            star.GetComponent<Image>().raycastTarget = false;
        }
    }

    private static void CreateSun(RectTransform root, Sprite circleSprite)
    {
        GameObject glow = CreateImage("SunGlow", root, circleSprite, new Vector2(-1010f, 32f), new Vector2(680f, 680f), new Color(1f, 0.45f, 0.08f, 0.18f));
        glow.GetComponent<Image>().raycastTarget = false;

        GameObject sun = CreateImage("Sun", root, circleSprite, new Vector2(-1035f, 32f), new Vector2(520f, 520f), new Color(1f, 0.72f, 0.15f, 1f));
        sun.GetComponent<Image>().raycastTarget = false;

        CreateText(root, "SunLabel", "太阳", new Vector2(-735f, 38f), new Vector2(100f, 38f), 26, new Color(1f, 0.45f, 0.32f, 1f), TextAnchor.MiddleLeft);
    }

    private static void CreateRouteLines(RectTransform root, Sprite solidSprite)
    {
        Color routeColor = new Color(0.8f, 0.88f, 0.96f, 0.42f);
        CreateLine(root, "Route_A", solidSprite, new Vector2(-600f, -168f), 420f, 4f, 18f, routeColor);
        CreateLine(root, "Route_B", solidSprite, new Vector2(-250f, -72f), 280f, 4f, 38f, routeColor);
        CreateLine(root, "Route_C", solidSprite, new Vector2(76f, -2f), 330f, 4f, 8f, routeColor);
        CreateLine(root, "Route_D", solidSprite, new Vector2(420f, 16f), 470f, 4f, -5f, routeColor);
        CreateLine(root, "Divider", solidSprite, new Vector2(-390f, 95f), 640f, 3f, 92f, new Color(0.9f, 0.18f, 0.14f, 0.42f));
    }

    private static void CreateShip(RectTransform root, Sprite solidSprite)
    {
        RectTransform shipRoot = CreatePanel("Ship", root, new Vector2(420f, 320f), new Vector2(210f, 72f), new Color(0f, 0f, 0f, 0f)).GetComponent<RectTransform>();
        CreateLine(shipRoot, "ShipBody", solidSprite, new Vector2(-34f, 0f), 58f, 8f, -18f, new Color(0.88f, 0.94f, 1f, 1f));
        CreateLine(shipRoot, "ShipWingA", solidSprite, new Vector2(-7f, 8f), 34f, 5f, 32f, new Color(0.88f, 0.94f, 1f, 1f));
        CreateLine(shipRoot, "ShipWingB", solidSprite, new Vector2(-4f, -10f), 34f, 5f, -52f, new Color(0.88f, 0.94f, 1f, 1f));
        CreateText(shipRoot, "ShipLabel", "主角飞船", new Vector2(48f, 0f), new Vector2(122f, 36f), 22, Color.white, TextAnchor.MiddleLeft);
    }

    private static PlanetOption CreatePlanet(
        RectTransform root,
        Sprite circleSprite,
        string objectName,
        string displayName,
        Vector2 position,
        float size,
        Color color)
    {
        GameObject planetObject = CreateImage(objectName, root, circleSprite, position, new Vector2(size, size), color);
        planetObject.AddComponent<PlanetOption>();

        GameObject ring = CreateImage(objectName + "_Ring", planetObject.transform, circleSprite, Vector2.zero, new Vector2(size + 20f, size + 20f), new Color(1f, 1f, 1f, 0.13f));
        ring.transform.SetAsFirstSibling();
        ring.GetComponent<Image>().raycastTarget = false;

        Text label = CreateText(planetObject.transform, objectName + "_Label", displayName, new Vector2(0f, -size * 0.72f), new Vector2(160f, 34f), 22, new Color(1f, 0.58f, 0.58f, 1f), TextAnchor.MiddleCenter);
        label.raycastTarget = false;

        return planetObject.GetComponent<PlanetOption>();
    }

    private static void CreateLandingPrompt(
        RectTransform root,
        Sprite solidSprite,
        out RectTransform promptRoot,
        out Text promptText)
    {
        GameObject promptObject = CreatePanel("LandingPrompt", root, Vector2.zero, new Vector2(430f, 152f), new Color(0.96f, 0.96f, 0.92f, 0.97f));
        promptRoot = promptObject.GetComponent<RectTransform>();

        CreateLine(promptRoot, "BorderTop", solidSprite, new Vector2(0f, 74f), 430f, 4f, 0f, new Color(0.08f, 0.08f, 0.08f, 1f));
        CreateLine(promptRoot, "BorderBottom", solidSprite, new Vector2(0f, -74f), 430f, 4f, 0f, new Color(0.08f, 0.08f, 0.08f, 1f));
        CreateLine(promptRoot, "BorderLeft", solidSprite, new Vector2(-214f, 0f), 152f, 4f, 90f, new Color(0.08f, 0.08f, 0.08f, 1f));
        CreateLine(promptRoot, "BorderRight", solidSprite, new Vector2(214f, 0f), 152f, 4f, 90f, new Color(0.08f, 0.08f, 0.08f, 1f));

        promptText = CreateText(promptRoot, "PromptText", "是否在此星球降落", Vector2.zero, new Vector2(392f, 72f), 28, new Color(0.1f, 0.1f, 0.1f, 1f), TextAnchor.MiddleCenter);
    }

    private static GameObject CreatePanel(string name, Transform parent, Vector2 position, Vector2 size, Color color)
    {
        GameObject go = CreateUIObject(name, parent, position, size);
        Image image = go.AddComponent<Image>();
        image.color = color;
        image.raycastTarget = color.a > 0.01f;
        return go;
    }

    private static GameObject CreateImage(string name, Transform parent, Sprite sprite, Vector2 position, Vector2 size, Color color)
    {
        GameObject go = CreateUIObject(name, parent, position, size);
        Image image = go.AddComponent<Image>();
        image.sprite = sprite;
        image.color = color;
        image.raycastTarget = true;
        return go;
    }

    private static Text CreateText(Transform parent, string name, string text, Vector2 position, Vector2 size, int fontSize, Color color, TextAnchor alignment)
    {
        GameObject go = CreateUIObject(name, parent, position, size);
        Text label = go.AddComponent<Text>();
        label.text = text;
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.fontSize = fontSize;
        label.alignment = alignment;
        label.color = color;
        label.raycastTarget = false;
        label.resizeTextForBestFit = true;
        label.resizeTextMinSize = Mathf.Max(12, fontSize - 10);
        label.resizeTextMaxSize = fontSize;
        return label;
    }

    private static void CreateLine(Transform parent, string name, Sprite sprite, Vector2 position, float length, float thickness, float angle, Color color)
    {
        GameObject go = CreateImage(name, parent, sprite, position, new Vector2(length, thickness), color);
        go.GetComponent<Image>().raycastTarget = false;
        go.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, angle);
    }

    private static GameObject CreateUIObject(string name, Transform parent, Vector2 position, Vector2 size)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);

        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;

        return go;
    }

    private static void Stretch(RectTransform rectTransform)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    private static void AddSceneToBuildSettings()
    {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
        scenes.Add(new EditorBuildSettingsScene(ScenePath, true));

        EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;
        for (int i = 0; i < existingScenes.Length; i++)
        {
            if (existingScenes[i].path != ScenePath)
                scenes.Add(existingScenes[i]);
        }

        EditorBuildSettings.scenes = scenes.ToArray();
    }
}
