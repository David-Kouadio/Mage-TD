using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isReflected = false;
    public int reflectedDamage = 25;

    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;

        if (!isReflected)
        {
            if (hitTransform.CompareTag("Player"))
            {
                var health = hitTransform.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(10);
                }
                Destroy(gameObject);
            }
            else if (!hitTransform.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (hitTransform.CompareTag("Enemy"))
            {
                Enemy enemy = hitTransform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(reflectedDamage);
                }
                Destroy(gameObject);
            }
            else if (!hitTransform.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}
