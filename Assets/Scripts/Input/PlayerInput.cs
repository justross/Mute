using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IManagedInput {

    public Command<bool> JumpButtonDown;
    public Command<bool> JumpButtonUp;
    public Command<bool> AimButton;
    public Command<bool> GrappleButton;
    public Command<float> MoveX;
    public Command<float> MoveY;

    public KeyCode JumpKey;
    public KeyCode AimKey;
    public KeyCode GrappleKey;

    [SerializeField]
    [HideInInspector]
    private string moveXAxis;
    [SerializeField]
    [HideInInspector]
    private string moveYAxis;
    [SerializeField]
    [HideInInspector]
    private string aimXAxis;
    [SerializeField]
    [HideInInspector]
    private string aimYAxis;

    const string JUMP_BUTTON_DOWN = "JumpButtonDown";
    const string JUMP_BUTTON_UP = "JumpButtonUp";
    const string AIM_BUTTON = "AimButton";
    const string GRAPPLE_BUTTON = "GrappleButton";
    const string MOVE_X = "MoveX";
    const string MOVE_Y = "MoveY";

    public string MoveXAxis
    {
        get
        {
            return moveXAxis;
        }

        set
        {
            moveXAxis = value;
        }
    }

    public string MoveYAxis
    {
        get
        {
            return moveYAxis;
        }

        set
        {
            moveYAxis = value;
        }
    }

    public string AimXAxis
    {
        get
        {
            return aimXAxis;
        }

        set
        {
            aimXAxis = value;
        }
    }

    public string AimYAxis
    {
        get
        {
            return aimYAxis;
        }

        set
        {
            aimYAxis = value;
        }
    }

    public float GetAxisInput(string name)
    {

        switch (name)
        {
            case MOVE_X:
                return MoveX.State;
            case MOVE_Y:
                return MoveY.State;
            default:
                Debug.Log("Input type not implemented.");
                return 0;

        }
    }

    public bool GetButtonInput(string name)
    {
        switch (name)
        {
            case JUMP_BUTTON_DOWN:
                return JumpButtonDown.State;
            case JUMP_BUTTON_UP:
                return JumpButtonUp.State;
            case AIM_BUTTON:
                return AimButton.State;
            case GRAPPLE_BUTTON:
                return GrappleButton.State;
            default:
                Debug.LogError("Input type not implemented.");
                return false;
        }
    }


    // Use this for initialization
    void Start () {
        JumpButtonDown = new Command<bool>(JUMP_BUTTON_DOWN, () => { return Input.GetKeyDown(JumpKey); });
        JumpButtonUp = new Command<bool>(JUMP_BUTTON_UP, () => { return Input.GetKeyUp(JumpKey);});
        AimButton = new Command<bool>(AIM_BUTTON, () => { return Input.GetKey(AimKey) || Input.GetAxisRaw("Aiming") <= 0; });
        GrappleButton = new Command<bool>(AIM_BUTTON, () => { return Input.GetKey(GrappleKey) || Input.GetAxisRaw("Grapple") > 0; });
        MoveX = new Command<float>(MOVE_X, () => { return Input.GetAxisRaw(MoveXAxis);});
        MoveY = new Command<float>(MOVE_Y, () => { return Input.GetAxisRaw(MoveYAxis);});
    }



}
