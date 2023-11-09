using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverArea : MonoBehaviour
{
    private LayerMask coverMask = LayerMask.NameToLayer("Cover");
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
        allCover.AddRange(FindObjectsOfType<Cover>());
    }
    
    public Cover GetCover(Vector3 pos, Transform target)
    {
        Cover cover = null;
        // perform a sphere cast to find the nearest cover
        // that is not in the line of sight of the target
        RaycastHit[] hits = Physics.SphereCastAll(pos, 15f, Vector3.up, 0f, coverMask);

        foreach(var hit in hits)
        {
            cover = hit.collider.GetComponent<Cover>();
            if (cover == null)
                continue;
            if (Physics.Linecast(cover.transform.position, target.position))
                continue;
            return cover;
        }
        return cover;
    }
}
