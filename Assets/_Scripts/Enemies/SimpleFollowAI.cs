using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SimpleFollowAI : MonoBehaviour
{

    public bool isReadyToFollow;
    [SerializeField] private int _curZone;

    void Update()
    {
        if (isReadyToFollow)
        {
            GetComponent<NavMeshAgent>().destination = Player.player.transform.position;
        }
    }

    public void StartFollowing(Component sender, object curZone)
    {
        if (curZone.GetType() != typeof(int)) return;
        if (_curZone == (int)curZone)
            isReadyToFollow = true;
    }
}
