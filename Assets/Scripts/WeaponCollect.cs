using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollect : MonoBehaviour
{
    //public delegate void OnWeaponDelegate();
    //public static OnWeaponDelegate onWeaponDelegate;

    //[SerializeField] private WeaponItemSO weaponItemSO;

    WeaponManager _wpManager;
    [SerializeField] private GameObject weaponPrefab;

    private void Awake()
    {
        _wpManager = FindObjectOfType<RigidBodyFPSController>().GetComponentInChildren<WeaponManager>();
        //_wpManager = FindObjectOfType<WeaponManager>();
        //onWeaponDelegate = Collect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        if (_wpManager.weaponPickUpList.Count > 0)
        {
            if (_wpManager.weaponPickUpList.Contains(weaponPrefab))
            {
                return;
            }
            else
            {
                _wpManager.weaponPickUpList.Add(weaponPrefab);
                _wpManager.UpdateWeaponList();
                Destroy(this.gameObject);
            }
            //if (_wpManager.weaponPickUpList.Contains(weaponPrefab))
            //{

            //weaponPrefab.SetActive(true);
            //Instantiate(weaponPrefab, this.transform);
            //_wpManager.WeaponPrefabs = _wpManager.weaponPickUpList.ToArray();
            //_wpManager.UpdateWeaponList(wepaopnPrefab);

            //_wpManager.AddToList(wepaopnPrefab);
            //_wpManager.UpdateWeaponList();
            //}
            //else
            //    return;
        }
    }

    private void Update()
    {
        //_wpManager.WeaponPrefabs = GameObject.FindGameObjectsWithTag("Weapon");
    }
}
