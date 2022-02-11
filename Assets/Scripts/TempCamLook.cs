using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempCamLook
{
    public float XMouseSensitivity = 2f;
    public float YMouseSensitivity = 2f;
    public float ClampMinimumX = -90f;   
    public float ClampMaximumX = 90f;
    public float smoothTime = 5f;

    private Quaternion playerTargetRot;
    private Quaternion camTargetRot;

    public void InitSettings(Transform player, Transform cam)
    {
        playerTargetRot = player.localRotation;
        camTargetRot    = cam.localRotation;
    }
    
    public void CameraLookRotation(Vector2 rotValue, Transform player, Transform cam) 
    {
        rotValue.y = Input.GetAxis("Mouse X") * XMouseSensitivity;
        rotValue.x = Input.GetAxis("Mouse Y") * YMouseSensitivity;

        playerTargetRot *= Quaternion.Euler(
            0f, 
            rotValue.y,
            0f);
        camTargetRot    *= Quaternion.Euler(
            -rotValue.x, 
            0f, 
            0f);

        camTargetRot = ClampCamera(camTargetRot);

        player.localRotation = Quaternion.Slerp
        (
            player.localRotation, 
            cam.localRotation, 
            smoothTime * Time.deltaTime
        );

        cam.localRotation = Quaternion.Slerp
        (
            cam.localRotation, 
            player.localRotation, 
            smoothTime * Time.deltaTime
        );

        Cursor.lockState = CursorLockMode.Locked;
    }

    Quaternion ClampCamera(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float xAngle = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        xAngle = Mathf.Clamp(xAngle, ClampMinimumX, ClampMaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Rad2Deg * xAngle);

        return q;
    }
}