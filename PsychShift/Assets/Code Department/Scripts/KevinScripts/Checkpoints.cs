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

    // Public field to drag the spawn position object in Unity Editor
    public Transform spawnPositionObject;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player" && !hitCheck)
        {
            Debug.Log("Checkpoint hit");
            hitCheck = true;
            PlayerMaster.Instance.SetCheckPoint(transform);
            StartCoroutine(checkPointReach());
        }
    }

    IEnumerator checkPointReach()
    {
        checkpointUI.SetActive(true);
        audioSource.PlayOneShot(audioClip);

        // Use the public spawn position object or fallback to respawnPoint
        Transform spawnTransform = spawnPositionObject != null ? spawnPositionObject : respawnPoint;
        Vector3 spawnPosition = spawnTransform.position;

        Instantiate(checkpoint, spawnPosition - new Vector3(0, 10, 0), Quaternion.FromToRotation(Vector3.up, spawnPosition));
    
        yield return new WaitForSeconds(2);
        checkpointUI.SetActive(false);
    }
}