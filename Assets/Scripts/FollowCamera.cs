using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public Transform target;
    private Transform targetHead;

    public float followDistance = 7f;
    public float followHeight = 5f;
    public float followSpeed = 10f;
    public float verticalFollowSpeed = 5f;
    public float rotateSensitivity = 10f;
    public float rotateDamping = 50f;

    // y Limit for default camera view
    public float yMinLimit = -80f;
    public float yMaxLimit = 80f;

    // limits for aiming camera view
    private float aimXMinLimit = -80f;
    private float aimXMaxLimit = 80f;
    private float aimYMinLimit = -80f;
    private float aimYMaxLimit = 80f;

    // lerp speed for moving camera into/out of aiming mode position
    private float aimPosSpeed = 5f;

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
    private float centeringAcceleration = .1f;
    private float centeringSpeed = 0f;

    [Header("Crosshair used when aiming")]
    public GameObject crosshair;


    public enum CameraState
    {
        idle,
        centering,
        aiming
    };

    public CameraState cameraState;

    [Header("Layer(s) to include")]
    public LayerMask camOcclusion;

    // Use this for initialization
    void Start()
    {
        newPos = new Vector3(target.position.x, target.position.y, target.position.z);
        camera = GetComponentInChildren<Camera>();

        targetHead = target.Find("Head");

        Vector3 angles = transform.eulerAngles;
        rotationYAxis = (rotationYAxis == 0) ? angles.y : rotationYAxis;
        rotationXAxis = angles.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cameraState != CameraState.centering)
        {
            if (Input.GetAxis("Aiming") > 0)
            {
                cameraState = CameraState.aiming;
            }

            else if (Input.GetButtonDown("Cam Center"))
            {
                cameraState = CameraState.centering;
                centeringSpeed = 0f;
            }

            else
            {
                cameraState = CameraState.idle;
            }
        }


        switch (cameraState)
        {
            case CameraState.centering:
                transform.forward = target.GetChild(0).forward;
                centeringSpeed += centeringAcceleration;
                rotationYAxis = Mathf.Lerp(rotationYAxis, transform.localRotation.eulerAngles.y, centeringSpeed);
                if (Mathf.Abs(rotationYAxis - transform.localRotation.eulerAngles.y) < 1)
                {
                    cameraState = CameraState.idle;
                }
                calculatePosition();
                calculateRotation();
                checkWalls();
                break;

            case CameraState.aiming:
                calculatePosition();

                velocityY += Input.GetAxis("Joy Y") * rotateSensitivity * .8f * Time.deltaTime;
                velocityX += Input.GetAxis("Joy X") * rotateSensitivity * .8f * Time.deltaTime;
                rotationYAxis += velocityX;
                rotationXAxis += velocityY;
                rotationXAxis = ClampAngle(rotationXAxis, aimXMinLimit, aimXMaxLimit);
                Quaternion rotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
                Quaternion pitch = Quaternion.Euler(0, rotationYAxis, 0);

                camera.transform.rotation = rotation;
                transform.localRotation = pitch;

                velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * rotateDamping);
                velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * rotateDamping);

                // How much the camera is angled above or below 0. If it's as high or as low as it can be the value will be 0. If it's not above or below at all it will be 1.
                float viewAnglePercentage = 1 - Mathf.Clamp(camera.transform.localEulerAngles.x > 180 ? 360 - camera.transform.localEulerAngles.x : camera.transform.localEulerAngles.x, aimXMinLimit, aimXMaxLimit) / aimXMaxLimit;

                // Over the shoulder position with an offset based on how high or low the camera is looking.
                Vector3 aimingPosition = targetHead.position - (transform.forward * 5f * viewAnglePercentage) + (targetHead.up * .65f * viewAnglePercentage);

                aimingPosition.x = Mathf.Lerp(camera.transform.position.x, aimingPosition.x, Time.deltaTime * aimPosSpeed);
                aimingPosition.y = Mathf.Lerp(camera.transform.position.y, aimingPosition.y, Time.deltaTime * aimPosSpeed);
                aimingPosition.z = Mathf.Lerp(camera.transform.position.z, aimingPosition.z, Time.deltaTime * aimPosSpeed);
                camera.transform.position = aimingPosition;

                checkWalls();
                break;

            case CameraState.idle:
                // update rotation of camera based on user input
                velocityY += Input.GetAxis("Joy Y") * rotateSensitivity * Time.deltaTime;
                velocityX += Input.GetAxis("Joy X") * rotateSensitivity * Time.deltaTime;
                calculatePosition();
                calculateRotation();
                checkWalls();
                break;

            default:
                calculatePosition();
                calculateRotation();
                checkWalls();
                break;
        }

        // shows crosshair if aiming
        crosshair.SetActive(cameraState == CameraState.aiming);

        Debug.Log(cameraState);
    }


    /// <summary>
    /// Calculates and sets position for transform
    /// </summary>
    private void calculatePosition()
    {
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Lerp(newPos.x, target.position.x, Time.deltaTime * followSpeed);
        newPos.y = Mathf.Lerp(newPos.y, target.position.y, Time.deltaTime * verticalFollowSpeed);
        newPos.z = Mathf.Lerp(newPos.z, target.position.z, Time.deltaTime * followSpeed);
        transform.position = newPos;
    }

    /// <summary>
    /// Calculates and sets rotation for transform
    /// </summary>
    private void calculateRotation()
    {
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


        velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * rotateDamping);
        velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * rotateDamping);
    }

    /// <summary>
    /// Checks if a wall is between where the camera moved and the player
    /// </summary>
    private void checkWalls()
    {
        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(target.position, camera.transform.position, out wallHit, camOcclusion))
        {
            Vector3 absPosition = new Vector3(wallHit.point.x + wallHit.normal.x, wallHit.point.y + wallHit.normal.y, wallHit.point.z + wallHit.normal.z);
            camera.transform.position = absPosition;
        }
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
