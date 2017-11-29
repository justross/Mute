using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput {

    bool GetButtonInput(string name);
    float GetAxisInput(string name);
}
