using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_State : IState
{
    #region Singleton
    static WeaponClassScriptableObject _weaponClassScriptableObject;
    //private static WeaponTypeEnum _weaponType;

    //public static Animator _playerAnimator;
    //private static Animator _playerAnimator;
    private static Normal_State instance = null;

    //WeaponTypeEnum IState._weaponType => weaponClassScriptableObject.WeaponType;

    private Normal_State() { }

    public static Normal_State GetInstance()
    {
        if (instance == null)
        {
            instance = new Normal_State();
        }
        //_weaponType = WeaponTypeEnum.Unarmed;

        return instance;
    }
    #endregion

    public bool CanFire()
    {
        return false;
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
