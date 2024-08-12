using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Audio
{
     public enum EPlayModeOfEvironmentAudio
     {
          [Tooltip("无播放模式 不播放任何音乐 阻断之前播放的音乐")]
          None,
          
          [Tooltip("直接播放，抢占式")]
          DirectlyPlay,
          
          [Tooltip("如果当前已经在播某一首歌了 那么就一直继续播放它 如果没有就播放配置的新的歌")]
          LastPlay,
     }

     [Serializable]
     public struct SceneEvironmentAudioSetting
     {
          [SerializeField]
          public string m_SceneName;
          
          [SerializeField]
          public EPlayModeOfEvironmentAudio m_EPlayModeOfEvironmentAudio;
          
          [SerializeField]
          public bool m_Loop;
          
          [FormerlySerializedAs("m_AudioSource")] [SerializeField]
          public AudioClip m_AudioClip;
     }

     [CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/EvironmentAudioConfig", order = 1)]
     public class ScenesEvironmentAudioConfig : ScriptableObject
     {
          public List<SceneEvironmentAudioSetting> m_SceneEvironmentAudioSettings =
               new List<SceneEvironmentAudioSetting>();
     }
}
