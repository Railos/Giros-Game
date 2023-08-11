using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{
    public static ObjectsPool objectPool;

    public GameObject bloodSpray;

    private void Awake()
    {
        if (objectPool != null)
        {
            Destroy(objectPool);
        }
        else
        {
            objectPool = this;
        }
        
        DontDestroyOnLoad(this);
    }
}
