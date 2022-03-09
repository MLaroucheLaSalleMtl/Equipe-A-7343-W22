using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponClassScriptableObject", menuName = "ScriptableObjects/WeaponClass", order = 0)]
public class WeaponClassScriptableObject : ScriptableObject
{
    [SerializeField] private WeaponType weaponType; 

    private IState current_State;   

    public IState Current_State { get => current_State; set => current_State = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }

    WeaponScriptableObject weaponScriptableObject;

    //private void Awake()
    //{
    //    this.
    //}

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
