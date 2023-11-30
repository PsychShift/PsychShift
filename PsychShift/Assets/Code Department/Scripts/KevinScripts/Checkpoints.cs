using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;
    private bool hitCheck;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject checkpointUI;
    public ParticleSystem checkpoint;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player"&& !hitCheck)
        {
            Debug.Log("Checkpoint hit");
            hitCheck = true;
            PlayerMaster.Instance.SetCheckPoint(gameObject.transform, other.gameObject);
            StartCoroutine(checkPointReach());
            

            
        }
    }

    IEnumerator checkPointReach()
    {
        checkpointUI.SetActive(true);
        audioSource.PlayOneShot(audioClip);
        Instantiate(checkpoint,transform.position, Quaternion.FromToRotation(Vector3.up, transform.position));
        yield return new WaitForSeconds(2);
        checkpointUI.SetActive(false);
    }


}
