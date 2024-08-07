using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AutoGetChild : NetworkBehaviour
{
    public HashSet<GameObject> ignoredObjects = new HashSet<GameObject>();

    private void Start()
    {
        foreach (var child in IronAngel.Utils.GetAllChildren(this.transform))
        {
            ignoredObjects.Add(child);
        }
        
        ignoredObjects.Add(this.gameObject);
    }
}