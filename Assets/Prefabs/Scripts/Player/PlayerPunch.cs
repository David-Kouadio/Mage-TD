using System;
using System.Collections;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    private InputManager inputManager;
    private Camera playerCamera;

    [Header("Punch Settings")]
    public float punchDistance = 3f;
    public float punchCooldown = 0.5f;
    private float lastPunchTime;

    [Header("Reflection Settings")]
    public float reflectForce = 15f;
    public string projectileTag = "PukekoBullet";

    [Header("Audio")]
    public AudioClip punchSound;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerCamera = GetComponent<PlayerLook>()?.GetComponentInChildren<Camera>() ?? Camera.main;

        if (inputManager != null)
        {
            inputManager.onFoot.Punch.performed += ctx => TryPunch();
        }
        else
        {
            Debug.LogWarning("InputManager not found on Player!");
        }
    }

    void TryPunch()
    {
        if (Time.time - lastPunchTime < punchCooldown) return;

        // Ensure we don't punch when game is paused or player is dead
        if (PauseMenu.isPaused) return;
        var playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.isDead) return;

        lastPunchTime = Time.time;
        ExecutePunch();
    }

    private void ExecutePunch()
    {
        // Play punch animation if available
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Punch");
        }

        // Play punch sound
        if (punchSound != null)
        {
            AudioSource.PlayClipAtPoint(punchSound, transform.position);
        }

        // Raycast to detect enemies or projectiles in front
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, punchDistance))
        {
            // 1. Hit Enemy
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(25); // Deal 25 damage on punch
                Debug.Log("Punched enemy: " + hit.collider.name);
                return;
            }

            // 2. Hit and Reflect Projectile
            if (hit.collider.CompareTag(projectileTag))
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 reflectDirection = playerCamera.transform.forward;
                    rb.linearVelocity = Vector3.zero; // Stop existing velocity
                    rb.AddForce(reflectDirection * reflectForce, ForceMode.Impulse);
                }

                Debug.Log("Punched and reflected projectile!");
            }
        }
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.onFoot.Punch.performed -= ctx => TryPunch();
        }
    }
}
