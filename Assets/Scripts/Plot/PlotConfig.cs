using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlotConfig", menuName = "ScriptableObjects/PlotConfig", order = 1)]
public class PlotConfig : ScriptableObject
{
    public List<PlotClip> m_PlotConfigList = new List<PlotClip>();
}

[Serializable]
public struct PlotClip
{
    public string m_SpeakerName;

    [TextArea(2, 4)]
    public string m_Content;

    public List<PlotOption> m_PlotOptions;
}

[Serializable]
public struct PlotOption
{
    [TextArea(2, 4)]
    public string m_OptionContent;

    public List<PlotClip> m_PlotClips;
}
