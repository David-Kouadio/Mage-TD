using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Weapon : MonoBehaviour
{

    // Portar arma
    public bool isActiveWeapon;

    public Camera playerCamera;

    //Atirar
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    // Rajada
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    // Disperção 
    public float spreadIntensity;

    // Bala
    private PlayerInput inputActions;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifTime = 3f; 

    // Animação
    private Animator animator;

    // Carregamento
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    // Pegar arma
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;


    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;


    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        inputActions = new PlayerInput();

        bulletsLeft = magazineSize;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        if(isActiveWeapon)
        {
            animator = WeaponManager.Instance.weaponslots.GetComponent<Animator>();
            

            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptySoundfrieren.Play();
            }


            if (currentShootingMode == ShootingMode.Auto)
            {
                // Segurar o botao esquerdo do rato
                isShooting = inputActions.OnFoot.Shoot.IsPressed();
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                // Clickar com o botao esquerdo uma vez
                isShooting = inputActions.OnFoot.Shoot.triggered;
            }

            if (inputActions.OnFoot.Reload.triggered && bulletsLeft < magazineSize && isReloading == false)
            {
                // recarregar a munição manualmente
                Reload();
            }

            if(readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
            {
                // recarregar automaticamente
                Reload();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }

            if (inputActions.OnFoot.Reload.triggered && bulletsLeft < magazineSize && isReloading == false)
            {
                // recarregar a munição manualmente
                Reload();
            }

            AmmoManager.Instance.ammo.SetActive(true);
            AmmoManager.Instance.arm.SetActive(true);

            if(AmmoManager.Instance.ammoDisplay != null)
            {
                AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft/bulletsPerBurst}/{magazineSize/bulletsPerBurst}";
            }

            

        }
    }

    private void FireWeapon()
    {
        // diminuir munição
        bulletsLeft--;

        // instanciar animação
        animator.SetTrigger("SHOOTING");

        // instanciar som
        SoundManager.Instance.shootingSoundfrieren.Play();

        readyToShoot = false;

        Vector3 shootingDirecition = CalculateDirectionAndSpread().normalized;

        // Instanciar a bala
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity );

        // Apontar a bala para a direção do disparo
        bullet.transform.forward = shootingDirecition;

        // Atirar a bala 
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirecition * bulletVelocity, ForceMode.Impulse);

        // Destruir a bala
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifTime ));

        // Checar se parou de atirar
        if (allowReset)
        {
            Invoke("ResetShot",shootingDelay);
            allowReset = false;
        }

        // Modo Rajada
        if(currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) // ja atirou uma vez por isso maior que 1
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);    
        }



    }

    private void Reload()
    {
        SoundManager.Instance.reloadingSoundfrieren.Play();

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        // Atirar do meio da tela para checar onde esta apontando
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            // atinjiu algo
            targetPoint = hit.point;
        }
        else
        {
            // atirando para o ar
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // retorna a direção e disperção do tiro
        return direction + new Vector3 (x,y,0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletPrefabLifTime)
    {
        yield return new WaitForSeconds(bulletPrefabLifTime);
        Destroy(bullet);
    }
}
