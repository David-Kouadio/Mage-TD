using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;

    public GameObject gameOverUI;

    public GameObject weapon;

    public bool isDead;

    [Header("Health Bar")]
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;
    
    [Header("Damage Overlay")]
    public Image overlay; 
    public float duration;
    public float fadespeed;
    private float durationTimer;

    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }
    void Update()
    {
        health = Mathf.Clamp(health,0,maxHealth);
        UpdateHealthUI();
        if(overlay.color.a > 0)
        {
            if(health < 30)
            {
                return;
            }
            durationTimer += Time.deltaTime;
            if(durationTimer > duration)
            {
                //transparecer a imagem
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadespeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if(fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if(fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction,percentComplete);
        }
        
        healthText.text = health + "/" + maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;

        if (health <= 0)
        {
            PlayerDead();
            isDead = true;
        }
        else
        {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.75f);
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);

        SoundManager.Instance.playerChannel.clip = SoundManager.Instance.gameOverMusic;
        SoundManager.Instance.playerChannel.PlayDelayed(2f);

        GetComponent<PlayerMotor>().enabled = false;
        GetComponent<PlayerLook>().enabled = false;

        // Dying Animation
        GetComponentInChildren<Animator>().enabled = true;
        HUDManager.Instance.ammo.SetActive(false);
        HUDManager.Instance.text.SetActive(false);
        HUDManager.Instance.HPbar.SetActive(false);
        HUDManager.Instance.Minimap.SetActive(false);
        HUDManager.Instance.crosshair.SetActive(false);
        HUDManager.Instance.overlay.SetActive(false);
        
        weapon.SetActive(false);

        Cursor.lockState = CursorLockMode.None;

        GetComponent<ScreenFader>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DamageGround"))
        {
            if (!isDead)
            {
                TakeDamage(1000);
            }
            
        }
    }
}
