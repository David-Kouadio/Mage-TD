using UnityEngine;

public class Player_bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hit " + collision.gameObject.name + " !");
            Destroy(gameObject);
        }
    }

}
