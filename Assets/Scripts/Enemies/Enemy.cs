using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{

    public float health;
    public GameEvent tookDamage;
    
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            tookDamage.Raise();
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
        Destroy(Instantiate(ObjectsPool.objectPool.bloodSpray, transform.position, Quaternion.identity), 0.4f);
    }
}
