using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateAngle : MonoBehaviour
{
    public float CalculateForwardAngle()
    {
        Vector3 forward = transform.forward;

        Vector3 surfaceNormal = Vector3.up;

        float angle = Vector3.Angle(forward, surfaceNormal);

        return angle;
    }

    void Update()
    {
        float angle = CalculateForwardAngle();
        Debug.Log("Forward Angle: " + angle);
    }
}
