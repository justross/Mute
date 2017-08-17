using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public Transform target;
    public float followDistance = 7f;
    public float followHeight = 5f;
    public float moveSpeed = 10f;
    public float verticalMoveSpeed = 5f;
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
        // update rotation of camera rig
        newRot.x += Input.GetAxis("Joy Y") * rotateSensitivity;
        newRot.y += Input.GetAxis("Joy X") * rotateSensitivity;
        newRot.z = 0;
        newRot.x = Mathf.Clamp(newRot.x, -maxRotate, maxRotate);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(newRot), Time.deltaTime * rotateDamping);

        // update rotation of camera
        Vector3 camRot = cam.transform.rotation.eulerAngles;
        Quaternion newCamRot = Quaternion.LookRotation(target.position - cam.transform.position, transform.up);
        camRot.x = Mathf.Lerp(camRot.x, newCamRot.eulerAngles.x, rotateDamping * Time.deltaTime);
        cam.transform.rotation = Quaternion.Euler(camRot);
        
        //update position of camera rig
        newPos = transform.position;
        newPos.x = Mathf.Lerp(newPos.x, target.position.x, Time.deltaTime * moveSpeed);
        newPos.y = Mathf.Lerp(newPos.y, target.position.y, Time.deltaTime * verticalMoveSpeed);
        newPos.z = Mathf.Lerp(newPos.z, target.position.z, Time.deltaTime * moveSpeed);
        transform.position = newPos;

    }
}
