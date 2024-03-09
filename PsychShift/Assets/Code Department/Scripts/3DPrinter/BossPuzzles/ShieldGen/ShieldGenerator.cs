using UnityEngine;

public class ShieldGenerator : MonoBehaviour, IDamageable
{
    private Animator anim;
    [SerializeField] private ActivateShield_BossPuzzle shieldScript;
    public ParticleSystem beam;
    private static readonly int activateAnimHash = Animator.StringToHash("Activate");
    private static readonly int deactivateAnimHash = Animator.StringToHash("DeActivate");

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    public ChildCollider[] Colliders;

    public int CurrentHealth => throw new System.NotImplementedException();

    public int MaxHealth => throw new System.NotImplementedException();

    public bool IsWeakPoint => throw new System.NotImplementedException();

    void OnEnable()
    {
        beam.Stop();
        anim = GetComponent<Animator>();

        foreach (var collider in Colliders)
        {
            collider.SetUp(this);
        }
    }
    public void Activate()
    {
        Debug.Log("activate shield generator");
        anim.SetTrigger(activateAnimHash);
    }
    public void Disable()
    {
        Debug.Log("deactivate shield generator");
        anim.SetTrigger(deactivateAnimHash);
    }
    public void Destoyed()
    {
        shieldScript.GeneratorDestroyed(this);
        beam.Stop();
        Destroy(gameObject);
    }

    public void TakeDamage(int Damage)
    {
        Destoyed();
    }
}
