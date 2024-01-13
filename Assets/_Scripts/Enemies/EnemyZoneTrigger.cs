using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZoneTrigger : MonoBehaviour
{
    public GameEvent lockDoorsEvent;
    public GameEvent unlockDoorsEvent;
    public int curZone;
    public int enemiesInZone;
    public bool isActive;

    public void KilledEnemy()
    {
        if (!isActive) return;
        enemiesInZone--;
        if (enemiesInZone > 0) return;
        unlockDoorsEvent.Raise();
        isActive = false;
        Destroy(gameObject);
    }

    public void LockDoors()
    {
        if (isActive) return;
        
        lockDoorsEvent.Raise(null, curZone);
        isActive = true;
    }

}
