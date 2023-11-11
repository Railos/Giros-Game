using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] float rotationAmount = 4f;
    [SerializeField] float maxRotationAmount = 5f;
    [SerializeField] float smoothRotation = 12f;

    [SerializeField] bool rotationX = true;
    [SerializeField] bool rotationY = true;
    [SerializeField] bool rotationZ = true;
    
    private Quaternion _initialRotation;

    private float _inputX;
    private float _inputY;

    private void Start()
    {
        _initialRotation = transform.rotation;
    }
    private void Update()
    {
        CalculateSway();
        
        TiltSway();
    }
    

    private void CalculateSway()
    {
        _inputX = -Input.GetAxis("Mouse X");
        _inputY = -Input.GetAxis("Mouse Y");
    }
    

    private void TiltSway()
    {
        float tiltY = Mathf.Clamp(_inputX * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(_inputY * rotationAmount, -maxRotationAmount, maxRotationAmount);
        
        Quaternion finalRotation = Quaternion.Euler(new Vector3(
            rotationX ? -tiltX : 0f,
            rotationY ? tiltY : 0f,
            rotationZ ? tiltY : 0));
        transform.localRotation = Quaternion.Slerp(transform.localRotation,finalRotation * _initialRotation, Time.deltaTime * smoothRotation);
    }
}
