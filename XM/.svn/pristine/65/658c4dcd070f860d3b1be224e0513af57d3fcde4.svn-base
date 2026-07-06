using System;
using UnityEngine;

/// <summary>
/// 展示品主题 —— 对应博物馆的三排架子
/// </summary>
public enum ExhibitTheme
{
    world1,    // 古代文物
    world2,    // 自然标本
    world3      // 科技发明
}

/// <summary>
/// 展示品数据
/// </summary>
[Serializable]
public class ExhibitData
{
    [Tooltip("唯一标识")]
    public int id;

    [Tooltip("展示品名称")]
    public string name;

    [Tooltip("来自老师的展览品")]
    public bool fromTeacher;

    [Tooltip("详细描述")]
    [TextArea] public string description;

    [Tooltip("所属主题（对应架子排）")]
    public ExhibitTheme theme;

    [Tooltip("展示品图标")]
    public Sprite icon;

    [Tooltip("展示品图标颜色")]
    public Color color = Color.white;
}
