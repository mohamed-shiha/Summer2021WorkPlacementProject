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
    public Transform FirePoint;
    public GameObject bulletPrefab;
    public FiringMode firingMode;
    private float FireTime;
    public LayerMask DamageMask;

    public virtual void Fire()
    {
        if (FireTime <= Time.time)
        {
            //on = true;
            if (Physics.Raycast(FirePoint.position, FirePoint.forward, out RaycastHit hit, MaxDistance, DamageMask))
            {
                Debug.Log("Fire: hitName :" + hit.transform.name);
                Debug.Log("Fire: hitTag :" + (hit.transform.tag));

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
                    if (hit.transform.GetComponentInParent<Health>().TakeDamage(damage))
                        Destroy(hit.transform.gameObject);// got a kill
                }

            }

            Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation).GetComponent<Bullet_Prototype>();
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
}
