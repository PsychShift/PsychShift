using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultScript : MonoBehaviour
{
    private int vaultLayer;
    public new Camera camera;
    private float playerHeight = 2f;
    private float playerRadius = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        vaultLayer = LayerMask.NameToLayer("VaultLayer");
        vaultLayer = ~vaultLayer;
    }

    // Update is called once per frame
    void Update()
    {
        Vault();
    }
    private void Vault()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out var firstHit, 1f, vaultLayer))
            {
                print("vaultable in front");
                if (Physics.Raycast(firstHit.point + (camera.transform.forward * playerRadius) + (Vector3.up * 0.6f * (playerHeight + 10)), Vector3.down, out var secondHit, (playerHeight + 8)))
                {
                    print("found place to land");
                    StartCoroutine(LerpVault(secondHit.point, 0.5f));
                }
            }
        }

    }
    IEnumerator LerpVault(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
