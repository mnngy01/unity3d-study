using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RailController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 180f;
    [SerializeField] private float snapAngle = 2f;

    // 레일 회전
    public void RotateRail(float direction)
    {
        transform.Rotate(
            0f,
            0f,
            -rotateSpeed * Time.deltaTime,
            Space.Self
        );
    }

    public void SnapRotation()
    {
        Vector3 angle = transform.localEulerAngles;

        angle.z = Mathf.Round(angle.z / snapAngle) * snapAngle;

        transform.localEulerAngles = angle;
    }
}
