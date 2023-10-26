using UnityEngine;

public abstract class Interactive : MonoBehaviour
{

    public Camera cam;
    private RaycastHit _rayHit;
    
    public void Interact()
    {
        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out _rayHit, 3f))
        {
            if (_rayHit.collider.GetComponent<Interactive>())
                Debug.Log("CAn Interact");
        }
    }
}
