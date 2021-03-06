using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol_State : IState
{
    //public bool IsFiring = false;
    #region Singleton   
    static WeaponClassScriptableObject _weaponClassScriptableObject;
    //private static WeaponTypeEnum _weaponType;

    //public static Animator _playerAnimator;
    private static Pistol_State instance = null;

    private Pistol_State() { }

    //WeaponTypeEnum IState._weaponType => weaponClassScriptableObject.WeaponType;

    public static Pistol_State GetInstance()
    {
        if (instance == null)
        {
            instance = new Pistol_State();
        }
        //_weaponType = WeaponTypeEnum.Pistol;

        return instance;
    }
    #endregion

    void IState.OnFire(InputAction.CallbackContext context, bool _isFiring)
    {
        throw new System.NotImplementedException();
    }

    //public WeaponType WeaponTypeUpdater(WeaponType _weaponType)
    //{
    //    _weaponType = _weaponClassScriptableObject.WeaponType;
    //    return _weaponType;
    //}

    //public Animator playerAimator(Animator _playerAnimator)
    //{
    //    return _playerAnimator;
    //}
}
