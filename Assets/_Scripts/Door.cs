using UnityEngine;

public class Door : MonoBehaviour
{
    private bool _isOpen;
    [SerializeField] private bool canOpenDoor;
    [SerializeField] private Animator animator;
    public void OpenDoor()
    {
        if (!canOpenDoor) return;
        _isOpen = true;
        animator.CrossFade("DoorOpen", 0, 0, 0);
    }

    public void CloseDoor()
    {
        if (!canOpenDoor) return;
        _isOpen = false;
        animator.CrossFade("DoorClose", 0, 0, 0);
    }

    public void LockDoors()
    {
        if (_isOpen) CloseDoor();;
        canOpenDoor = false;
    }
    
    public void UnlockDoors()
    {
        canOpenDoor = true;
        OpenDoor();
        GetComponent<BoxCollider>().enabled = false;
    }
}
