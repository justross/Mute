using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public Transform cameraRig;

    private GameObject grappleTarget;
    private FollowCamera cameraScript;

    // Use this for initialization
    void Start()
    {
        cameraScript = cameraRig.gameObject.GetComponent<FollowCamera>();
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
                    if (test > closestDot)
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

        if (Input.GetAxis("Grappling") > 0 && grappleTarget != null)
        {
            Debug.Log("grappling my dude");
        }
    }
}
