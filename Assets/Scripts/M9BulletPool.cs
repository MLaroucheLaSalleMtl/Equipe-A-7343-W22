using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M9BulletPool : MonoBehaviour
{
    public static M9BulletPool SharedBulletInstance;
    public List<GameObject> _m9BulletsList;
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
        _m9BulletsList = new List<GameObject>();
        GameObject tmpBullet;
        for (int i = 0; i < _weaponShoot.currMagAmmo; i++)
        {            
            tmpBullet = Instantiate(bulletToPool);
            tmpBullet.SetActive(false);
            _m9BulletsList.Add(tmpBullet);            
        }
    }

    public void GetM9PooledBullet(Transform muzzleTransform, float fireRate)
    {
        for (int i = 0; i < _weaponShoot.currMagAmmo; i++)
        {
            if (!_m9BulletsList[i].activeInHierarchy)
            {
                _m9BulletsList[i].transform.position = muzzleTransform.position;
                _m9BulletsList[i].transform.rotation = muzzleTransform.rotation;
                _m9BulletsList[i].SetActive(true);
                Rigidbody bulletRB = _m9BulletsList[i].GetComponent<Rigidbody>();
                bulletRB.AddForce(muzzleTransform.transform.forward * fireRate);                
                break;
            }
        }        
    }
}
