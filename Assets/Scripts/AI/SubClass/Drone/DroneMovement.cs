using UnityEngine;
using System.Collections;
using NavMeshPlus.Extensions;

public class DroneMovement : AIMovement
{
    protected override IEnumerator MoveToDestinationCoroutine(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01)
        {
            var dir = target - transform.position;
            dir.z = 0;
            dir = dir.normalized;
            transform.position += agent.speed * Time.deltaTime * dir;
            yield return null;
        }
    }
}
