using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] Transform head;
	[SerializeField] private PlayerMove character;
	
	public float headBobFrequency = 1.5f;
	public float headBobHeight = 0.1f;
	[SerializeField] float headBobSwayAngle = 0.5f;
	[SerializeField] float headBobSideMovement = 0.05f;
	[SerializeField] float bobHeightSpeedMultiplier = 0.3f;
	[SerializeField] float bobStrideSpeedLengthen = 0.3f;
	
	[SerializeField] float jumpLandMove = 3;
	[SerializeField] float jumpLandTilt = 60;

	Vector3 originalLocalPos;
	
	float headBobCycle;
	float headBobFade;

	
	float springPos;
	float springVelocity;
	float springElastic = 1.1f;		
	float springDampen = 0.8f;
	float springVelocityThreshold = 0.05f;
	float springPositionThreshold = 0.05f;


	Vector3 prevPosition;
	Vector3 prevVelocity = Vector3.zero;

	void Start () {

		originalLocalPos = head.localPosition;
		prevPosition = character.GetComponent<Rigidbody>().position;
	}
	
	void FixedUpdate () {
		Vector3 velocity = (character.GetComponent<Rigidbody>().position - prevPosition) / Time.deltaTime;
		Vector3 velocityChange = velocity - prevVelocity;
		prevPosition = character.GetComponent<Rigidbody>().position;
		prevVelocity = velocity;
		
		springVelocity -= velocityChange.y;
		springVelocity -= springPos*springElastic;
		springVelocity *= springDampen;
		springPos += springVelocity * Time.deltaTime;
		springPos = Mathf.Clamp( springPos, -.3f, .3f );
		
		if (Mathf.Abs(springVelocity) < springVelocityThreshold && Mathf.Abs (springPos) < springPositionThreshold)
		{
			springVelocity = 0;
			springPos = 0;
		}
		
		float flatVelocity = new Vector3(velocity.x,0,velocity.z).magnitude;
		
		float strideLengthen = 1 + (flatVelocity * bobStrideSpeedLengthen);
		
		headBobCycle += (flatVelocity / strideLengthen) * (Time.deltaTime / headBobFrequency);
		
		float bobFactor = Mathf.Sin(headBobCycle*Mathf.PI*2); 
		float bobSwayFactor = Mathf.Sin(headBobCycle*Mathf.PI*2 + Mathf.PI*.5f);
		bobFactor = 1-(bobFactor*.5f+1);
		bobFactor *= bobFactor;
		
		if (new Vector3(velocity.x,0,velocity.z).magnitude < 0.1f)
		{
			headBobFade = Mathf.Lerp(headBobFade,0,Time.deltaTime);
		} else {
			headBobFade = Mathf.Lerp(headBobFade,1,Time.deltaTime);
		}
		
		float speedHeightFactor = 1 + (flatVelocity * bobHeightSpeedMultiplier);
		
		float xPos = -headBobSideMovement * bobSwayFactor;
		float yPos = springPos * jumpLandMove + bobFactor*headBobHeight*headBobFade*speedHeightFactor;
		float xTilt = -springPos*jumpLandTilt;
		float zTilt = bobSwayFactor*headBobSwayAngle*headBobFade;
		head.localPosition = originalLocalPos + new Vector3(xPos, yPos, 0);
		head.localRotation = Quaternion.Euler(xTilt,0,zTilt);
	}
}
