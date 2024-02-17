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

 void OnTriggerEnter(Collider other)
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
 }
}