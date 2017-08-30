using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public Transform target;

    public float followDistance = 7f;
    public float followHeight = 5f;
    public float followSpeed = 10f;
    public float centeringSpeed = 10f;
    public float verticalFollowSpeed = 5f;
    public float rotateSensitivity = 10f;
    public float rotateDamping = 50f;
    public float yMinLimit = -80f;
    public float yMaxLimit = 80f;

    private Vector3 offset;
    private new Camera camera;
    private Vector3 camMask;
    private Vector3 newPos = Vector3.zero;
    private float newPitch;
    private float newYaw;
    float velocityX = 0.0f;
    float velocityY = 0.0f;
    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;


    public enum CameraState
    {
        idle,
        centering
    };

    public CameraState cameraState;

    [Header("Layer(s) to include")]
    public LayerMask camOcclusion;

    // Use this for initialization
    void Start()
    {
        newPos = new Vector3(target.position.x, target.position.y, target.position.z);
        camera = GetComponentInChildren<Camera>();

        Vector3 angles = transform.eulerAngles;
        rotationYAxis = (rotationYAxis == 0) ? angles.y : rotationYAxis;
        rotationXAxis = angles.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetButton("Cam Center"))
        {
            cameraState = CameraState.centering;
        }

        switch (cameraState)
        {
            case CameraState.centering:
                transform.forward = target.GetChild(0).forward;
                rotationXAxis = camera.transform.localRotation.eulerAngles.x;
                rotationYAxis = transform.localRotation.eulerAngles.y;
                cameraState = CameraState.idle;
                break;

            case CameraState.idle:
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

                break;
        }

        rotationYAxis += velocityX;
        rotationXAxis += velocityY;
        rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
        Quaternion rotation = Quaternion.Euler(rotationXAxis, 0, 0);
        Quaternion pitch = Quaternion.Euler(0, rotationYAxis, 0);
        Vector3 negDistance = new Vector3(0.0f, followHeight, -followDistance);
        Vector3 position = rotation * negDistance;

        camera.transform.localRotation = rotation;
        camera.transform.localPosition = position;
        transform.localRotation = pitch;

        // checks if a wall is between where the camera moved and the player
        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(target.position, camera.transform.position, out wallHit, camOcclusion))
        {
            Vector3 absPosition = new Vector3(wallHit.point.x + wallHit.normal.x, wallHit.point.y + wallHit.normal.y, wallHit.point.z + wallHit.normal.z);
            camera.transform.position = absPosition;
        }
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
