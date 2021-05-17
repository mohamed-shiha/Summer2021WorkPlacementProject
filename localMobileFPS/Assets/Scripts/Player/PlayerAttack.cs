using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Weapon CurrentWeapon;
    public Weapon PreviousWeapon;

    private void Start()
    {
        CurrentWeapon = GetComponentInChildren<Weapon>();
    }
    private void Update()
    {
        if (CurrentWeapon == null)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Fire();
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
