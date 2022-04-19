using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObjects/WeaponPickableItems")]
public class WeaponItemSO : ScriptableObject
{
    public static WeaponItemSO instance;
    WeaponManager _wpManager;
    [SerializeField] private GameObject wepaopnPrefab;

    private void Awake()
    {
        _wpManager = FindObjectOfType<WeaponManager>();
    }

    public void OnCollect()
    {
        if (_wpManager.weaponPickUpList != null)
        {
            if (!_wpManager.weaponPickUpList.Contains(wepaopnPrefab))
            {
                _wpManager.AddToList(wepaopnPrefab);
            }
            else
                return;
        }        
    }
}
