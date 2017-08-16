using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    public float moveSpeed = 12.5f;
    public float maxJumpHeight = 5f;
    public float minJumpHeight = 1f;
    public float maxJumpTime = 0.44f;
    public int jumpCount = 2;
    public bool airControl = true;
    [Range(0, 1)]
    public float airControlFactor = 0.75f;
    public float maxVelocity = 10;

    // Time in ms the player has when falling that they can still do a full double jump
    public int ledgeForgivenessTime = 120;

    private bool grounded = false;
    private Vector3 move = Vector3.zero;
    private float inputX = 0;
    private float inputY = 0;
    private CharacterController characterController = null;
    int jumpCounter = 0;
    float gravity = 0;
    float jumpVelocity = 0;
    float velocityJumpTermination = 0;


    // Use this for initialization
    void Start () {
        characterController = GetComponent<CharacterController>();
	}


    float timer = 0;
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
            timer += 1000 * Time.deltaTime;
            move.y -= gravity * Time.deltaTime;
            if(timer >= ledgeForgivenessTime && jumpCounter < 1)
            {
                jumpCounter++;
            }

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

            if (Input.GetButtonDown("Jump") && jumpCounter < jumpCount)
            {
                
                move.y = jumpVelocity;
                jumpCounter++;
            }

        }
        else
        {
            move.x = (inputX * moveSpeed * inputModifyFactor);
            move.z = (inputY * moveSpeed * inputModifyFactor);
            move.y = -0.75f;
            if (Input.GetButtonDown("Jump"))
            {
                move.y = jumpVelocity;
                jumpCounter++;
            }
            
        }

        
        grounded = (characterController.Move(move * Time.deltaTime) & CollisionFlags.Below) !=0;
         //if we became or stayed grounded on this frame, reset the jump counter
        if (grounded)
        {
            jumpCounter = 0;
            timer = 0;
        }
    }



}
