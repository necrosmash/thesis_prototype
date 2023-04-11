using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkTrait : Trait
{
    // Start is called before the first frame update
    protected override void Start()
    {
        Name = "Drunk";
        Description = "This orc came to the battle prepared";
        Duration = 3;
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void OnStartTurn(GameObject newGameObject)
    {
        base.OnStartTurn(newGameObject);

        EnemyController ec = newGameObject.GetComponent<EnemyController>();

        if (ec.orc.weaponEnum == BattleInfo.Orc.Weapon.Bow)
        {
            ec.attackRadius = 1f;
        }

        Debug.Log("Drunk start turn");
        Debug.Log("My duration: " + RemainingDuration);
    }

    public override void OnTakeTurn(GameObject newGameObject)
    {
        base.OnTakeTurn(newGameObject);

        EnemyController ec = newGameObject.GetComponent<EnemyController>();


        if (ec.orc.weaponEnum == BattleInfo.Orc.Weapon.Bow)
        {
            ec.attackRadius = 3f;
        }

        Debug.Log("Drunk take turn");
    }
    public override void OnAttack()
    {
        base.OnAttack();
        Debug.Log("Drunk attack");
    }

    public override void OnTakeDamage()
    {
        base.OnTakeDamage();
        Debug.Log("Drunk take damage");
    }
}
