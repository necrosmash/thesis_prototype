using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasilyDistractedTrait : Trait
{

    float skipChance = 20f;
    protected override void Awake()
    {
        Name = "Easily Distracted";
        Description = "This orc is easily distracted, so they... Is that a squirrel on that tree? So cute!";
        Duration = 10000;
        base.Awake();
    }
    // Start is called before the first frame update
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
        float chanceToSkip = Random.Range(0f, 100f);
        
        if (chanceToSkip <= skipChance)
        {
            newGameObject.GetComponent<EnemyController>().skipTurn = true;
        }
    }

    public override void OnTakeTurn(GameObject newGameObject)
    {
        base.OnTakeTurn(newGameObject);
    }
}
