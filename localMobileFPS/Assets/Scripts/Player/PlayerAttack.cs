using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    public Weapon CurrentWeapon;
    public Weapon PreviousWeapon;
    public Transform HipPos;
    public Transform ADSPos;
    public bool PlayMode = false;

    private void Start()
    {
        //if (IsLocalPlayer)

       CurrentWeapon = GetComponentInChildren<Weapon>();
    }

    private void Update()
    {
        if (!IsLocalPlayer)
            return;

        if (CurrentWeapon == null)
            return;

        if (!PlayMode)
            return;

        //Debug.Log("in update attack");
        switch (CurrentWeapon.firingMode)
        {
            case FiringMode.Automatic:
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    Debug.Log("Shooting");
                    Fire();
                }
                break;
            case FiringMode.Burst:
            case FiringMode.Bolt:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    Fire();
                break;
        }

        if (Input.GetKeyUp(KeyCode.R))
            Reload();

        var ads = Input.GetMouseButton(1);
        CurrentWeapon.transform.parent = ads ? ADSPos : HipPos;
        CurrentWeapon.transform.localPosition = Vector3.zero;
        CurrentWeapon.SwitchADS(ads);

    }

    //[ServerRpc]
    public void Fire()
    {
        Debug.Log("inFireServer");
        if (CurrentWeapon == null)
            return;
        Debug.Log("Trying to fire the weapon");
        if (CurrentWeapon.CurrentAmmo <= 0)
            CurrentWeapon.Reload();
        else CurrentWeapon.Fire();
    }

    public void Reload()
    {
        CurrentWeapon.Reload();
    }
}
