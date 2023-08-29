using System.Collections;
using EZCameraShake;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private RaycastHit _out;
    private GameObject enemy;

    [Header("Stats")] 
    public bool canGrapple;
    public float distance;
    public float distanceFromEnemy;

    public float waitForGrapple;
    public float grappleCooldown;

    public KeyCode grappleKey;

    [Header("Camera Shake")]
    public float magnitude;
    public float roughness;
    public float fadeInTime;
    public float fadeOutTime;

    [Header("Refs")] 
    public GameObject grappleTip;
    public GameObject cam;
    public LineRenderer lineRenderer;

    private void Update()
    {
        if (!canGrapple) return;
        if (Player.player.GetComponent<PlayerMove>().isForcingDown) return;
        var cameraCenter = cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, cam.GetComponent<Camera>().nearClipPlane));
        if (!Physics.Raycast(cameraCenter, transform.forward, out _out, distance)) return;
        if (!Input.GetKeyDown(grappleKey)) return;
        if (_out.collider.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        if (_out.distance < distanceFromEnemy) return;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, _out.transform.position);
        enemy = _out.collider.gameObject;
        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        Player.player.GetComponent<PlayerMove>().enabled = false;
        canGrapple = false;
        Player.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        StartCoroutine(WaitForGrapple());
    }

    private void LateUpdate()
    {
        lineRenderer.SetPosition(0, grappleTip.transform.position);
    }
    
    IEnumerator WaitForGrapple()
    {
        yield return new WaitForSeconds(waitForGrapple);

        StartCoroutine(DoGrapple());
    }

    IEnumerator DoGrapple()
    {
        var moveDir = (enemy.transform.position - Player.player.transform.position).normalized;
        
        float startTime = Time.time;
        
        Player.player.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition;
        Player.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        
        Player.player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        var grappleForce = Vector3.Distance(transform.position, enemy.transform.position) * 700f * Time.deltaTime;
        while (Time.time < startTime + 0.5f)
        {
            Player.player.GetComponent<Rigidbody>().velocity = moveDir * grappleForce;
            if (Player.player.GetComponent<Rigidbody>().velocity.y > 5f) { Player.player.GetComponent<Rigidbody>().velocity = new Vector3(Player.player.GetComponent<Rigidbody>().velocity.x, 0f, Player.player.GetComponent<Rigidbody>().velocity.z); }

            yield return null;
        }
        
        Player.player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        lineRenderer.enabled = false;
        Player.player.GetComponent<PlayerMove>().enabled = true;
        yield return new WaitForSeconds(grappleCooldown);
        canGrapple = true;
    }
}
