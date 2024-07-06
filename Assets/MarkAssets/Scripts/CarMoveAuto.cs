using UnityEngine;
using System.Collections;

public class CarMoveAuto : MonoBehaviour {
	public WheelJoint2D frontwheel;
	public WheelJoint2D backwheel;

	JointMotor2D motorFront;

	JointMotor2D motorBack;

	public float speedF;

	public float torqueF;
	public float torqueB;


	public bool TractionFront = true;
	public bool TractionBack = true;


	public float carRotationSpeed;
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space) && InGameManager.Instance.currentState == InGameManager.GameState.Build)
		{
			Debug.Log("Space pressed");
			InGameManager.Instance.ConfirmState();
		}
		
		if(InGameManager.Instance.currentState == InGameManager.GameState.Test)
		{
			if (TractionFront) {
				motorFront.motorSpeed = speedF * -1;
				motorFront.maxMotorTorque = torqueF;
				frontwheel.motor = motorFront;
			}
			if (TractionBack) {
				motorBack.motorSpeed = speedF * -1;
				motorBack.maxMotorTorque = torqueF;
				backwheel.motor = motorBack;
			}
		}		
	}
}
