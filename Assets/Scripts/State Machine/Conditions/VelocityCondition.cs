using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;


[CreateAssetMenu(menuName = "FSM/Conditions/Actors/Velocity")]
public class VelocityCondition : Condition {

    public VelocityCheck[] checks;

    public override bool Decide(Controller controller)
    {
        for(int i = 0; i < checks.Length; i++)
        {
            checks[i].Result(controller);
        }

        return false;
    }

}
