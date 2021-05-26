using MLAPI.Messaging;
using UnityEngine;

public enum FiringMode
{
    Automatic,
    Burst,
    Bolt
}
public class Weapon : MonoBehaviour
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


    //[ServerRpc]
    public virtual void Fire()
    {
        if (FireTime <= Time.time)
        {
            //on = true;
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
                    var health = hit.transform.GetComponentInParent<Health>();
                    if (health.TakeDamage(damage))
                        health.Die();// got a kill
                }

            }

            Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
            CurrentAmmo--;
            FireTime = Time.time + FireRate;
        }
    }

    public virtual void Reload()
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
       FirePoint = isAds? ADSFirePoint: HipFirePoint;
    }
}
