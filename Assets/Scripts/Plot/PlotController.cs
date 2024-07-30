using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlotController : MonoBehaviour
{
    public PlotConfig m_PlotConfig;
    public List<PlotClip> m_MainPlotClips;
    public List<PlotClip> m_SubPlotClips;

    public int m_CurrentIndex = -1;
    public TextMeshProUGUI m_TextMeshPro;

    public void Start()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
        m_MainPlotClips = m_PlotConfig.m_PlotConfigList;
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            UpdatePlot();
        }
    }

//  private-----------------------------------------------------
    private void UpdatePlot()
    {
        m_CurrentIndex++;
        if(m_MainPlotClips == null)
        {
            
        }
        if (m_PlotConfig.m_PlotConfigList.Count > m_CurrentIndex)
        {
            m_TextMeshPro.text = m_PlotConfig.m_PlotConfigList[m_CurrentIndex].m_Content;
        }
        else
        {
            EndCurrentPlot();
        }
    }

    private void EndCurrentPlot()
    {

    }
}
