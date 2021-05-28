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

    public MobileTouchButton FireButton;
    public MobileTouchButton ADSButton;


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
        // android input
        if (Application.platform == RuntimePlatform.Android)
        {
            if(FireButton.pressed)
                Fire();
            SwicthADSServerRpc(ADSButton.pressed);
            return;
        }
        // windows input 
        //Debug.Log("in update attack");
        switch (CurrentWeapon.firingMode)
        {
            case FiringMode.Automatic:
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    //Debug.Log("Shooting");
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
        SwicthADSServerRpc(Input.GetMouseButton(1));

    }


    [ServerRpc]
    public void SwicthADSServerRpc(bool ads)
    {
        SwicthADSClientRpc(ads);
    }

    [ClientRpc]
    public void SwicthADSClientRpc(bool ads)
    {

        // this needs to be done using animations
        CurrentWeapon.transform.parent = ads ? ADSPos : HipPos;
        CurrentWeapon.transform.localPosition = Vector3.zero;
        CurrentWeapon.SwitchADSClientRpc(ads);
    }

    //[ServerRpc]
    public void Fire()
    {
        //Debug.Log("inFireServer");
        if (CurrentWeapon == null)
            return;
        //Debug.Log("Trying to fire the weapon");
        if (CurrentWeapon.CurrentAmmo <= 0)
            CurrentWeapon.ReloadServerRpc();
        else CurrentWeapon.Fire();
    }

    public void Reload()
    {
        CurrentWeapon.ReloadServerRpc();
    }
}
