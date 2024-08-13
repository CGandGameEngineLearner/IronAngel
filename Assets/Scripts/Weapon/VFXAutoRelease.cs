using System;
using UnityEngine;

public class VFXAutoRelease : MonoBehaviour
{
    public VfxType selfVfxType;
    public float autoDestroyTime = 0.2f;
    public bool needRelease = true;

    private float counter = 0;

    public void PlayAudioClip(AudioClip audioClip)
    {
        var audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource!=null&&audioClip!=null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
    
    public void Update()
    {
        counter += Time.deltaTime;
        if (counter >= autoDestroyTime)
        {
            VfxPool.Instance.ReleaseVfx(selfVfxType, this.gameObject);

            counter = 0;
        }
    }
}