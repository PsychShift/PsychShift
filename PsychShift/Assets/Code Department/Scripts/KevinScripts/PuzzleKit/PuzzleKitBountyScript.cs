using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleKitBountyScript : PuzzleKit
{
    [SerializeField]
    public Guns.Health.EnemyHealth enemyTarget;
    // Start is called before the first frame update
    void Start()
    {
        doNotReact = true;
        enemyTarget.OnDeath += ThisActivate;
    }
    public override void ThisActivate()
    {
        base.ThisActivate();
    }

    // Update is called once per frame
    private void OnDisable()
    {
        enemyTarget.OnDeath -= ThisActivate;
    }
}
