using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BurgerDespawn : MonoBehaviour
{
    private int despawnTimer= 3;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(despawn());
    }
    IEnumerator despawn()
    {
         yield return new WaitForSeconds(despawnTimer);
         Destroy(gameObject);
    }
}
