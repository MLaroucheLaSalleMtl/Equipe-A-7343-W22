using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleScript : MonoBehaviour
{
    //private WeaponScriptableObject weaponScriptable;
    [SerializeField] private Transform _weaponMuzzleLocation;
    private ParticleSystem wpVFX;
    //private LineRenderer wpLine;

    private float duration = 0.03f;

    public Transform WeaponMuzzleLocation { get => _weaponMuzzleLocation; set => _weaponMuzzleLocation = value; }

    // Start is called before the first frame update
    void Start()
    {
        wpVFX = GetComponent<ParticleSystem>();
        //wpLine = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    public void StartEmit(float lineDistance)
    {
        wpVFX.Play();
        //wpVFX.TriggerSubEmitter(0);

        //GameObject bulletOBJCopy = BulletPool.SharedBulletInstance.GetPooledBullet();
        //if (bulletOBJCopy != null)
        //{
        //    bulletOBJCopy.transform.position = _weaponMuzzleLocation.transform.position;
        //    bulletOBJCopy.transform.rotation = _weaponMuzzleLocation.transform.rotation;
        //    bulletOBJCopy.SetActive(true);
        //}

        //Vector3 endPosition = transform.position + transform.forward * lineDistance;
        //DrawLine(endPosition);
    }

    public void StartEmit(Vector3 endPosition)
    {
        wpVFX.Play();
        wpVFX.TriggerSubEmitter(0);
        
        //DrawLine(endPosition);
    }

    //void DrawLine(Vector3 endPosition)
    //{
    //    wpLine.enabled = true;
    //    Vector3[] points = new Vector3[2];
    //    points[0] = transform.position;
    //    points[1] = endPosition;
    //    wpLine.positionCount = points.Length;
    //    //wpLine.SetPositions(points);

    //    Invoke("StopEmit", duration);
    //}

    //void StopEmit() 
    //{
    //    wpLine.enabled = false; 
    //}
}
