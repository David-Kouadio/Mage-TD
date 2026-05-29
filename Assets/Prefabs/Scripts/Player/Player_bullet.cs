using System;
using UnityEngine;

public class Player_bullet : MonoBehaviour
{
    public int bulletDamage;

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
        else if (objectWeHit.gameObject.CompareTag("Ground"))
        {
            Debug.Log("hit the ground!");
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }       
        else if (objectWeHit.gameObject.CompareTag("ExplosiveTarget"))
        {
            Debug.Log("hit ExplosiveTarget!");
            objectWeHit.gameObject.GetComponent<SphereTarget>().Shatter();
            
        }
        else if (objectWeHit.gameObject.CompareTag("Enemy"))
        { 
            objectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);

            CreateBloodSprayEffect(objectWeHit);

            Destroy(gameObject);
        }
    }

    private void CreateBloodSprayEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(
            GlobalReferences.Instance.bloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)

        );

        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
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
