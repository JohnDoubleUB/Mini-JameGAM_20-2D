using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public bool FollowEnabled = true;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        if (Target == null) return;
        if (FollowEnabled == false) return;

        //Where we want the camera
        Vector3 desiredPosition = new Vector3(Target.position.x, Target.position.y, transform.position.z);

        //The Linearly interpolated movement from current to desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition + offset, smoothSpeed * Time.unscaledDeltaTime);
        transform.position = smoothedPosition;
    }
}
