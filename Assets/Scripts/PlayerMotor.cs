using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    public float moveSpeed = 10f;
    public float maxJumpHeight = 5f;
    public float minJumpHeight = 1f;
    public float maxJumpTime = 0.5f;
    public float stoppingSpeed = 10f;
    public AnimationCurve stoppingCurve;

    public bool airControl = true;
    [Range(0,1)]
    public float airControlFactor = 0.75f;
        
    private CharacterController characterController;


    public float maxVelocity = 10;

    private bool grounded = false;
    bool doubleJumped = false;
    private Vector3 move = Vector3.zero;
    float gravity;
    float jumpVelocity;
    float velocityJumpTermination;
    private float inputX = 0;
    private float inputY = 0;

    // Use this for initialization
    void Start () {
        characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        //Calculate our physics constants for this frame
        gravity = (2 * maxJumpHeight) / Mathf.Pow(maxJumpTime, 2);
        jumpVelocity = Mathf.Sqrt(2 * gravity * maxJumpHeight);

        //Calculate the downward velocity needed to exit a jump early. 
        velocityJumpTermination = Mathf.Sqrt(Mathf.Pow(jumpVelocity, 2) + (2 * -gravity) * (maxJumpHeight - minJumpHeight));

        // If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f) ? .7071f : 1.0f;

        if (!grounded)
        {

            move.y -= gravity * Time.deltaTime;

            if (airControl)
            {
                move.x = ((inputX * airControlFactor) * moveSpeed * inputModifyFactor);
                move.z = ((inputY * airControlFactor) * moveSpeed * inputModifyFactor);
            }
            
            if (Input.GetButtonUp("Jump") && move.y > 0)
            {
                //choose the minimum between the exit velocity and current upward velocity
                move.y = Mathf.Min(velocityJumpTermination, move.y);
            }
            if (Input.GetButtonDown("Jump") && !doubleJumped)
            {
                move.y = jumpVelocity;
                doubleJumped = true;
            }

        }
        else
        {
            move.x = (inputX * moveSpeed * inputModifyFactor);
            move.z = (inputY * moveSpeed * inputModifyFactor);
            move.y = -0.75f;
            doubleJumped = false;
            if (Input.GetButtonDown("Jump"))
            {
                move.y = jumpVelocity;
            }
            
        }

        grounded = (characterController.Move(move * Time.deltaTime) & CollisionFlags.Below) !=0;

    }



}
