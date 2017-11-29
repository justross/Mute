using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerInput))]
public class PlayerInputEditor : Editor {

    string[] _axes;
    int _aimXIndex;
    int _aimYIndex;
    int _moveXIndex;
    int _moveYIndex;

    
    public override void OnInspectorGUI()
    {
        _axes = ReadInputManager.ReadAxes();
        var p_input = target as PlayerInput;
        DrawDefaultInspector();

        // set editor indexes based on what is set in the PlayerInput component
        if(p_input.AimXAxis != "")
            _aimXIndex = ArrayUtility.IndexOf(_axes, p_input.AimXAxis);
        if (p_input.AimYAxis != "")
            _aimYIndex = ArrayUtility.IndexOf(_axes, p_input.AimYAxis);
        if (p_input.MoveXAxis != "")
            _moveXIndex = ArrayUtility.IndexOf(_axes, p_input.MoveXAxis);
        if (p_input.MoveYAxis != "")
            _moveYIndex = ArrayUtility.IndexOf(_axes, p_input.MoveYAxis);

        // Draw our dropdowns
        _moveXIndex = EditorGUILayout.Popup("X Movement Axis", _moveXIndex, _axes);
        _moveYIndex = EditorGUILayout.Popup("Y Movement Axis", _moveYIndex, _axes);
        _aimXIndex = EditorGUILayout.Popup("X Aiming Axis", _aimXIndex, _axes);
        _aimYIndex = EditorGUILayout.Popup("Y Aiming Axis", _aimYIndex, _axes);

        // Update our PlayerInput component
        p_input.AimXAxis = _axes[_aimXIndex];
        p_input.AimYAxis = _axes[_aimYIndex];
        p_input.MoveXAxis = _axes[_moveXIndex];
        p_input.MoveYAxis = _axes[_moveYIndex];

        EditorUtility.SetDirty(target);
    }

}
