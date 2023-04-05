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

    public override void OnStartTurn()
    {
        base.OnStartTurn();
        Debug.Log("Burning start turn");
        Debug.Log("My duration: " + RemainingDuration);
    }

    public override void OnTakeTurn()
    {
        base.OnTakeTurn();
        Debug.Log("Burning take turn");
    }
    public override void OnAttack()
    {
        base.OnAttack();
        Debug.Log("Burning attack");
    }

    public override void OnTakeDamage()
    {
        base.OnTakeDamage();
        Debug.Log("Burning take damage");
    }
}
