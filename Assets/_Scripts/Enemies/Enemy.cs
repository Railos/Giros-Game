using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health;
    public float moneyWhenDie;
    public GameEvent tookDamageEvent;
    public GameEvent diedEvent;

    public float Health
    {
        get => health;
        set
        {
            health = value;
            tookDamageEvent.Raise();
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (Health <= 0f)
        {
            GetComponent<Collider>().enabled = false;
            diedEvent.Raise(this, moneyWhenDie);
            
            Destroy(this.gameObject);
        }
        
        Destroy(Instantiate(ObjectsPool.objectPool.bloodSpray, transform.position, Quaternion.identity, transform), 0.4f);
    }
}
