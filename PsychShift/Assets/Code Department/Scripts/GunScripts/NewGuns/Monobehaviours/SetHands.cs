using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandsOrientation
{
    public GameObject leftHand;
    public GameObject rightHand;
}
public class SetHands : MonoBehaviour
{
    public List<HandsOrientation> hands;
}