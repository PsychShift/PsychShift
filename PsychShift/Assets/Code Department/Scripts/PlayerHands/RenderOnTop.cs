using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOnTop : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.renderQueue = 4000;
    }

}
