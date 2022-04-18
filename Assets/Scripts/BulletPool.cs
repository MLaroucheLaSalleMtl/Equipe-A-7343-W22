using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletPool : MonoBehaviour
{
    public static BulletPool SharedBulletInstance;
    public List<GameObject> _bulletsList;
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
        _bulletsList = new List<GameObject>();
        GameObject tmpBullet;
        for (int i = 0; i < _weaponShoot.currMagAmmo; i++)
        {
            tmpBullet = Instantiate(bulletToPool);
            tmpBullet.SetActive(false);
            _bulletsList.Add(tmpBullet);
        }
    }    

    public GameObject GetPooledBullet()
    {
        for (int i = 0; i < _weaponShoot.currMagAmmo; i++)
        {
            if (!_bulletsList[i].activeInHierarchy)
            {
                return _bulletsList[i];
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
