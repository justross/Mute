using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public Transform target;
    public float followDistance = 7f;
    public float followHeight = 5f;
    public float moveSpeed = 10f;
    public float rotateSpeed = 10f;

    private Vector3 newPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        newPos.x = target.position.x;
        newPos.y = target.position.y + followHeight;
        newPos.z = target.position.z - followDistance;

        transform.position = Vector3.Lerp(transform.position, newPos, moveSpeed * Time.deltaTime);
	}
}
