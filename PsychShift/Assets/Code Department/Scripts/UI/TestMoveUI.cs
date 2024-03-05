using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TestMoveUI : MonoBehaviour
{
    //Image Display
    [SerializeField] GameObject playerImageMatch;
 
    //Action buttons
    [SerializeField] GameObject checkFalseButton;
    [SerializeField] GameObject checkTrueButton;
 
    //Random image generator
    int playerSpriteInt = 1;
    [SerializeField] Sprite Sprite1;
    [SerializeField] Sprite Sprite2;
    [SerializeField] Sprite Sprite3;
 
    //Animated match movement
    [SerializeField] GameObject playerImagePrefab;
    [SerializeField] GameObject mainCanvasObj;
 
 
    public void CheckTrue()
    {
        //Moving image animation after swipe
        StartCoroutine(SwipeAnimatedObject(1.0f, playerSpriteInt));
        NewPlayerInt();
    }
 
    public void CheckFalse()
    {
        //Moving image animation after swipe
        StartCoroutine(SwipeAnimatedObject(-1.0f, playerSpriteInt));
        NewPlayerInt();
    }
 
    void NewPlayerInt()
    {
        playerSpriteInt = Random.Range(1, 4);
    }
 
    //Used to instantiate and move a symbol
    IEnumerator SwipeAnimatedObject(float direction, int symbol)
    {
        //Original symbol on playfield
        RectTransform playerSymbolTransform = playerImageMatch.GetComponent<RectTransform>();
 
        //Location of original playfield symmbol
        Vector3 spawnLocation = new Vector3(playerSymbolTransform.localPosition.x, playerSymbolTransform.localPosition.y, playerSymbolTransform.localPosition.z);
 
        //Spawn flying object
        GameObject flyingSymbol = Instantiate(playerImagePrefab, spawnLocation, Quaternion.identity);
        flyingSymbol.transform.SetParent(mainCanvasObj.transform, false);
 
        //Flying symbol rect transform
        RectTransform flyingSymbolTransform = flyingSymbol.GetComponent<RectTransform>();
        Vector3 flyingSymbolLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
 
        flyingSymbolTransform.localPosition = flyingSymbolLocation;
 
        //Get flying symbol image component
        Image flyingSymbolImage = flyingSymbol.GetComponent<Image>();
 
        //Set flying object sprite
        if (symbol == 1)
        {
            flyingSymbolImage.sprite = Sprite1;
        }
        else if (symbol == 2)
        {
            flyingSymbolImage.sprite = Sprite2;
        }
        else
        {
            flyingSymbolImage.sprite = Sprite3;
        }
 
        //Button icons rect transform
        RectTransform falseButtonTransform = checkFalseButton.GetComponent<RectTransform>();
        RectTransform trueButtonTransform = checkTrueButton.GetComponent<RectTransform>();
 
        //Find max/min boundary sprite can move based on direction  of movement
        Vector3 maxMinLocation;
        if (direction < 0)
        {
            maxMinLocation = new Vector3(falseButtonTransform.localPosition.x, falseButtonTransform.localPosition.y, falseButtonTransform.localPosition.z);
        }
        else
        {
            maxMinLocation = new Vector3(trueButtonTransform.localPosition.x, trueButtonTransform.localPosition.y, trueButtonTransform.localPosition.z);
        }
 
        //Move symbol
        float speed = (Mathf.Abs(flyingSymbolLocation.x - maxMinLocation.x)) / .5f;
        if (direction < 0)
        {
            while (flyingSymbolLocation.x > maxMinLocation.x)
            {
                flyingSymbolLocation.x = flyingSymbolLocation.x + direction * speed * Time.deltaTime;
                flyingSymbolTransform.localPosition = flyingSymbolLocation;
                yield return null;
            }
        }
        else
        {
            while (flyingSymbolLocation.x < maxMinLocation.x)
            {
                flyingSymbolLocation.x = flyingSymbolLocation.x + direction * speed * Time.deltaTime;
                flyingSymbolTransform.localPosition = flyingSymbolLocation;
                yield return null;
            }
        }
 
        //Destroy symbol once it has moved
        Destroy(flyingSymbol);
    }
}
