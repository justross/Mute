using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    public float acceleration = 3;
    public float maxSpeed = 10;
    public float stoppingSpeed = 10f;
    public float airStoppingSpeed = 0.95f;
    public float maxJumpHeight = 5f;
    public float minJumpHeight = 1f;
    public float maxJumpTime = 0.44f;
    public int jumpCount = 2;
    public bool airControl = true;
    [Range(0, 1)]
    public float airControlFactor = 0.75f;
    

    // Time in ms the player has when falling that they can still do a full double jump
    public int ledgeForgivenessTime = 120;

    public Vector3 velocity = Vector3.zero;

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

            CalculateDrag();

            timer += 1000 * Time.deltaTime;
            move.y -= gravity * Time.deltaTime;
            if(timer >= ledgeForgivenessTime && jumpCounter < 1)
            {
                jumpCounter++;
            }

            if (airControl)
            {
                move.x = ((inputX * airControlFactor) * acceleration * inputModifyFactor);
                move.z = ((inputY * airControlFactor) * acceleration * inputModifyFactor);
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
            CalculateDrag();
            move.x = (inputX * acceleration * inputModifyFactor);
            move.z = (inputY * acceleration * inputModifyFactor);
            move.y = -0.75f;
            if (Input.GetButtonDown("Jump"))
            {
                move.y = jumpVelocity;
                jumpCounter++;
            }
            
        }

        CalculateVelocity();

        grounded = (characterController.Move(velocity * Time.deltaTime) & CollisionFlags.Below) !=0;
         //if we became or stayed grounded on this frame, reset the jump counter
        if (grounded)
        {
            jumpCounter = 0;
            timer = 0;
        }

    }


    void CalculateVelocity()
    {
        velocity.x += move.x * Time.deltaTime;
        velocity.y = move.y;
        velocity.z += move.z * Time.deltaTime;

        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        velocity.z = Mathf.Clamp(velocity.z, -maxSpeed, maxSpeed);
    }

    void CalculateDrag()
    {

        float drag = grounded ? stoppingSpeed : airStoppingSpeed;

        if (inputX == 0)
        {
            if (velocity.x > 0.1f)
            {
                velocity.x -= drag * Time.deltaTime;
            }
            else if (velocity.x < -0.1f)
            {
                velocity.x += drag * Time.deltaTime;
            }
            else
            {
                velocity.x = 0;
            }
        }

        if(inputY == 0)
        {
            if (velocity.z > 0.1f)
            {
                velocity.z -= drag * Time.deltaTime;
            }
            else if (velocity.z < -0.1f)
            {
                velocity.z += drag * Time.deltaTime;
            }
            else
            {
                velocity.z = 0;
            }
        }
    }

}
