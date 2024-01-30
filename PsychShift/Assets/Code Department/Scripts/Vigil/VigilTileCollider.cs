using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VigilTileCollider : MonoBehaviour
{
    public Vector2Int C
    {
        private get;
        set;
    }


    public Vector2Int GetTileVector2()
    {
        return C;
    }
}
