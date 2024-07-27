using UnityEngine;

public class MissileWeapon : AWeapon
{
    protected override void InternalLogicShoot(Vector3 startPoint, Vector3 endPoint)
    {
        
    }

    protected override void AppearanceShoot(Vector3 startPoint, Vector3 endPoint)
    {
        float range = (endPoint - startPoint).magnitude;
        Debug.DrawRay(startPoint, endPoint - startPoint, Color.red, 0.1f);
    }
}