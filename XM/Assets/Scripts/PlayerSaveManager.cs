using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家存档管理器 —— 使用 PlayerPrefs 管理单主角存档
/// </summary>
public class PlayerSaveManager : MonoBehaviour
{
    public static PlayerSaveManager ins;
    [Tooltip("ExhibitConfig 配置文件引用")]
    public ExhibitConfig exhibitConfig;

    // PlayerPrefs Keys
    private const string ThemeKey = "Player_Theme";
    private const string CollectedKey = "Player_CollectedExhibits";

    /// <summary>
    /// 存档数据
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public ExhibitTheme currentTheme;
        public List<int> collectedExhibitIds = new List<int>();
    }

    /// <summary>
    /// 当前世界主题
    /// </summary>
    public ExhibitTheme CurrentTheme
    {
        get { return Load().currentTheme; }
    }

    void Start()
    {
        ins = this;
        if (!HasSave())
        {
            Save(ExhibitTheme.world1, new List<int>());
            Debug.Log("[存档] 已初始化默认存档");
        }

        PrintSave();
    }

    // ────────────── 存读档核心方法 ──────────────

    /// <summary>
    /// 保存存档数据
    /// </summary>
    public void Save(ExhibitTheme theme, List<int> collectedIds)
    {
        PlayerPrefs.SetInt(ThemeKey, (int)theme);
        PlayerPrefs.SetString(CollectedKey, string.Join(",", collectedIds));
        PlayerPrefs.Save();

        Debug.Log($"[存档] 已保存 | 主题: {theme} | 已收集: {collectedIds.Count}件");
    }

    /// <summary>
    /// 读取存档数据
    /// </summary>
    public SaveData Load()
    {
        var data = new SaveData();

        if (PlayerPrefs.HasKey(ThemeKey))
            data.currentTheme = (ExhibitTheme)PlayerPrefs.GetInt(ThemeKey);
        else
            data.currentTheme = ExhibitTheme.world1;

        if (PlayerPrefs.HasKey(CollectedKey))
        {
            string raw = PlayerPrefs.GetString(CollectedKey);
            if (!string.IsNullOrEmpty(raw))
            {
                var parts = raw.Split(',');
                foreach (var p in parts)
                {
                    if (int.TryParse(p.Trim(), out int id))
                        data.collectedExhibitIds.Add(id);
                }
            }
        }

        return data;
    }

    /// <summary>
    /// 切换当前世界主题
    /// </summary>
    public void SetTheme(ExhibitTheme theme)
    {
        var data = Load();
        Save(theme, data.collectedExhibitIds);
    }

    /// <summary>
    /// 添加已收集的展览品
    /// </summary>
    public void AddCollectedExhibit(int exhibitId)
    {
        var data = Load();
        if (!data.collectedExhibitIds.Contains(exhibitId))
        {
            data.collectedExhibitIds.Add(exhibitId);
            Save(data.currentTheme, data.collectedExhibitIds);
            Debug.Log($"[存档] 收集了展览品 #{exhibitId}");
        }
    }

    /// <summary>
    /// 判断指定展览品是否已收集
    /// </summary>
    public bool HasCollected(int exhibitId)
    {
        return Load().collectedExhibitIds.Contains(exhibitId);
    }

    /// <summary>
    /// 清除存档
    /// </summary>
    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(ThemeKey);
        PlayerPrefs.DeleteKey(CollectedKey);
        PlayerPrefs.Save();
        Debug.Log("[存档] 已清除存档");
    }

    /// <summary>
    /// 是否存在存档
    /// </summary>
    public bool HasSave()
    {
        return PlayerPrefs.HasKey(ThemeKey);
    }

    /// <summary>
    /// 根据当前世界主题，随机奖励一个未获得过的展览品。
    /// 若当前主题的展览品已全部收集，则返回 null。
    /// </summary>
    public ExhibitData TryRewardRandomExhibit()
    {
        if (exhibitConfig == null)
        {
            Debug.LogWarning("[存档] ExhibitConfig 未配置，无法奖励展览品");
            return null;
        }

        var data = Load();
        ExhibitTheme theme = data.currentTheme;

        // 筛选当前主题下尚未收集的展览品
        var uncollected = new List<ExhibitData>();
        foreach (var exhibit in exhibitConfig.exhibits)
        {
            if (exhibit.theme == theme && !data.collectedExhibitIds.Contains(exhibit.id))
                uncollected.Add(exhibit);
        }

        if (uncollected.Count == 0)
        {
            Debug.Log($"[存档] 主题 {theme} 的展览品已全部收集");
            return null;
        }

        var reward = uncollected[UnityEngine.Random.Range(0, uncollected.Count)];
        AddCollectedExhibit(reward.id);

        Debug.Log($"[存档] 挖掘成功！获得展览品: {reward.name} (主题: {theme})");
        return reward;
    }

    /// <summary>
    /// 打印存档信息
    /// </summary>
    public void PrintSave()
    {
        var data = Load();
        string exhibitNames = "";
        if (exhibitConfig != null)
        {
            var names = new List<string>();
            foreach (var id in data.collectedExhibitIds)
            {
                var exhibit = exhibitConfig.exhibits.Find(e => e.id == id);
                names.Add(exhibit != null ? exhibit.name : $"#{id}");
            }
            exhibitNames = string.Join("、", names);
        }
        else
        {
            exhibitNames = string.Join(", ", data.collectedExhibitIds);
        }

        Debug.Log($"════ 存档 | 当前主题: {data.currentTheme} | 已收集({data.collectedExhibitIds.Count}): {exhibitNames} ════");
    }
}
