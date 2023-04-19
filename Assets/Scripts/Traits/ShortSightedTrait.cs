using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSightedTrait : Trait
{
    // Start is called before the first frame update

    [SerializeField]
    int sightRangeModified;

    int oldSightRange;

    protected override void Awake()
    {
        Name = "Short-sighted";
        Description = "This orc can't C#";
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

        EnemyController tempEnemyController = newGameObject.GetComponent<EnemyController>();

        oldSightRange = tempEnemyController.sightRange;

        tempEnemyController.sightRange = sightRangeModified;

    }

    public override void OnTakeTurn(GameObject newGameObject)
    {

        base.OnTakeTurn(newGameObject);

        //EnemyController tempEnemyController = newGameObject.GetComponent<EnemyController>();

        //tempEnemyController.sightRange = oldSightRange;

    }
}
