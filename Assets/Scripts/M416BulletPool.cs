using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class M416BulletPool : MonoBehaviour
{
    public static M416BulletPool SharedBulletInstance;
    public List<GameObject> _m416BulletsList;
    public GameObject bulletToPool;

    WeaponManager weaponManager;

    WeaponShoot _weaponShoot;

    private void Awake()
    {
        weaponManager = WeaponManager.instance;
        _weaponShoot = GetComponent<WeaponShoot>();        
        SharedBulletInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _m416BulletsList = new List<GameObject>();        
        GameObject tmpBullet;
        for (int i = 0; i < _weaponShoot.currMagAmmo; i++)
        {
            tmpBullet = Instantiate(bulletToPool);
            tmpBullet.SetActive(false);
            _m416BulletsList.Add(tmpBullet);            
        }
    }

    public void GetM416PooledBullet(Transform muzzleTransform, float fireRate)
    {
        for (int i = 0; i < _m416BulletsList.Count; i++)
        {
            if (!_m416BulletsList[i].activeInHierarchy)
            {
                _m416BulletsList[i].transform.position = muzzleTransform.position;
                _m416BulletsList[i].transform.rotation = muzzleTransform.rotation;
                _m416BulletsList[i].SetActive(true);
                Rigidbody bulletRB = _m416BulletsList[i].GetComponent<Rigidbody>();
                bulletRB.AddForce(muzzleTransform.transform.forward * fireRate);
                break;
            }
        }        
    }    
}