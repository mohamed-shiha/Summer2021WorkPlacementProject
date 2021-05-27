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

    private float FireTime;
    Transform FirePoint;

    // run on server called by the client 
    [ClientRpc]
    private void FireClientRpc()
    {
        if (Physics.Raycast(FirePoint.position, FirePoint.forward, out RaycastHit hit, MaxDistance, DamageMask))
        {
            var tag = hit.transform.tag;
            float damage = 0;
            if (tag.Contains("Player"))
            {
                Debug.Log(hit.transform.tag);

                if (hit.transform.CompareTag("PlayerHead"))
                {
                    damage = HeadDamage;

                }
                else if (hit.transform.CompareTag("PlayerBody"))
                {
                    damage = BodyDamage;
                }

            }

        }
        Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
        CurrentAmmo--;
    }

    // run on server called by the client 
    [ServerRpc]
    private void FireServerRpc()
    {
        FireClientRpc();

        if (Physics.Raycast(FirePoint.position, FirePoint.forward, out RaycastHit hit, MaxDistance))
        {
            var tag = hit.transform.tag;
            if (tag.Contains("Player"))
            {
                float damage = tag.Contains("Body")? BodyDamage : HeadDamage;
                var health = hit.transform.GetComponentInParent<Health>();
                health.TakeDamage(damage);
            }

        }
    }

    public virtual void Fire()
    {
        if (FireTime <= Time.time)
        {
            //on = true;
            /*if (FirePoint == null)
                FirePoint = ADSFirePoint;*/
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
        FirePoint = isAds ? ADSFirePoint : HipFirePoint;
        //transform.parent = ads ? ADSPos : HipPos;
        //transform.localPosition = Vector3.zero;
    }

    [ServerRpc]
    public void SwitchADSServerRpc(bool isAds)
    {
        SwitchADSClientRpc(isAds);
    }
}
