using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerDespawn : MonoBehaviour
{
    private int despawnTimer= 2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(despawn());
    }
    IEnumerator despawn()
    {
         yield return new WaitForSeconds(despawnTimer);
    }
}
