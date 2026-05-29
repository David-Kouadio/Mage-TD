using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTarget : MonoBehaviour
{
    public List<Rigidbody> allParts = new List<Rigidbody>();

    public void Shatter()
    {
        foreach(Rigidbody part in allParts)
        {
            part.isKinematic = false;

            StartCoroutine(DestroyTarget());
        }
    }

    private IEnumerator DestroyTarget()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
