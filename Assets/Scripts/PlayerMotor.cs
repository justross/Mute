using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

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

    private Vector3 velocity;

    private bool grounded = false;
    private Vector3 move = Vector3.zero;
    private float inputX = 0;
    private float inputY = 0;
    private CharacterController characterController = null;
    int jumpCounter = 0;
    float gravity = 0;
    float jumpVelocity = 0;
    float velocityJumpTermination = 0;
    public Transform cameraRig;
    private FollowCamera followCamera;


    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        followCamera = cameraRig.gameObject.GetComponent<FollowCamera>();
    }


    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(inputX, inputY);

        //Calculate our physics constants for this frame
        gravity = (2 * maxJumpHeight) / Mathf.Pow(maxJumpTime, 2);
        jumpVelocity = Mathf.Sqrt(2 * gravity * maxJumpHeight);

        //Calculate the downward velocity needed to exit a jump early. 
        velocityJumpTermination = Mathf.Sqrt(Mathf.Pow(jumpVelocity, 2) + (2 * -gravity) * (maxJumpHeight - minJumpHeight));

        if (!grounded)
        {

            CalculateDrag();

            timer += 1000 * Time.deltaTime;
            move.y -= gravity * Time.deltaTime;
            if (timer >= ledgeForgivenessTime && jumpCounter < 1)
            {
                jumpCounter++;
            }

            if (airControl)
            {
                move.x = ((inputX * airControlFactor) * acceleration);
                move.z = ((inputY * airControlFactor) * acceleration);
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
            move.x = (inputX * acceleration);
            move.z = (inputY * acceleration);
            move.y = -0.75f;
            if (Input.GetButtonDown("Jump"))
            {
                move.y = jumpVelocity;
                jumpCounter++;
            }

        }

        CalculateVelocity();
        Vector3 movement = new Vector3(velocity.x, 0, velocity.z);
        movement = cameraRig.TransformDirection(movement);
        movement.y = velocity.y;
        if (followCamera.cameraState == FollowCamera.CameraState.aiming)
        {
            transform.GetChild(0).forward = cameraRig.GetComponentInChildren<Camera>().transform.forward;
        }
        grounded = (characterController.Move(movement * Time.deltaTime) & CollisionFlags.Below) != 0;
        //if we became or stayed grounded on this frame, reset the jump counter
        if (grounded)
        {
            jumpCounter = 0;
            timer = 0;
        }

    }

    private Vector3 AlignToVector(Vector3 from, Vector3 to)
    {
        Vector3 result = Vector3.zero;



        return result;
    }

    // gets the world-relative velocity of the player;
    public Vector3 GetVelocity()
    {
        return velocity;
    }

    // gets the world-relative velocity of the player;
    public Vector3 SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
        return velocity;
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
            if (velocity.x > 0.25f)
            {
                velocity.x -= drag * Time.deltaTime;
            }
            else if (velocity.x < -0.25f)
            {
                velocity.x += drag * Time.deltaTime;
            }
            else
            {
                velocity.x = 0;
            }
        }

        if (inputY == 0)
        {
            if (velocity.z > 0.25f)
            {
                velocity.z -= drag * Time.deltaTime;
            }
            else if (velocity.z < -0.25f)
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
