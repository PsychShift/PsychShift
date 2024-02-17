using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public Transform player, destination;
 public GameObject playerg;
 
 /* void OnTriggerEnter(Collider other){
  if(other.CompareTag("Player")){
   playerg.SetActive(false);
   player.position = destination.position;
   playerg.SetActive(true);
  }
 } */

 /* void OnTriggerEnter(Collider other)
 {
    if(other.CompareTag("Player"))
    {
        CharacterController playerC=other.GetComponent<CharacterController>();
        playerC.enabled = false;
        GameObject playerObj = other.GetComponent<GameObject>();
        Transform playerPosition = playerObj.GetComponent<Transform>();
        playerPosition = destination;
        playerC.enabled = true;
        gameObject.SetActive(false);  
    }
 } */

 void OnTriggerEnter(Collider other)
{
    if(other.CompareTag("Player"))
    {
        CharacterController playerC = other.GetComponent<CharacterController>();
        if (playerC != null)
        {
            playerC.enabled = false;
            // Assuming 'destination' is a Vector3 defined elsewhere in your script
            other.transform.position = destination.position;
            playerC.enabled = true;
        }
        // If you want to deactivate the teleporter after use, uncomment the line below
        // gameObject.SetActive(false);
    }
}
}