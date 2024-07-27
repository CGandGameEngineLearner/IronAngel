using UnityEngine;

public class LazerWeapon : AWeapon
{
    protected override void InternalLogicShoot(Vector3 startPoint, Vector3 endPoint)
    {
        RaycastHit hit;
        float range = (endPoint - startPoint).magnitude;
        if (Physics.Raycast(startPoint, endPoint - startPoint, out hit, range))
        {
            
        }
    }

    protected override void AppearanceShoot(Vector3 startPoint, Vector3 endPoint)
    {
        float range = (endPoint - startPoint).magnitude;
        Debug.DrawRay(startPoint, endPoint - startPoint, Color.red, 0.1f);
    }
}