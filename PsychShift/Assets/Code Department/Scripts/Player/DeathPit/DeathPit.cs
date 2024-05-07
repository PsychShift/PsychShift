using UnityEngine;
using Guns.Health;

[RequireComponent(typeof(Collider))]
public class DeathPit : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Collider>().isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(10000000, Guns.GunType.None);
        }
    }
}
