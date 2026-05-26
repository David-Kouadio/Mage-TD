using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get;set; }

    public GameObject weaponslots;    

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalFrierenAmmo = 0;

    public Weapon weapon;
    //private int mode;

    //if(inputs.Mode)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if(weaponslots == activeWeaponSlot) weaponslots.SetActive(true);
        else weaponslots.SetActive(false);
    }

    void Start()
    {
        activeWeaponSlot = weaponslots;
    }

    public void PickupWeapon(GameObject pickedupWeapon)
    {
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {

        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();

        
        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);
        weaponslots.GetComponent<Animator>().enabled = true;
        weapon.isActiveWeapon = true;
        
    }

    internal void PickupAmmo(FireAmmo ammo)
    {
        totalFrierenAmmo += ammo.ammoAmout; 
    }

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.FrierenStaff:
                totalFrierenAmmo -= bulletsToDecrease;
                break;
        }
    }

    public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.FrierenStaff:
                return totalFrierenAmmo;
            
            default:
                return 0;
        }
    }

    /*public void SwitchModes (int modeNumber)
    {
        if(activeWeaponSlot.transform.childCount > 0)
        {
            if(modeNumber % 2 == 0)
            {
                weapon.shootingDelay = 2;

            }
        }
    }*/
}
