using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardDoor : MonoBehaviour
{
    // Start is called before the first frame update
    private MeshRenderer meshStuff;
    public Material locked;
    public Material opened;
    private void Awake() {
        //locked = this.GetComponent<Material>();
        meshStuff = this.GetComponent<MeshRenderer>();
    }
    public void OpenDaNoor()
    {
        this.GetComponent<Collider>().enabled = false;
        meshStuff.material = opened;
        
        Debug.Log("OPen na door");
        StartCoroutine(CloseDaNoor());
    }
    IEnumerator CloseDaNoor()
    {
        yield return new WaitForSeconds(3);
        this.GetComponent<Collider>().enabled = true;
        meshStuff.material = locked;
    }
}
