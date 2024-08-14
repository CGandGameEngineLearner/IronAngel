using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelChooseController : MonoBehaviour
{
    [SerializeField]
    public Transform ScrollView;

    [SerializeField]
    public GameObject ItemPrefab;

    [SerializeField]
    public LevelChooseConfig LevelChooseConfig;
    
    public ScrollRect m_ScrollRect;

    public RectTransform m_contentTransform;

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i < LevelChooseConfig.LevelOptionSettings.Count;i++)
        {
            var Item = Instantiate(ItemPrefab);
            var newItemTransform = Item.transform;
            newItemTransform.SetParent(m_contentTransform);
            newItemTransform.localPosition = Vector3.zero;
            newItemTransform.localRotation = Quaternion.identity;
            newItemTransform.localScale = Vector3.one;
            var levelOption = Item.GetComponent<LevelOption>();
            if (levelOption != null)
            {
                levelOption.LevelIcon = LevelChooseConfig.LevelOptionSettings[i].LevelIcon;
                levelOption.LevelSceneName = LevelChooseConfig.LevelOptionSettings[i].LevelSceneName;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
