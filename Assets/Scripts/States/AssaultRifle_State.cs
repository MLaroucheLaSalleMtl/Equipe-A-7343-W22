using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle_State : IState
{
    #region Singleton
    //public static Animator _playerAnimator;
    private static AssaultRifle_State instance = null;
    private static WeaponType _weaponType = WeaponType.AssaultRifle;

    private AssaultRifle_State() { }

    public static AssaultRifle_State GetInstance()
    {
        if (instance == null)
        {
            instance = new AssaultRifle_State();
        }
        _weaponType = WeaponType.AssaultRifle;

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
