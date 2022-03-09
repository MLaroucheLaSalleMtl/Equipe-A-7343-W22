using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_State : IState
{
    #region Singleton
    private static WeaponType _weaponType = WeaponType.Unarmed;
    
    //public static Animator _playerAnimator;
    //private static Animator _playerAnimator;
    private static Normal_State instance = null;

    private Normal_State() { }

    public static Normal_State GetInstance()
    {
        if (instance == null)
        {
            instance = new Normal_State();
        }
        _weaponType = WeaponType.Unarmed;

        return instance;
    }
    #endregion

    public bool CanFire()
    {
        return false;
    }

    //public Animator playerAimator(Animator _playerAnimator) 
    //{
    //    return _playerAnimator;
    //}
}
