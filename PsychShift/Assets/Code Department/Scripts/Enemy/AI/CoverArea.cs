using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverArea : MonoBehaviour
{
    [SerializeField]private LayerMask coverMask;
    private static CoverArea instance;
    public static CoverArea Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CoverArea>();
            }
            return instance;
        }
    }
    private List<Cover> allCover = new();

    void Awake()
    {
        //coverMask = LayerMask.NameToLayer("Cover");
        allCover.AddRange(FindObjectsOfType<Cover>());
    }
    
    public Cover GetCover(Vector3 pos)
    {
        Cover cover = null;
        // perform a sphere cast to find the nearest cover
        // that is not in the line of sight of the target
        RaycastHit[] hits = Physics.SphereCastAll(pos, 100f, Vector3.up, 0f, coverMask);

        foreach(var hit in hits)
        {
            cover = hit.collider.GetComponent<Cover>();
            if (cover == null)
                continue;
            return cover;
        }
        return cover;
    }
}
