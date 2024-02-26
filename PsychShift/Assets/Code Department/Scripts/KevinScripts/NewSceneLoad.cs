using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewSceneLoad : MonoBehaviour
{
    public bool trevor;
    public bool noUSEyet;
    public PlayerStateMachine resetCheckpoint;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            if(trevor)
            {
                StartCoroutine(Test());
                //PlayerMaster.Instance.StartNew();
                //SceneManager.LoadScene("TREVOR ROOM");
            }
                
        }
    }

    private IEnumerator Test()
    {
        PlayerMaster.Instance.StartNew();
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("TREVOR ROOM");
    }
}
