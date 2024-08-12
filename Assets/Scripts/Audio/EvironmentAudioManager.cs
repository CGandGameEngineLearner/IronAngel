using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    public class EvironmentAudioManager : MonoBehaviour
    {
        public Dictionary<string, SceneEvironmentAudioSetting> m_SceneEvironmentAudioSettingDic;

        public ScenesEvironmentAudioConfig m_ScenesEvironmentAudioConfig;

        private AudioSource m_AudioSource;

        private void OnEnable()
        {
            m_AudioSource = GetComponent<AudioSource>();
            foreach (var sceneEvironmentAudioSetting in m_ScenesEvironmentAudioConfig.m_SceneEvironmentAudioSettings)
            {
                var sceneName = sceneEvironmentAudioSetting.m_SceneName;
                m_SceneEvironmentAudioSettingDic[sceneName] = sceneEvironmentAudioSetting;
            }
            EventCenter.AddListener(EventType.ChangeScene,OnChangeScene);
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
            string currentSceneName = SceneManager.GetActiveScene().name;
            var sceneEvironmentAudioSetting = m_SceneEvironmentAudioSettingDic[currentSceneName];
            var playModeOfEvironmentAudio = sceneEvironmentAudioSetting.m_EPlayModeOfEvironmentAudio;
            var loop = sceneEvironmentAudioSetting.m_Loop;
            if (playModeOfEvironmentAudio == EPlayModeOfEvironmentAudio.None)
            {
                m_AudioSource.Stop();
                return;
            }
            else if (playModeOfEvironmentAudio == EPlayModeOfEvironmentAudio.DirectlyPlay)
            {
                m_AudioSource.clip = sceneEvironmentAudioSetting.m_AudioClip;
                m_AudioSource.loop = loop;
                m_AudioSource.Play();
            }
            else if (playModeOfEvironmentAudio == EPlayModeOfEvironmentAudio.LastPlay)
            {
                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.clip = sceneEvironmentAudioSetting.m_AudioClip;
                    m_AudioSource.loop = loop;
                    m_AudioSource.Play();
                }
            }
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener(EventType.ChangeScene,OnChangeScene);
        }
    }
}

