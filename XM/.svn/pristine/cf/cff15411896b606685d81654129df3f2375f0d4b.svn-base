using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用图片显示倒计时（mm:ss），4个Image分别对应分钟十位、个位和秒十位、个位。
/// </summary>
public class CountdownImageDisplay : MonoBehaviour
{
    [Header("数字图片 (0-9)")]
    [SerializeField] private Sprite[] digitSprites = new Sprite[10];

    [Header("显示位 Image 引用")]
    [Tooltip("索引0=分钟十位, 1=分钟个位, 2=秒十位, 3=秒个位")]
    [SerializeField] private Image[] digitImages = new Image[4];

    /// <summary>
    /// 设置显示的时间。
    /// </summary>
    public void SetTime(int minutes, int seconds)
    {
        minutes = Mathf.Clamp(minutes, 0, 99);
        seconds = Mathf.Clamp(seconds, 0, 59);

        SetDigit(0, minutes / 10);
        SetDigit(1, minutes % 10);
        SetDigit(2, seconds / 10);
        SetDigit(3, seconds % 10);
    }

    /// <summary>
    /// 用浮点秒数设置时间（内部转换为 mm:ss）。
    /// </summary>
    public void SetTime(float totalSeconds)
    {
        totalSeconds = Mathf.Max(0f, totalSeconds);
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds % 60f);
        SetTime(minutes, seconds);
    }

    private void SetDigit(int index, int digit)
    {
        if (index < 0 || index >= digitImages.Length) return;
        if (digitImages[index] == null) return;
        if (digit >= 0 && digit < digitSprites.Length && digitSprites[digit] != null)
            digitImages[index].sprite = digitSprites[digit];
    }
}
