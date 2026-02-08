using UnityEngine;
using static scr_Models;
public class scr_WeaponController : MonoBehaviour
{
    private scr_PlayerController playerController;

    [Header("Settings")]
    public WeaponSettingsModel settings;

    bool isInitialised;

    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
    }

    public void Initialise(scr_PlayerController PlayerController)
    {
        playerController = PlayerController;
        isInitialised = true;
    }
    private void Update()
    {
        if (!isInitialised)
        {
            return;
        }

        newWeaponRotation.y += settings.swayAmount * (settings.SwayXInverted ? -playerController.input_View.x : playerController.input_View.x) * Time.deltaTime;
        newWeaponRotation.x += settings.swayAmount * (settings.SwayYInverted ? playerController.input_View.y : -playerController.input_View.y) * Time.deltaTime;
        //newWeaponRotation.x = Mathf.Clamp(newWeaponRotation.x, viewClampYMin, viewClampYMax);

        transform.localRotation = Quaternion.Euler(newWeaponRotation);





    }
}
