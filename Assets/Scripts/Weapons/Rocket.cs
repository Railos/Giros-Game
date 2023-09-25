using EZCameraShake;
using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    [Header("Stats")]
    public float radius;
    public float damage;
    public float explosionForce;

    public GameEvent explosionEvent;

    [Header("Camera Shake")] 
    public float magnitude;
    public float roughness;
    public float fadeInTime;
    public float fadeOutTime;
    
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(Player.player.GetComponent<PlayerLook>().cam.transform.forward * 30f,
            ForceMode.Impulse);
        StartCoroutine(nameof(Flying));
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("KABOOM");

        StopCoroutine(Flying());
        Instantiate(ObjectsPool.objectPool.bigExplosion, transform.position, Quaternion.identity);
        Explode();
        
        Destroy(this.gameObject);
    }

    private IEnumerator Flying()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(ObjectsPool.objectPool.bigExplosion, transform.position, Quaternion.identity);
        Explode();
        
        Destroy(this.gameObject);
        
    }

    private void Explode()
    {
        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        explosionEvent.Raise(this);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out Enemy enemy))
            {
                enemy.Health -= damage;
            }
            else if(colliders[i].TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
            }
        }
    }
}
