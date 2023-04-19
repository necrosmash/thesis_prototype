using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockdownTrait : Trait
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        Name = "Knocked Down";
        Description = "This orc has fallen and can't get up";
        LLMDescription = "This orc was knocked to the ground earlier and momentarily cannot get back up";
        Duration = 3;
        base.Awake();
    }
    protected override void Start()
    {
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

        newGameObject.GetComponent<EnemyController>().skipTurn = true;
    }

    public override void OnTakeTurn(GameObject newGameObject)
    {
        base.OnTakeTurn(newGameObject);
    }
}
