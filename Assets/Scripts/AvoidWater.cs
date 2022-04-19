using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidWater : MonoBehaviour
{
    private Vector3 wantedPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        wantedPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000.0f))
        {
            if (hit.collider.CompareTag("Water"))
            {
                transform.position = new Vector3(wantedPos.x, transform.position.y, wantedPos.z);
            }
            else
            {
                wantedPos = transform.position;
            }
        }
    }
}
