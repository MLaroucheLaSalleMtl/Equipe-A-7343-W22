using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_State : IState
{
    #region Singleton
    private static WeaponType _weaponType = WeaponType.Pistol;

    //public static Animator _playerAnimator;
    private static Pistol_State instance = null;

    private Pistol_State() { }

    public static Pistol_State GetInstance()
    {
        if (instance == null)
        {
            instance = new Pistol_State();
        }
        _weaponType = WeaponType.Pistol;

        return instance;
    }
    #endregion

    public bool CanFire()
    {
        return true;
    }

    //public Animator playerAimator(Animator _playerAnimator)
    //{
    //    return _playerAnimator;
    //}
}
