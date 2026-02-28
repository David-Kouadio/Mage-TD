using UnityEngine;

public class scr_Bullet : MonoBehaviour
{

    [Header("Settings")]
    public float lifetime = 1;

    private void Awake()
    {
        Destroy(gameObject, lifetime);
    }
}
