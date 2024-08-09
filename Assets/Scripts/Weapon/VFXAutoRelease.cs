using System;
using UnityEngine;

public class VFXAutoRelease : MonoBehaviour
{
    public VfxType selfVfxType;
    public float autoDestroyTime = 0.2f;
    public bool needRelease = true;

    private float counter = 0; 
    
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