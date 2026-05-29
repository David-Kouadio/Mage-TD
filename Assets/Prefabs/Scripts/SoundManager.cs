using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get;set; }

    public AudioSource shootingSoundfrieren;
    public AudioSource reloadingSoundfrieren;
    public AudioSource emptySoundfrieren;

    public AudioClip pukekoWalking;
    public AudioClip pukekoChase;
    public AudioClip pukekoAttack;
    public AudioClip pukekoHurt;
    public AudioClip pukekoDeath;

    public AudioSource pukekoChannel;

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDeath;

    public AudioClip gameOverMusic;

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
}
