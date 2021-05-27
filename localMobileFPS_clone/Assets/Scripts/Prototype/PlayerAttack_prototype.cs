using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_prototype : MonoBehaviour
{
    public Weapon CurrentWeapon;
    public Weapon PreviousWeapon;

    private void Start()
    {
        CurrentWeapon = GetComponentInChildren<Weapon>();
    }


    private void Update()
    {
        if (CurrentWeapon == null)
            return;
        switch (CurrentWeapon.firingMode)
        {
            case FiringMode.Automatic:
                if (Input.GetKey(KeyCode.Mouse0))
                    Fire();
                break;
            case FiringMode.Burst:
            case FiringMode.Bolt:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    Fire();
                break;
        }

        if (Input.GetKeyUp(KeyCode.R))
            Reload();
    }

    public void Fire()
    {
        if (CurrentWeapon.CurrentAmmo <= 0)
            Reload();
        else CurrentWeapon.Fire();
    }

    public void Reload()
    {
        CurrentWeapon.ReloadServerRpc();
    }
}
