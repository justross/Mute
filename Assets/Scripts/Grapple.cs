using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public Transform cameraRig;
    public Transform player;

    private GameObject grappleTarget;
    private FollowCamera cameraScript;
    private PlayerMotor playerMotorScript;
    private IManagedInput managedInput;

    // Use this for initialization
    void Start()
    {
        cameraScript = cameraRig.gameObject.GetComponent<FollowCamera>();
        playerMotorScript = player.gameObject.GetComponent<PlayerMotor>();
        managedInput = player.GetComponent<IManagedInput>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cameraScript.cameraState == FollowCamera.CameraState.aiming)
        {
            // Look for closest on-screen grapple target
            List<GameObject> grappleTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Grapple"));
            GameObject closestGrappleTarget = null;
            float closestDot = 0;
            foreach (GameObject grappleTarget in grappleTargets)
            {
                if (Physics.Raycast(Camera.main.transform.position, grappleTarget.transform.position - Camera.main.transform.position, 35f))
                {
                    Vector3 localPoint = Camera.main.transform.InverseTransformPoint(grappleTarget.transform.position).normalized;
                    float test = Vector3.Dot(localPoint, Vector3.forward);
                    // The closer test is to one the closer it is to the center of the screen
                    if (test > closestDot && test > 0.975f)
                    {
                        closestDot = test;
                        closestGrappleTarget = grappleTarget;
                    }
                }
            }
            grappleTarget = closestGrappleTarget;
        }
        else
        {
            grappleTarget = null;
        }

        if (managedInput.GetButtonInput("GrappleButton") && grappleTarget != null)
        {
            playerMotorScript.grappleTo(grappleTarget.transform);
        }
    }
}
