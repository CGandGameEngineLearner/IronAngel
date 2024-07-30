using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;


public class CSVReader : EditorWindow
{
    private int optionIndex = 0;
    private string[] options = new string[] { "Plot" };

    private string m_PlotPath = "Assets/Config/Plot/";

    [MenuItem("Window/CSV Reader")]
    public static void ShowWindow()
    {
        GetWindow<CSVReader>("CSV Reader");
    }
    public void OnGUI()
    {
        GUILayout.Label("Select a CSV file");
        GUILayout.Label("Use to");
        EditorGUILayout.Popup(optionIndex, options);
        if(GUILayout.Button("Select CSV File"))
        {
            string filePath = EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
            if(!string.IsNullOrEmpty(filePath))
            {
                Debug.Log("Read CSV file : " + filePath);
                switch (optionIndex)
                {
                    case 0:
                        {
                            var res = ReadFile(filePath);
                            if(res != null)
                            {
                                PlotConfig config = ScriptableObject.CreateInstance<PlotConfig>();
                                string fileName = m_PlotPath + filePath.Substring(filePath.LastIndexOf('/') + 1);
                                fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + ".asset";
                                foreach(var item in res)
                                {
                                    if(item != null && item.Length > 0)
                                    {
                                        if (item[0] == "СЎПо")
                                        {
                                            var clip = config.m_PlotConfigList[config.m_PlotConfigList.Count - 1];
                                            PlotOption option = new PlotOption();
                                            option.m_OptionContent = item[1];
                                            option.m_PlotClips = new List<PlotClip>();
                                            for(int i = 2; i < item.Length; i++)
                                            {
                                                if(string.IsNullOrEmpty(item[i]))
                                                {
                                                    continue;
                                                }
                                                PlotClip tempClip = new PlotClip();
                                                tempClip.m_SpeakerName = item[i].Substring(item[i].LastIndexOf("/") + 1);
                                                tempClip.m_Content = item[i].Substring(0, item[i].LastIndexOf("/"));
                                                option.m_PlotClips.Add(tempClip);
                                            }
                                            clip.m_PlotOptions.Add(option);
                                            config.m_PlotConfigList[config.m_PlotConfigList.Count - 1] = clip;
                                        }
                                        else
                                        {
                                            PlotClip clip = new PlotClip();
                                            clip.m_PlotOptions = new List<PlotOption>();
                                            Debug.Log(item[0]);
                                            clip.m_SpeakerName = item[0].Substring(item[0].LastIndexOf("/") + 1);
                                            clip.m_Content = item[0].Substring(0, item[0].LastIndexOf("/"));
                                            config.m_PlotConfigList.Add(clip);
                                        }
                                    }
                                }
                                AssetDatabase.CreateAsset(config, fileName);
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                                Debug.Log("Save SO data : " + fileName);
                            }
                            break;
                        }
                }
            }
        }
    }
    static List<string[]> ReadFile(string path)
    {
        if(path == null && !File.Exists(path))
        {
            return null;
        }
        var reader = new StreamReader(path, System.Text.Encoding.UTF8);
        string line;
        var list = new List<string[]>();
        while ((line = reader.ReadLine()) != null)
        {
            list.Add(line.Split(','));
        }
        reader.Close();
        reader.Dispose();
        return list;
    }
}
