using System.Runtime.InteropServices.ComTypes;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public enum FiringMode
{
    Automatic,
    Burst,
    Bolt
}
public class Weapon : NetworkBehaviour
{
    public float HeadDamage;
    public float BodyDamage;
    public float MaxAmmo;
    public float CurrentAmmo;
    public float SideAmmo;
    public float MaxSideAmmo;
    public float FireRate;
    public float ReloadTime;
    public float MaxDistance;

    public Transform HipFirePoint;
    public Transform ADSFirePoint;
    public GameObject bulletPrefab;
    public FiringMode firingMode;
    public LayerMask DamageMask;
   // [HideInInspector]
    public Transform FirePoint;

    private float FireTime;
    /*{
        get
        {
            if (_FirePoint == null)
                _FirePoint =  GetComponentInChildren<Camera>().transform;
            return _FirePoint;
        }
    }*/
    Transform _FirePoint;

    // run on server called by the client 
    [ClientRpc]
    private void FireClientRpc()
    {
        Instantiate(bulletPrefab, ADSFirePoint.position, ADSFirePoint.rotation);
        CurrentAmmo--;
    }

    // run on server called by the client 
    [ServerRpc]
    private void FireServerRpc()
    {
        FireClientRpc();
        if (Physics.SphereCast(FirePoint.position, 0.5f, FirePoint.forward, out RaycastHit hit, MaxDistance * 100, DamageMask))
        //if (Physics.Raycast (FirePoint.position, FirePoint.forward, out RaycastHit hit,MaxDistance*10 ,DamageMask))
        {
            Debug.Log("On Server" + hit.transform.tag);
            var tag = hit.transform.tag;
            if (tag.Contains("Player"))
            {
                float damage = tag.Contains("Body") ? BodyDamage : HeadDamage;
                var health = hit.transform.GetComponentInParent<Health>();
                health.TakeDamage(damage);
            }

        }
    }

    public virtual void Fire(Transform camera)
    {
        //FirePoint = camera;
        if (FireTime <= Time.time)
        {
            FireServerRpc();
            FireTime = Time.time + FireRate;
        }
    }

    [ServerRpc]
    public virtual void ReloadServerRpc()
    {
        ReloadClientRpc();
    }

    [ClientRpc]
    private void ReloadClientRpc()
    {
        SideAmmo += CurrentAmmo;
        CurrentAmmo = 0;
        FireTime = Time.time + ReloadTime;
        if (SideAmmo > 0)
        {
            Debug.Log("Reloading");
            if (SideAmmo >= MaxAmmo)
            {
                CurrentAmmo = MaxAmmo;
                SideAmmo -= CurrentAmmo;
            }
            else
            {
                CurrentAmmo = SideAmmo;
                SideAmmo = 0;
            }
        }
    }

    public void SwitchADS(bool isAds)
    {
        SwitchADSServerRpc(isAds);
    }

    [ClientRpc]
    public void SwitchADSClientRpc(bool isAds)
    {
        //FirePoint = isAds ? ADSFirePoint : HipFirePoint;
        //transform.parent = ads ? ADSPos : HipPos;
        //transform.localPosition = Vector3.zero;
    }

    [ServerRpc]
    public void SwitchADSServerRpc(bool isAds)
    {
        SwitchADSClientRpc(isAds);
    }
}
