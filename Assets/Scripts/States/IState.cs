using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    //WeaponTypeEnum _weaponType { set; }
    WeaponType WeaponTypeUpdater(WeaponType _weaponType);

    bool CanFire();

    //Animator playerAimator(Animator _playerAnimator);
}
