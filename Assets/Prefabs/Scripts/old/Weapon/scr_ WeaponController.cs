using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static scr_Models;
using TMPro;
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
    public WeaponFireType currentFireType;
    [HideInInspector]
    public List<WeaponFireType> allowedFireTypes;
    [HideInInspector]
    public bool isShooting;
    //balas
    public GameObject bullet;
    //força da bala
    public float shootForce, upwardForce;
    //Status da arma
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazingSize, bulletsPerTap;
    public int bulletsLeft, bulletsShot;
    //bools
    bool shooting, readyToShoot, reloading;
    //Referencias
    public Camera fpsCam;
    public Transform attackPoint; //arma
    //correção de bug
    public bool allowInvoke = true;
    //Graficos
    public TextMeshProUGUI ammunitionDisplay;

    public bool isShootingHolding;
    public int timesShooted;
    public bool askReload;




    #region - Start / Update - 
    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;

        currentFireType = allowedFireTypes.First();

                //Garantir que a munição está completa
        bulletsLeft = magazingSize;
        readyToShoot = true;

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
        CallReload();

        //Faz o texto da munição, se ela existir
        if(ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazingSize / bulletsPerTap);
        }
    }
    
    #endregion

    #region - Shooting - 
    
    private void CalculateShooting()
    {
        if (isShooting)
        {
            shooting = true;

            if(isShootingHolding == true && currentFireType != WeaponFireType.SemiAuto)
            {
                timesShooted = 0;
            }
            if(timesShooted == 0)
            {
                timesShooted++;
                //Regarregar automaticamente se não tiver munição
                if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

                //Atirar
                if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
                {
                    //Define os tiros de bala para 0
                    bulletsShot = 0;

                    Shoot();
                }   
            }   
        }
        else timesShooted = 0;
    }

    private void CallReload()
    {
        if(askReload) Reload();
    }

        private void Shoot()
    {
    readyToShoot = false;

    //Achar a posição da mira usando um raycast
    Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f,0.5f,0)); //ray no meio da tela
    RaycastHit hit;

    //Checar se o raio colide em algo
    Vector3 targetPoint;
    if(Physics.Raycast(ray, out hit))
        targetPoint = hit.point;
    else
        targetPoint = ray.GetPoint(75); //ponto longe do player

    //Calcular a direção da arma para o ponto de colisão
    Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

    //Calcular o espalhamento
    float x = UnityEngine.Random.Range(-spread, spread);
    float y = UnityEngine.Random.Range(-spread, spread);

    //Calcular a nova direção da bala com espalhamento
    Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x,y,0); //Adiona o espalhamento nas posições x e y

    //Instancia da bala/projectil
    GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //armazena a bala/projectil dentro da variavel
    //Rotaciona a bala para a direção do disparo
    currentBullet.transform.forward = directionWithSpread.normalized;

    //adiciona força a disparo
    currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        
    //Adicionar gravidade nos projeteis
    //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
        
    bulletsLeft--;
    bulletsShot++;

    //Invoca a função de resetar tiro (se já não tiver sido invocada)
    if (allowInvoke)
    {
        Invoke("ResetShot", timeBetweenShooting); //Invoke(Nome da função que vai ser chamada, tempo que vai demorar pra ser chamada)
    allowInvoke = false;
    }

    //Caso seja preciso atirar mais que uma bala/projectil
    if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
    Invoke("ResetShot", timeBetweenShooting);


    }

    private void ResetShot()
    {
        //Permitir atirar e invocar de novo
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazingSize;
        reloading = false;
        askReload = false;
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
