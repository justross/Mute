using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorPhysics : MonoBehaviour {

    public bool applyGravity = true;
    public float gravity = 20f;
    public float friction = 15f;
    public float maxVelocity = 30f;
    private Vector3 velocity = Vector3.zero;

    public void AddForce(Vector3 vector)
    {
        velocity += vector;

        if (velocity.x > maxVelocity)
        {
            velocity.x = maxVelocity;
        }
        else if (velocity.x < -maxVelocity)
        {
            velocity.x = -maxVelocity;
        }

        if (velocity.z > maxVelocity)
        {
            velocity.z = maxVelocity;
        }
        else if (velocity.z < -maxVelocity)
        {
            velocity.z = -maxVelocity;
        }
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void UpdatePhysics()
    {
        CalcVelocity(velocity);
    }

    void CalcVelocity(Vector3 velocityLastFrame)
    {
        velocity = velocityLastFrame;
        velocity.y -= gravity * Time.fixedDeltaTime;

        // Calculate X drag
        if (velocity.x > 0)
        {
            if (velocity.x - friction >= 0)
            {
                velocity.x -= friction * Time.fixedDeltaTime;
            }
            else
            {
                velocity.x = 0;
            }
        }
        else if(velocity.x < 0)
        {
            if (velocity.x + friction <= 0)
            {
                velocity.x += friction * Time.fixedDeltaTime;
            }
            else
            {
                velocity.x = 0;
            }
        }

        // Calculate Z drag
        if (velocity.z > 0)
        {
            if (velocity.z - friction >= 0)
            {
                velocity.z -= friction * Time.fixedDeltaTime;
            }
            else
            {
                velocity.z = 0;
            }
        }
        else if (velocity.z < 0)
        {
            if (velocity.z + friction <= 0)
            {
                velocity.z += friction * Time.fixedDeltaTime;
            }
            else
            {
                velocity.z = 0;
            }
        }

    }

}
