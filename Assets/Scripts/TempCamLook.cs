using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempCamLook
{
    public float XMouseSensitivity = 0.1f;
    public float YMouseSensitivity = 0.1f;

    public float XAimMouseSensitivity = 0.05f;
    public float YAimMouseSensitivity = 0.05f;

    public float ClampMinimumX = -80F;   
    public float ClampMaximumX = 80F;
    public float smoothTime = 13f;

    private Quaternion playerTargetRot;
    private Quaternion camTargetRot;

    public void InitSettings(Transform player, Transform cam)
    {        
        playerTargetRot = player.localRotation;
        camTargetRot    = cam.localRotation;
    }   

    public void CameraLookRotation(Vector2 rotValue, Transform player, Transform cam, RigidBodyFPSController m_FPSController) 
    {
        float yRotation;
        float xRotation;

        if (m_FPSController.IsAiming)
        {
            yRotation = rotValue.x * (XMouseSensitivity * 0.5f);
            xRotation = rotValue.y * (YMouseSensitivity * 0.5f);
        }
        else
        {
            yRotation = rotValue.x * XMouseSensitivity;
            xRotation = rotValue.y * YMouseSensitivity;
        }

        playerTargetRot *= Quaternion.Euler( 0f, yRotation, 0f);
        camTargetRot    *= Quaternion.Euler(-xRotation, 0f, 0f);

        camTargetRot = ClampCamera(camTargetRot);

        player.localRotation = Quaternion.Slerp (player.localRotation, playerTargetRot, 
            smoothTime * Time.fixedDeltaTime);
        cam.localRotation = Quaternion.Slerp (cam.localRotation, camTargetRot, 
            smoothTime * Time.fixedDeltaTime);
       
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

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * xAngle);

        return q;
    }
}