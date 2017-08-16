using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public Transform target;
    public float followDistance = 7f;
    public float followHeight = 5f;
    public float moveSpeed = 10f;
    public float rotateSpeed = 10f;
    public float camRotSpeed = 8f;

    private Vector3 newPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        newPos.x = target.position.x;
        newPos.y = target.position.y + followHeight;
        newPos.z = target.position.z - followDistance;

        //transform.position = Vector3.Lerp(transform.position, newPos, moveSpeed * Time.deltaTime);
        
        Vector2 rightStickInput = new Vector2(Input.GetAxis("Joy X"), Input.GetAxis("Joy Y"));

        transform.LookAt(target);
        transform.Translate(rightStickInput * Time.deltaTime * camRotSpeed);

    }
}
