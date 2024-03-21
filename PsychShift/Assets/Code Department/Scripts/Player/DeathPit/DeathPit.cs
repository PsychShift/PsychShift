using UnityEngine;
using Guns.Health;

[RequireComponent(typeof(Collider))]
public class DeathPit : MonoBehaviour
{
    public Vector3 godModeRespawnPoint;
    void OnEnable()
    {
        GetComponent<Collider>().isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GodModeScript.Instance.GodMode)
            {
                CharacterController charCon = other.GetComponent<CharacterController>();
                charCon.enabled = false;
                other.transform.position = godModeRespawnPoint;
                charCon.enabled = true;
                return;
            }
            other.GetComponent<EnemyHealth>().TakeDamage(10000000, Guns.GunType.None);
        }
    }
}
