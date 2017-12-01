using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

[System.Serializable]
public class VelocityCheck {

    public enum Axis
    {
        X,
        Y,
        Z
    }

    public bool isLessThan = false;
    public bool orEqualTo = false;
    public float compareAgainst;
    public Axis axis;

    public bool Result(Controller controller)
    {
        ActorPhysics physics = controller.GetComponent<ActorPhysics>();
        if (physics != null)
        {
            float value = 0;
            Vector3 velocity = physics.GetVelocity();
            switch (axis)
            {
                case Axis.X:
                    value = velocity.x;
                    return CompareAxis(value, isLessThan, orEqualTo, compareAgainst);
                case Axis.Y:
                    value = velocity.y;
                    return CompareAxis(value, isLessThan, orEqualTo, compareAgainst);
                case Axis.Z:
                    value = velocity.z;
                    return CompareAxis(value, isLessThan, orEqualTo, compareAgainst);
            }            
        }

        Debug.Log("No physics component found");
        return false;

    }

    private bool CompareAxis(float a, bool lessThan, bool equalTo, float b )
    {
        if (lessThan)
        {
            if (equalTo)
            {
                return a <= b;
            }
            else
            {
                return a < b;
            }
        }
        else
        {
            if (equalTo)
            {
                return a >= b;
            }
            else
            {
                return a > b;
            }
        }
    }
}