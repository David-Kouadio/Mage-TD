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
    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;

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

        targetWeaponRotation.y += settings.swayAmount * (settings.SwayXInverted ? -playerController.input_View.x : playerController.input_View.x) * Time.deltaTime;
        targetWeaponRotation.x += settings.swayAmount * (settings.SwayYInverted ? playerController.input_View.y : -playerController.input_View.y) * Time.deltaTime;
        
        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.SwayClampX, settings.SwayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.SwayClampY, settings.SwayClampY);

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.SwayResetSmoothning);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.SwaySmoothning);

        transform.localRotation = Quaternion.Euler(newWeaponRotation);





    }
}
