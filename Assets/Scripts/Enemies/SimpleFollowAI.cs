using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleFollowAI : MonoBehaviour
{

    private bool isReadyToFollow;
    
    void Start()
    {
        StartCoroutine(waitForFollow());
    }
    
    void Update()
    {
        if (isReadyToFollow)
        {
            GetComponent<NavMeshAgent>().destination = Player.player.transform.position;
        }
    }

    IEnumerator waitForFollow()
    {
        yield return new WaitForSeconds(10f);

        isReadyToFollow = true;
    }
}
