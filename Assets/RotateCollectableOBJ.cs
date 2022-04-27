using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCollectableOBJ : MonoBehaviour
{
    [Range(0f, 500)][Tooltip("Default Object Rotation Speed is 180")] public float rotationSpeed = 180;
    private SphereCollider m_Collider;
    private Vector3 m_Center;

    // Start is called before the first frame update
    void Start()
    {        
        m_Collider = GetComponent<SphereCollider>();
        m_Center = m_Collider.bounds.center;        
    }

    //// Update is called once per frame
    void Update()
    {
        transform.RotateAround(m_Center, new Vector3(0f, -1f, 0f), rotationSpeed * Time.deltaTime);
    }
}
