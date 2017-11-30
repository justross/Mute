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

    /// <summary
    /// Time in ms the player has when falling that they can still do a full double jump.
    /// </summary>
    public int ledgeForgivenessTime = 120;

    private Vector3 velocity;

    public enum MoveState
    {
        idle,
        jumping,
        grappling
    }

    public MoveState moveState;

    private bool grounded = false;
    private Vector3 move = Vector3.zero;
    private float inputX = 0;
    private float inputY = 0;
    private float heldXInput = 0;
    private float heldYInput = 0;
    private CharacterController characterController = null;
    int jumpCounter = 0;
    float gravity = 0;
    float jumpVelocity = 0;
    float velocityJumpTermination = 0;
    float timer = 0;
    public Transform cameraRig;
    private FollowCamera followCamera;
    private Transform grappleTarget;
    private Vector3 movement = Vector3.zero;
    private Vector3 input = Vector3.zero;
    private IManagedInput managedInput;

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        followCamera = cameraRig.gameObject.GetComponent<FollowCamera>();
        managedInput = GetComponent<IManagedInput>();
    }


    // Update is called once per frame
    void Update()
    {
        if (managedInput.GetButtonInput(PlayerInput.JUMP_BUTTON_DOWN) && grounded)
        {
            moveState = MoveState.jumping;
            move.y = jumpVelocity;
        }

        switch (moveState)
        {
            case MoveState.grappling:
                if (!managedInput.GetButtonInput(PlayerInput.AIM_BUTTON))
                {
                    moveState = MoveState.idle;
                }
                else
                {
                    characterController.Move(grappleTarget.position - transform.position);
                }
                break;

            case MoveState.jumping:
                inputX = managedInput.GetAxisInput(PlayerInput.MOVE_X);
                inputY = managedInput.GetAxisInput(PlayerInput.MOVE_X);

                input = new Vector3(inputX, 0f, inputY);
                if (input.magnitude > 1f)
                {
                    input /= input.magnitude;
                }

                if (airControl)
                {
                    input.x *= airControlFactor;
                    input.z *= airControlFactor;
                }

                move.x = input.x;
                move.z = input.z;

                //Calculate our physics constants for this frame
                gravity = (2 * maxJumpHeight) / Mathf.Pow(maxJumpTime, 2);
                jumpVelocity = Mathf.Sqrt(2 * gravity * maxJumpHeight);

                //Calculate the downward velocity needed to exit a jump early. 
                velocityJumpTermination = Mathf.Sqrt(Mathf.Pow(jumpVelocity, 2) + (2 * -gravity) * (maxJumpHeight - minJumpHeight));
                                
                timer += 1000 * Time.deltaTime;
                move.y -= gravity * Time.deltaTime;
                if (timer >= ledgeForgivenessTime && jumpCounter < 1)
                {
                    jumpCounter++;
                }

                if (managedInput.GetButtonInput(PlayerInput.JUMP_BUTTON_UP) && move.y > 0)
                {
                    //choose the minimum between the exit velocity and current upward velocity
                    move.y = Mathf.Min(velocityJumpTermination, move.y);
                }

                if (managedInput.GetButtonInput(PlayerInput.JUMP_BUTTON_DOWN) && jumpCounter < jumpCount)
                {
                    move.y = jumpVelocity;
                    jumpCounter++;
                }

                CalculateDrag();
                CalculateVelocity();
                movement = new Vector3(velocity.x, 0, velocity.z);
                movement = cameraRig.TransformDirection(movement);

                movement.y = velocity.y;
                grounded = (characterController.Move(movement * Time.deltaTime) & CollisionFlags.Below) != 0;
                //if we became or stayed grounded on this frame, reset the jump counter
                if (grounded)
                {
                    jumpCounter = 0;
                    timer = 0;
                    moveState = MoveState.idle;
                }
                break;

            default:
                inputX = managedInput.GetAxisInput(PlayerInput.MOVE_X);
                inputY = managedInput.GetAxisInput(PlayerInput.MOVE_Y);

                input = new Vector3(inputX, 0f, inputY);
                if (input.magnitude > 1f)
                {
                    input /= input.magnitude;
                }
                move.x = input.x;
                move.z = input.z;

                //Calculate our physics constants for this frame
                gravity = (2 * maxJumpHeight) / Mathf.Pow(maxJumpTime, 2);
                jumpVelocity = Mathf.Sqrt(2 * gravity * maxJumpHeight);

                //Calculate the downward velocity needed to exit a jump early. 
                velocityJumpTermination = Mathf.Sqrt(Mathf.Pow(jumpVelocity, 2) + (2 * -gravity) * (maxJumpHeight - minJumpHeight));

                CalculateDrag();


                if (!grounded)
                {
                    timer += 1000 * Time.deltaTime;
                    move.y -= gravity * Time.deltaTime;
                    if (timer >= ledgeForgivenessTime && jumpCounter < 1)
                    {
                        jumpCounter++;
                    }

                    if (airControl)
                    {
                        move.x *= airControlFactor * acceleration;
                        move.z *= airControlFactor * acceleration;
                    }

                    if (managedInput.GetButtonInput(PlayerInput.JUMP_BUTTON_UP) && move.y > 0)
                    {
                        //choose the minimum between the exit velocity and current upward velocity
                        move.y = Mathf.Min(velocityJumpTermination, move.y);
                    }

                    if (managedInput.GetButtonInput(PlayerInput.JUMP_BUTTON_DOWN) && jumpCounter < jumpCount)
                    {

                        move.y = jumpVelocity;
                        jumpCounter++;
                    }

                }
                else
                {
                    move.x *= acceleration;
                    move.z *= acceleration;
                    move.y = -0.75f;
                    if (managedInput.GetButtonInput(PlayerInput.JUMP_BUTTON_DOWN))
                    {
                        move.y = jumpVelocity;
                        jumpCounter++;
                    }

                }

                CalculateVelocity();
                movement = new Vector3(velocity.x, 0, velocity.z);
                movement = cameraRig.TransformDirection(movement);

                movement.y = velocity.y;
                grounded = (characterController.Move(movement * Time.deltaTime) & CollisionFlags.Below) != 0;
                //if we became or stayed grounded on this frame, reset the jump counter
                if (grounded)
                {
                    jumpCounter = 0;
                    timer = 0;
                }
                break;
        }
    }

    void LateUpdate()
    {
        if (followCamera.cameraState == FollowCamera.CameraState.aiming)
        {
            transform.GetChild(0).forward = cameraRig.GetComponentInChildren<Camera>().transform.forward;

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

        Vector2 v2 = new Vector2(velocity.x, velocity.z);
        v2 = Vector2.ClampMagnitude(v2, maxSpeed);
        velocity.x = v2.x;
        velocity.z = v2.y;
    }

    void CalculateDrag()
    {

        float drag = grounded ? stoppingSpeed : airStoppingSpeed;

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

    /// <summary>
    /// Sets player's grappling target to target and sets move state to grappling.
    /// </summary>
    /// <param name="target"></param>
    public void grappleTo(Transform target)
    {
        grappleTarget = target;
        moveState = MoveState.grappling;
    }

}
