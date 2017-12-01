using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

[CreateAssetMenu(menuName = "FSM/Conditions/Player/Input")]
public class InputCondition : Condition {

    public CustomInput[] inputs;


    public override bool Decide(Controller controller)
    {
        IManagedInput managedInput = controller.GetComponent<IManagedInput>();

        for(int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i].CheckInput(managedInput))
            {
                return true;
            }
        }

        return false;
    }
}
