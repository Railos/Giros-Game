using System.Collections;
using UnityEngine;

public class GermesBoots : MonoBehaviour, IPickable
{

    public GameEvent pickedUpEvent;
    
    public void PickUp()
    {
        pickedUpEvent.Raise(this);
        Player.player.GetComponent<PlayerMove>().walkSpeed += 4f;
        Player.player.GetComponent<PlayerMove>().playerSprinting.sprintSpeed += 1f;

        Player.player.GetComponent<PlayerMove>().playerJumping.jumpForce += 2f;

        StartCoroutine(nameof(Break));

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    IEnumerator Break()
    {
        yield return new WaitForSeconds(30f);
        
        Player.player.GetComponent<PlayerMove>().walkSpeed -= 4f;
        Player.player.GetComponent<PlayerMove>().playerSprinting.sprintSpeed -= 1f;

        Player.player.GetComponent<PlayerMove>().playerJumping.jumpForce -= 2f;
        
        Destroy(this.gameObject);
    } 
}
