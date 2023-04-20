using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningTrait : Trait
{

    [SerializeField]
    int damagePerTurn;
    // Start is called before the first frame update
    protected override void Awake()
    {
        Name = "Burning";
        Description = "This orc is really hot";
        LLMDescription = "This orc is on fire because, earlier, they were near an explosive barrel when it exploded";
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
    }

    public override void OnTakeTurn(GameObject newGameObject)
    {
        base.OnTakeTurn(newGameObject);

        newGameObject.GetComponent<GamePiece>().TakeDamage(damagePerTurn);

    }
}
