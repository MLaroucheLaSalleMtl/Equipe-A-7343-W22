using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float mouseMouvement = 100f;
    float rotationX;
    [SerializeField] bool lockCursor = true;
    // Start is called before the first frame update
    void Start()
    {
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseMouvement * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseMouvement * Time.deltaTime;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
         player.Rotate(Vector3.up * mouseX);

        
    }
}
