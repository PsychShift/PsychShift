using UnityEngine;
using System.Collections;

public class TestBreakObjectCode : MonoBehaviour
{
    public GameObject fractured;
    public GameObject objectToDestroy; // Add a public GameObject variable to specify the object to destroy
    public float breakForce;
    private bool Test = true;

    private bool isDestroyed = false;



    public void BreakTheThing()
    {
        GameObject frac = Instantiate(fractured, transform.position, transform.rotation);

        foreach (Rigidbody rb in frac.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (rb.transform.position - transform.position).normalized * breakForce;
            rb.AddForce(force);
        }

        // Destroy the specified object along with the original GameObject
        if (objectToDestroy != null){
            Destroy(objectToDestroy);
            //Test = false;
        }

        // Wait for 5 seconds and then destroy the instantiated object
        StartCoroutine(DestroyAfterDelay(frac, 5.0f));
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        Destroy(obj);
    }
}