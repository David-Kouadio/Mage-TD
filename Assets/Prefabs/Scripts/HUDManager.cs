using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get;set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;

    [Header("UIthings")]
    public GameObject ammo;
    public GameObject arm;
    public GameObject deactivated;
    public GameObject door;

    [Header("PlayerUI")]
    public GameObject text;
    public GameObject crosshair;
    public GameObject HPbar;
    public GameObject Minimap;
    public GameObject overlay;

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
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";
        }
    }



}
