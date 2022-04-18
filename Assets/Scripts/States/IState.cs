using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IState
{
    //WeaponTypeEnum _weaponType { set; }
    //WeaponType WeaponTypeUpdater(WeaponType _weaponType);

    public void OnFire(InputAction.CallbackContext context, bool _isFiring);

    //Animator playerAimator(Animator _playerAnimator);
}
