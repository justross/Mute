using UnityEngine;
using System.Collections;
using FSM;
using System;

[CreateAssetMenu(menuName = "FSM/Conditions/General/Multiple")]
public class MultiCondition : Condition
{
    public ConditionGroup condition;

    public override bool Decide(Controller controller)
    {
        return condition.Result(controller);
    }
}
