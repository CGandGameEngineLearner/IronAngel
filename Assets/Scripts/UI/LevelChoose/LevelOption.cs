using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelOption : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;

    public Sprite LevelIcon
    {
        set { m_SpriteRenderer.sprite = value; }
        get { return m_SpriteRenderer.sprite; }
    }
    public string LevelSceneName;

    public void OnEnable()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnButtonClick()
    {
        
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
