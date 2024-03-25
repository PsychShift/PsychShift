using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleKitBountyScript : PuzzleKit
{
    [SerializeField]
    public Guns.Health.EnemyHealth enemyTarget;
    public GameObject damageGO;
    private IDamageable damageable;
    // Start is called before the first frame update
    void Start()
    {
        doNotReact = true;
        if(enemyTarget != null)
            enemyTarget.OnDeath += ThisActivate;
        if(damageGO == null) return;
        if(damageGO.TryGetComponent(out damageable))
            damageable.OnDeath += ThisActivate;
    }
    public override void ThisActivate()
    {
        base.ThisActivate();
    }

    // Update is called once per frame
    private void OnDisable()
    {
        if(enemyTarget != null)
            enemyTarget.OnDeath -= ThisActivate;
        if(damageable != null)
            damageable.OnDeath -= ThisActivate;
    }
}
