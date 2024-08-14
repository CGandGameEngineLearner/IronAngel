using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelOption : MonoBehaviour
{
    [SerializeField]
    private Image m_LevelIcon;

    public Sprite LevelIcon
    {
        set { m_LevelIcon.sprite = value; }
        get { return m_LevelIcon.sprite; }
    }
    public string LevelSceneName;

    public void Awake()
    {
        m_LevelIcon = GetComponent<Image>();
    }

    public void OnButtonClick()
    {
        EventCenter.Broadcast<string>(EventType.RequireChangeMultiScene,LevelSceneName);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
