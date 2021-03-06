﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    private PlayerMotor playerMotor;
    private IManagedInput managedInput;
    public float rotationDamping = 7.5f;
    // Use this for initialization
    void Start()
    {
        playerMotor = transform.parent.GetComponent<PlayerMotor>();
        managedInput = transform.parent.GetComponent<IManagedInput>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = new Vector3(managedInput.GetAxisInput(PlayerInput.MOVE_X), 0, managedInput.GetAxisInput(PlayerInput.MOVE_Y));
        direction = playerMotor.cameraRig.TransformDirection(direction);
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            if (direction.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationDamping * Time.deltaTime);
            }
        }

    }
}
