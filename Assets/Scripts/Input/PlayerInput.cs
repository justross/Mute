using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IManagedInput {

    public Command<bool> JumpDown;
    public Command<bool> JumpUp;
    public Command<bool> Aim;
    public Command<bool> Grapple;
    public Command<bool> CameraCenter;
    public Command<float> MoveX;
    public Command<float> MoveY;
    public Command<float> AimX;
    public Command<float> AimY;

    public KeyCode JumpKey;
    public KeyCode AimKey;
    public KeyCode GrappleKey;
    public KeyCode CenterKey;

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

    public const string JUMP_BUTTON_DOWN = "JumpDown";
    public const string JUMP_BUTTON_UP = "JumpUp";
    public const string AIM_BUTTON = "Aim";
    public const string GRAPPLE_BUTTON = "Grapple";
    public const string CAM_CENTER = "CameraCenter";
    public const string MOVE_X = "MoveX";
    public const string MOVE_Y = "MoveY";
    public const string AIM_X = "AimX";
    public const string AIM_Y = "AimY";

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
            case AIM_X:
                return AimX.State;
            case AIM_Y:
                return AimY.State;
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
                return JumpDown.State;
            case JUMP_BUTTON_UP:
                return JumpUp.State;
            case AIM_BUTTON:
                return Aim.State;
            case GRAPPLE_BUTTON:
                return Grapple.State;
            case CAM_CENTER:
                return CameraCenter.State;
            default:
                Debug.LogError("Input " + name + " not implemented.");
                return false;
        }
    }
    
    // Use this for initialization
    void Start () {
        JumpDown = new Command<bool>(JUMP_BUTTON_DOWN, () => { return Input.GetKeyDown(JumpKey); });
        JumpUp = new Command<bool>(JUMP_BUTTON_UP, () => { return Input.GetKeyUp(JumpKey);});
        Aim = new Command<bool>(AIM_BUTTON, () => { return Input.GetKey(AimKey);});
        Grapple = new Command<bool>(AIM_BUTTON, () => { return Input.GetKey(GrappleKey); });
        CameraCenter = new Command<bool>(CAM_CENTER, () => { return Input.GetKey(CenterKey); });
        MoveX = new Command<float>(MOVE_X, () => { return Input.GetAxisRaw(MoveXAxis);});
        MoveY = new Command<float>(MOVE_Y, () => { return Input.GetAxisRaw(MoveYAxis);});
        AimX = new Command<float>(AIM_X, () => { return Input.GetAxisRaw(AimXAxis); });
        AimY = new Command<float>(AIM_Y, () => { return Input.GetAxisRaw(AimYAxis); });
    }

}
