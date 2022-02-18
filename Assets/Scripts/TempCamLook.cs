using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempCamLook
{
    public float XMouseSensitivity = 0.075f;
    public float YMouseSensitivity = 0.075f;
    public float ClampMinimumX = -60F;   
    public float ClampMaximumX = 60F;
    public float smoothTime = 2.0f;

    private Quaternion playerTargetRot;
    //private Quaternion playerHeadTargetRot;
    private Quaternion camTargetRot;

    public void InitSettings(Transform player,/* Transform playerHead,*/ Transform cam)
    {
        //playerHeadTargetRot = playerHead.transform.localRotation;
        playerTargetRot = player.localRotation;
        camTargetRot    = cam.localRotation;
    }
    
    public void CameraLookRotation(Vector2 rotValue, Transform player,/* Transform playerHead,*/ Transform cam) 
    {
        //rotValue.y = Input.GetAxis("Mouse X") * XMouseSensitivity;
        //rotValue.x = Input.GetAxis("Mouse Y") * YMouseSensitivity;

        float yRotation = rotValue.x * XMouseSensitivity;
        float xRotation = rotValue.y * YMouseSensitivity;

        playerTargetRot *= Quaternion.Euler( 0f, yRotation, 0f);
        camTargetRot    *= Quaternion.Euler(-xRotation, 0f, 0f);

        camTargetRot = ClampCamera(camTargetRot);

        player.localRotation = Quaternion.Slerp (player.localRotation, playerTargetRot, 
            smoothTime /** Time.fixedDeltaTime*/);
        cam.localRotation = Quaternion.Slerp (cam.localRotation, camTargetRot, 
            smoothTime /** Time.fixedDeltaTime*/);

        //playerHead.transform.rotation = cam.transform.rotation;
        //Vector3 localHeadRotation = playerHeadTargetRot.eulerAngles/*playerHead.transform.localRotation.eulerAngles*/;
        //localHeadRotation.y = Mathf.Clamp(localHeadRotation.y, -45f, 45f);
        //localHeadRotation.x = Mathf.Clamp(localHeadRotation.x, -30f, 30f);
        //playerHeadTargetRot = Quaternion.Euler(localHeadRotation);

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