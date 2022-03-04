using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject weaponScriptableObjet;
    private int currDMG;
    private int currMagAmmo;
    private int currAvailableAmmo;

    // Start is called before the first frame update
    void Start()
    {
        currDMG = weaponScriptableObjet.weaponDMG;
        currMagAmmo = weaponScriptableObjet.weaponMagazineAmmo;
        currAvailableAmmo = weaponScriptableObjet.weaponMaxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
