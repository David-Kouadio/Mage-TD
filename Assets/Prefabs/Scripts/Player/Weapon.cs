using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    private PlayerInput inputActions;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifTime = 3f; 

    private void Awake()
    {
        inputActions = new PlayerInput();
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
        if (inputActions.OnFoot.Shoot.triggered)
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        // Instanciar a bala
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity );
        // Atirar a bala 
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized *bulletVelocity, ForceMode.Impulse);
        // Destruir a bala
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifTime ));
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletPrefabLifTime)
    {
        yield return new WaitForSeconds(bulletPrefabLifTime);
        Destroy(bullet);
    }
}
