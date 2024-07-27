using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponConfig : ScriptableObject
{
    // 射击间隔
    public float interval;

    // 弹夹大小
    public float magSize;

    // vfx
    // vfs
}


public abstract class AWeapon
{
    public float interval;
    public float magSize;


    protected float m_PastTime = 0;

    public void Init(WeaponConfig config)
    {
        interval = config.interval;
        magSize = config.magSize;
    }


    public bool LogicShoot(Vector3 startPoint, Vector3 endPoint)
    {
        float pastTime = m_PastTime;
        m_PastTime = Time.time;

        if (Time.time - pastTime < interval)
        {
            return false;
        }

        InternalLogicShoot(startPoint, endPoint);

        return true;
    }

    public void AppearanceUpdate(Vector3 startPoint, Vector3 endPoint)
    {
        AppearanceShoot(startPoint, endPoint);
    }

    protected abstract void InternalLogicShoot(Vector3 startPoint, Vector3 endPoint);

    protected abstract void AppearanceShoot(Vector3 startPoint, Vector3 endPoint);
}