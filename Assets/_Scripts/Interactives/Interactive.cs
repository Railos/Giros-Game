using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
    public float checkInteractRange;
    public Camera cam;
    private RaycastHit _rayHit;
    
    public bool CheckInteract()
    {
        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out _rayHit, 3f))
        {
            if (_rayHit.collider.GetComponent<Interactive>())
                return true; 
            return false;
        }
        return false;
    }

    public virtual void Interact()
    {
        
    }
}
