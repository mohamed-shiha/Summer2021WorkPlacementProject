using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Damage;
    public float MaxAmmo;
    public float CurrentAmmo;
    public float SideAmmo;
    public float MaxSideAmmo;
    public float FireRate;
    private float FireTime;
    public float MaxDistance;
    public Transform FirePoint;

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

   /* private void OnDrawGizmos()
    {
        if (on)
        {
            Gizmos.DrawLine(FirePoint.position, FirePoint.forward.normalized * 1000);
            on = false;
        }
    }*/
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
