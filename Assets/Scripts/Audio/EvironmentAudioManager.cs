using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class EvironmentAudioManager : MonoBehaviour
    {
        public Dictionary<string, ScenesEvironmentAudioSetting> m_ScenesEvironmentAudioSettingDic;

        public ScenesEvironmentAudioConfig m_ScenesEvironmentAudioConfig;

        private void OnEnable()
        {
            foreach (var scenesEvironmentAudioSetting in m_ScenesEvironmentAudioSettingDic.Values)
            {
                
            }
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnChangeScene()
        {
        
        }
    }
}

