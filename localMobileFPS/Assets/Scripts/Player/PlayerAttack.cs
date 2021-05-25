using MLAPI;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    public Weapon CurrentWeapon;
    public Weapon PreviousWeapon;
    public Transform HipPos;
    public Transform ADSPos;

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

        var ads = Input.GetMouseButton(1);
        CurrentWeapon.transform.parent = ads ? ADSPos : HipPos;
        CurrentWeapon.transform.localPosition = Vector3.zero;
        CurrentWeapon.SwitchADS(ads);

        /*
                if (Input.GetMouseButton(2))
                {
                    CurrentWeapon.transform.parent = ADSPos;
                    CurrentWeapon.transform.localPosition = Vector3.zero;
                    CurrentWeapon.SwitchADS(true);
                }
                else if(Input.GetMouseButtonUp(2))
                {
                    CurrentWeapon.transform.parent = HipPos;
                    CurrentWeapon.transform.localPosition = Vector3.zero;
                    CurrentWeapon.SwitchADS(false);
                }*/
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
