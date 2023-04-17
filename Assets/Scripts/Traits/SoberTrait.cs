using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoberTrait : AlcoholTrait
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        Name = "Sober";
        Description = "This orc is full of anxiety and existential dread";
        Duration = 10000;
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
    }

    public override void OnTakeTurn(GameObject newGameObject)
    {
        base.OnTakeTurn(newGameObject);
    }
    public override void OnAttack(GameObject newGameObject)
    {
        base.OnAttack(newGameObject);
    }

    public override void OnTakeDamage(GameObject newGameObject)
    {
        base.OnTakeDamage(newGameObject);
    }
}
