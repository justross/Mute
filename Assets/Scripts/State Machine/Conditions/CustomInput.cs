using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomInput {

    public string name;
    public InputType inputType;

    [Header("Axis Input options")]
    public bool isInclusive = false;
    [Range(-1f,0f)]
    public float leftValue;
    [Range(0f, 1f)]
    public float rightValue;

    public enum InputType{
        Axis,
        Button
    }

    public bool CheckInput(IManagedInput managedInput)
    {
        switch (inputType)
        {
            case InputType.Axis:
                if (isInclusive)
                {
                    return (managedInput.GetAxisInput(name) > leftValue || managedInput.GetAxisInput(name) < rightValue);
                }
                else
                {
                    return (managedInput.GetAxisInput(name) > leftValue || managedInput.GetAxisInput(name) < rightValue);
                }
                
            case InputType.Button:
                return managedInput.GetButtonInput(name);
        }

        return false;
    }
    
}
