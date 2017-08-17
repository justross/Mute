using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public Transform target;
    public float followDistance = 7f;
    public float followHeight = 5f;
    public float moveSpeed = 10f;
    public float rotateSensitivity = 10f;
    public float rotateDamping = 50f;
    public float maxRotate = 60;
    private Vector3 offset;
    private Camera cam;
    private Vector3 newPos = Vector3.zero;
    private Vector3 newRot;
    // Use this for initialization
    void Start () {
        newPos = new Vector3(target.position.x, target.position.y, target.position.z);
        newRot = transform.localRotation.eulerAngles;
        offset = new Vector3(0 , followHeight, -followDistance);
        cam = GetComponentInChildren<Camera>();
        cam.transform.localPosition = offset;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        newRot.x += Input.GetAxis("Joy Y") * rotateSensitivity;
        newRot.y += Input.GetAxis("Joy X") * rotateSensitivity;
        newRot.z = 0;
        newRot.x = Mathf.Clamp(newRot.x, -maxRotate, maxRotate);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(newRot), Time.deltaTime * rotateDamping);
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * moveSpeed);

    }
}
