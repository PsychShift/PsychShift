using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandsOrientation
{
    public GameObject leftHand;
    public GameObject leftElbow;
    public GameObject rightHand;
    public GameObject rightElbow;

    public override string ToString()
    {
        return $"Left Hand: {leftHand.name}\nLeft Elbow: {leftElbow.name}\nRight Hand: {rightHand.name}\nRight Elbow: {rightElbow.name}";
    }
}
public class SetHands : MonoBehaviour
{
    public List<HandsOrientation> hands;
}