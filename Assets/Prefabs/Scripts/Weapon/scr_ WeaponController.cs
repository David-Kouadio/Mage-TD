using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static scr_Models;
public class scr_WeaponController : MonoBehaviour
{
    private scr_PlayerController playerController;

    [Header("References")]
    public Animator weaponAnimator;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    [Header("Settings")]
    public WeaponSettingsModel settings;

    bool isInitialised;

    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;
    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;

    Vector3 newWeaponMovementRotation;
    Vector3 newWeaponMovementRotationVelocity;
    Vector3 targetWeaponMovementRotation;
    Vector3 targetWeaponMovementRotationVelocity;

    private bool isGroundedTrigger;

    private float fallingDelay;

    [Header("Weapon Breathing")]
    public Transform weaponSwayObject;

    public float swayAmountA = 1;
    public float swayAmountB = 2;
    public float swayScale = 600;
    public float swayLerpSpeed = 14;

    float swayTime;
    Vector3 swayPosition;

    [Header("Sights")]
    public Transform sightTarget;
    public float sightOffset;
    public float aimingIntime;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;
    [HideInInspector]
    public bool isAimingIn;

    [Header("Shooting")]
    public float rateOfFire;
    private float currentFireRate;
    public List<WeaponFireType> allowedFireTypes;
    public WeaponFireType currentFireType;
    [HideInInspector]
    public bool isShooting;

    #region - Start / Update - 
    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;

        currentFireType = allowedFireTypes.First();
    }
    private void Update()
    {
        if (!isInitialised)
        {
            return;
        }

        CalculateWeaponRotation();
        SetWeaponAnimation();
        CalculateWeaponSway();
        CalculateAimingIn();
        CalculateShooting();
    }
    
    #endregion

    #region - Shooting - 
    
    private void CalculateShooting()
    {
        if (isShooting)
        {
            Shoot();

            if (currentFireType == WeaponFireType.SemiAuto)
            {
                isShooting = false;
            }
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn);

        //Carregar as configurações da bala
    }

    #endregion

    #region - Initialise - 
        public void Initialise(scr_PlayerController PlayerController)
    {
        playerController = PlayerController;
        isInitialised = true;
    }
    
    #endregion

    #region - Aiming In - 
    private void CalculateAimingIn()
    {
        var targetPosition = transform.position;

        if (isAimingIn)
        {
            targetPosition = playerController.cameraHolder.transform.position + (weaponSwayObject.position - sightTarget.position) + (playerController.cameraHolder.transform.forward * sightOffset);
        }

        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, aimingIntime);
        weaponSwayObject.transform.position = weaponSwayPosition + swayPosition;
        
        
    }
    
    #endregion

    #region - Jumping - 
    public void TriggerJump()
    {
        weaponAnimator.SetTrigger("Jump");
        isGroundedTrigger = false;
    }
    
    #endregion

    #region - Rotation - 
    private void CalculateWeaponRotation()
    {
        targetWeaponRotation.y += (isAimingIn ? settings.swayAmount / 3 : settings.swayAmount) * (settings.SwayXInverted ? -playerController.input_View.x : playerController.input_View.x) * Time.deltaTime;
        targetWeaponRotation.x += (isAimingIn ? settings.swayAmount / 3 : settings.swayAmount) * (settings.SwayYInverted ? playerController.input_View.y : -playerController.input_View.y) * Time.deltaTime;
        
        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.SwayClampX, settings.SwayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.SwayClampY, settings.SwayClampY);
        targetWeaponRotation.z = isAimingIn ? 0 : targetWeaponRotation.y;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.SwayResetSmoothning);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.SwaySmoothning);

        targetWeaponMovementRotation.z = (isAimingIn ? settings.MovementSwayX / 3 : settings.MovementSwayX) * (settings.MovementSwayXInverted ? -playerController.input_Movement.x : playerController.input_Movement.x);
        targetWeaponMovementRotation.x = (isAimingIn ? settings.MovementSwayY / 3 : settings.MovementSwayY) * (settings.MovementSwayYInverted ? -playerController.input_Movement.y : playerController.input_Movement.y);

        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity, settings.MovementSwaySmoothning);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity, settings.MovementSwaySmoothning);


        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);
    }
    
    #endregion

    #region - Animation - 
    private void SetWeaponAnimation()
    {
        if (isGroundedTrigger)
        {
            fallingDelay = 0;
        }
        else
        {
            fallingDelay += Time.deltaTime;
        }

        if (playerController.isGrounded && !isGroundedTrigger && fallingDelay > 0.1f)
        {
            weaponAnimator.SetTrigger("Land");
            isGroundedTrigger = true;
        } 
        else if (!playerController.isGrounded && isGroundedTrigger)
        {
            weaponAnimator.SetTrigger("Falling");
            isGroundedTrigger = false;
        }

        weaponAnimator.SetBool("isSprinting", playerController.isSprinting);
        weaponAnimator.SetFloat("WeaponAnimationSpeed",playerController.weaponAnimatioSpeed);
    }
    
    #endregion

    #region - Sway -
    private void CalculateWeaponSway()
    {
        var targetposition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / (isAimingIn ? swayScale * 4: swayScale);

        swayPosition = Vector3.Lerp(swayPosition, targetposition, Time.smoothDeltaTime * swayLerpSpeed);
        swayTime += Time.deltaTime;

        if (swayTime > 6.3f)
        {
            swayTime = 0;
        }

        //weaponSwayObject.localPosition = swayPosition;
    }

    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        //função que simula um gráfico
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }
    #endregion
}
