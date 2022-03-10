using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle_State : IState
{
    #region Singleton
    static WeaponClassScriptableObject _weaponClassScriptableObject;
    //private static WeaponTypeEnum _weaponType;

    //public static Animator _playerAnimator;
    private static AssaultRifle_State instance = null;

    private AssaultRifle_State() { }

    //WeaponTypeEnum IState._weaponType => ;

    //WeaponTypeEnum IState._weaponType { set => _weaponClassScriptableObject.WeaponType; }

    public static AssaultRifle_State GetInstance()
    {
        if (instance == null)
        {
            instance = new AssaultRifle_State();
        }        

        return instance;
    }
    #endregion

    public bool CanFire()
    {
        return true;
    }

    public WeaponType WeaponTypeUpdater(WeaponType _weaponType)
    {
        _weaponType = _weaponClassScriptableObject.WeaponType;
        return _weaponType;
    }

    //public Animator playerAimator(Animator _playerAnimator)
    //{
    //    return _playerAnimator;
    //}
}
