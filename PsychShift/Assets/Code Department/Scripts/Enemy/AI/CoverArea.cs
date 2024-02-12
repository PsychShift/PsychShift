using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoverArea : MonoBehaviour
{
    [SerializeField]private LayerMask coverMask;
    private static CoverArea instance;
    public static CoverArea Instance
    {
        get
        {
            return instance;
        }
    }


    private List<Cover> allCover = new();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        allCover.Clear();
        allCover.AddRange(FindObjectsOfType<Cover>());
    }
    
    public Cover GetCover(Transform player)
    {
        Cover cover = null;
        // perform a sphere cast to find the nearest cover
        // that is not in the line of sight of the target
        RaycastHit[] hits = Physics.SphereCastAll(player.position, 30f, Vector3.up, 0f, coverMask);

        // pick randomly from the list of covers where hit != null && a line check can't hit the player
        cover = hits.Where(hit => hit.collider.GetComponent<Cover>() != null && !Physics.Linecast(player.position, hit.point, coverMask))
                    .Select(hit => hit.collider.GetComponent<Cover>())
                    .OrderBy(_ => Random.value)
                    .FirstOrDefault().GetComponent<Cover>();

        return cover;
    }

    public bool CoverIsAvailable()
    {
        // Do the same sphere cast as above, but this time check if the cover is available
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 300f, Vector3.up, 0f, coverMask);
        return hits.Where(hit => hit.collider.GetComponent<Cover>() != null && hit.collider.GetComponent<Cover>())
                    .Select(hit => hit.collider.GetComponent<Cover>())
                    .OrderBy(_ => Random.value)
                    .FirstOrDefault() != null;
    }
}
