using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public Transform target;
    
    public float followDistance = 7f;
    public float followHeight = 5f;
    public float followSpeed = 10f;
    public float verticalFollowSpeed = 5f;
    public float rotateSensitivity = 10f;
    public float rotateDamping = 50f;
    public float yMinLimit = -80f;
    public float yMaxLimit = 80f;

    private Vector3 offset;
    private new Camera camera;
    private Vector3 newPos = Vector3.zero;
    private float newPitch;
    private float newYaw;
    float velocityX = 0.0f;
    float velocityY = 0.0f;
    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;
    
    // Use this for initialization
    void Start () {
        newPos = new Vector3(target.position.x, target.position.y, target.position.z);
        camera = GetComponentInChildren<Camera>();

        Vector3 angles = transform.eulerAngles;
        rotationYAxis = (rotationYAxis == 0) ? angles.y : rotationYAxis;
        rotationXAxis = angles.x;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        // update position of camera rig
        newPos = transform.position;
        newPos.x = Mathf.Lerp(newPos.x, target.position.x, Time.deltaTime * followSpeed);
        newPos.y = Mathf.Lerp(newPos.y, target.position.y, Time.deltaTime * verticalFollowSpeed);
        newPos.z = Mathf.Lerp(newPos.z, target.position.z, Time.deltaTime * followSpeed);
        transform.position = newPos;

        // update rotation of camera
        newYaw = Input.GetAxis("Joy Y") * rotateSensitivity * Time.deltaTime;
        newPitch = Input.GetAxis("Joy X") * rotateSensitivity * Time.deltaTime;
        velocityY += newYaw;
        velocityX += newPitch;
        rotationYAxis += velocityX;
        rotationXAxis += velocityY;
        rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
        Quaternion toRotation = Quaternion.Euler(rotationXAxis, 0, 0);
        Quaternion rotation = toRotation;

        Quaternion toPitch = Quaternion.Euler(0, rotationYAxis, 0);
        Quaternion pitch = toPitch;

        Vector3 negDistance = new Vector3(0.0f, followHeight , -followDistance);
        Vector3 position = rotation * negDistance;
        camera.transform.localRotation = rotation;
        camera.transform.localPosition = position;
        transform.localRotation = pitch;


        velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * rotateDamping);
        velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * rotateDamping);

    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
