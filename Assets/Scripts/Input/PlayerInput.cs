using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IPlayerInput {

    public Command<bool> JumpButtonDown;
    public Command<bool> JumpButtonUp;
    public Command<float> MoveX;
    public Command<float> MoveY;

    const string JUMP_BUTTON_DOWN = "JumpButtonDown";
    const string JUMP_BUTTON_UP = "JumpButtonUp";

    const string MOVE_X = "MoveX";
    const string MOVE_Y = "MoveY";

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
            default:
                Debug.Log("Input type not implemented.");
                return false;
        }
    }


    // Use this for initialization
    void Start () {
        JumpButtonDown = new Command<bool>(JUMP_BUTTON_DOWN, false, () => { return Input.GetButtonDown("Jump");});
        JumpButtonUp = new Command<bool>(JUMP_BUTTON_UP, false, () => { return Input.GetButtonUp("Jump");});
        MoveX = new Command<float>(MOVE_X, 0.0f, () => { return Input.GetAxisRaw("Horizontal");});
        MoveY = new Command<float>(MOVE_Y, 0.0f, () => { return Input.GetAxisRaw("Vertical");});
    }



}
