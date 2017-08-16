using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public Transform target;
    public float followDistance = 7f;
    public float followHeight = 5f;
    public float moveSpeed = 10f;
    public float rotateSpeed = 10f;
    public float camRotSpeed = 5f;
    private Vector3 offset;

    private Vector3 newPos = Vector3.zero;

	// Use this for initialization
	void Start () {
        newPos = new Vector3(target.position.x, target.position.y + followHeight, target.position.z - followDistance);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        newPos = Quaternion.AngleAxis(Input.GetAxis("Joy X") * camRotSpeed, Vector3.up) * newPos;
        newPos = Quaternion.AngleAxis(Input.GetAxis("Joy Y") * camRotSpeed, Vector3.left) * newPos;

        transform.position = target.position + newPos;

        transform.LookAt(target.position);
    }
}
