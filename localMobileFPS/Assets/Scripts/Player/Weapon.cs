using UnityEngine;

public enum FiringMode
{
    Automatic,
    Burst,
    Bolt
}
public class Weapon : MonoBehaviour
{
    public float Damage;
    public float MaxAmmo;
    public float CurrentAmmo;
    public float SideAmmo;
    public float MaxSideAmmo;
    public float FireRate;
    public float MaxDistance;
    public Transform FirePoint;
    public FiringMode firingMode;
    private float FireTime;

    public virtual void Fire()
    {
        if (FireTime <= Time.time)
        {
            //on = true;
            if (Physics.Raycast(FirePoint.position, FirePoint.forward, out RaycastHit hit, MaxDistance))
            {
                Debug.Log("Fire: hitName :" + hit.transform.name);
                Debug.Log("Fire: hitTag :" + (hit.transform.tag));
            }
            FireTime = Time.time + FireRate;
        }
    }

    public virtual void Reload()
    {
        SideAmmo += CurrentAmmo;
        CurrentAmmo = 0;
        if (SideAmmo > 0)
        {
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
