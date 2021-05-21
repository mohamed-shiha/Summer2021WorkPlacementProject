using MLAPI;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    public Weapon CurrentWeapon;
    public Weapon PreviousWeapon;

    private void Start()
    {
        if (IsLocalPlayer)
            CurrentWeapon = GetComponentInChildren<Weapon>();
    }

    private void Update()
    {
        if (!IsLocalPlayer)
            return;

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
            CurrentWeapon.Reload();
        else CurrentWeapon.Fire();
    }

    public void Reload()
    {
        CurrentWeapon.Reload();
    }
}
