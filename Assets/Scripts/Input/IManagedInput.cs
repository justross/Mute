using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManagedInput {

    bool GetButtonInput(string name);
    float GetAxisInput(string name);
}
