using UnityEngine;

public class Player_bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hit " + objectWeHit.gameObject.name + " !");
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }
        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            Debug.Log("hit a wall!");
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }
        if (objectWeHit.gameObject.CompareTag("Ground"))
        {
            Debug.Log("hit the ground!");
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }
        if (objectWeHit.gameObject.CompareTag("ExplosiveTarget"))
        {
            Debug.Log("hit ExplosiveTarget!");
            objectWeHit.gameObject.GetComponent<SphereTarget>().Shatter();
        }
    }

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactBulletPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)

        );

        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }

}
