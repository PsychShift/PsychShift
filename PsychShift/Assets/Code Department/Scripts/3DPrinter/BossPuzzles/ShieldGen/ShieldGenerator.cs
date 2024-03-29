using UnityEngine;

public class ShieldGenerator : MonoBehaviour, IDamageable
{
    public bool isImportant;
    //kevin added this
    [HideInInspector]
    public bool isDead;
    public bool isHitable;
    private Animator anim;
    [SerializeField] private ActivateShield_BossPuzzle shieldScript;
    public ParticleSystem beam;
    private static readonly int activateAnimHash = Animator.StringToHash("Activate");
    private static readonly int deactivateAnimHash = Animator.StringToHash("DeActivate");

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    public ChildCollider[] Colliders;

    public float CurrentHealth => throw new System.NotImplementedException();

    public float MaxHealth => throw new System.NotImplementedException();

    public bool IsWeakPoint => throw new System.NotImplementedException();

    void OnEnable()
    {
        beam.Stop();
        anim = GetComponent<Animator>();

        foreach (var collider in Colliders)
        {
            collider.SetUp(this);
        }
        SetColliderEnabled(false);
    }
    public void Activate()
    {
        isHitable = true;
        anim.SetTrigger(activateAnimHash);
        SetColliderEnabled(true);
    }
    public void Disable()
    {
        isHitable = false;
        anim.SetTrigger(deactivateAnimHash);
        SetColliderEnabled(false);
    }
    public void Destoyed()
    {
        isDead = true;
        shieldScript.GeneratorDestroyed(this, isImportant);
        beam.Stop();
        OnDeath?.Invoke(transform);
    }
    private void SetColliderEnabled(bool condition)
    {
        for(int i = 0; i < Colliders.Length; i++)
            Colliders[i].GetComponent<Collider>().enabled = condition;
    }

    public void TakeDamage(float Damage, Guns.GunType gunType)
    {
        if(!isHitable) return;
        if (gunType == Guns.GunType.None || gunType == Guns.GunType.RocketLauncher)
        {
            Destoyed();
        }
    }
}
