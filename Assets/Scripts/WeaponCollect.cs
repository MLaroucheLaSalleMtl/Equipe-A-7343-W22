using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollect : MonoBehaviour
{
    WeaponManager _wpManager;
    [SerializeField] private GameObject weaponPrefab;

    private void Awake()
    {
        _wpManager = FindObjectOfType<RigidBodyFPSController>().GetComponentInChildren<WeaponManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RigidBodyFPSController>())
        {
            Collect();
        }
    }

    public void Collect()
    {
        GameObject tmpWeaponPrefab;
        if (_wpManager.weaponPickUpList.Count == 1)
        {
            tmpWeaponPrefab = Instantiate(weaponPrefab, _wpManager.transform) as GameObject;
            _wpManager.weaponPickUpList.Add(tmpWeaponPrefab);
            _wpManager.UpdateWeaponList();
            tmpWeaponPrefab.SetActive(false);

            Destroy(this.gameObject);           
        }
        else
        {
            print("!!! You already have this weapon !!!");
            Destroy(this.gameObject);
            return;
        }
    }
}
