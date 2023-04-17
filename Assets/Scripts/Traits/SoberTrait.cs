using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoberTrait : Trait
{
    [SerializeField]
    int minDamageRollModifier;
    [SerializeField]
    int maxDamageRollModifier;

    [SerializeField]
    int minDamageResistRollModifier;
    [SerializeField]
    int maxDamageResistRollModifier;

    int oldMinDamageRoll;
    int oldMaxDamageRoll;

    int oldMinDamageResistRoll;
    int oldMaxDamageResistRoll;

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
        GamePiece tempGamePiece = transform.parent.GetComponent<GamePiece>();

        oldMinDamageRoll = tempGamePiece.minDamageRoll;
        oldMaxDamageRoll = tempGamePiece.maxDamageRoll;

        oldMinDamageResistRoll = tempGamePiece.minDamageResistRoll;
        oldMaxDamageResistRoll = tempGamePiece.maxDamageResistRoll;

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void OnStartTurn(GameObject newGameObject)
    {
        GamePiece tempGamePiece = newGameObject.GetComponent<GamePiece>();

        tempGamePiece.minDamageRoll += minDamageRollModifier;
        tempGamePiece.maxDamageRoll += maxDamageRollModifier;


        base.OnStartTurn(newGameObject);
    }

    public override void OnTakeTurn(GameObject newGameObject)
    {

        GamePiece tempGamePiece = newGameObject.GetComponent<GamePiece>();

        tempGamePiece.minDamageRoll = oldMinDamageRoll;
        tempGamePiece.maxDamageRoll = oldMaxDamageRoll;

        tempGamePiece.minDamageResistRoll = oldMinDamageResistRoll;
        tempGamePiece.maxDamageResistRoll = oldMaxDamageResistRoll;

        base.OnTakeTurn(newGameObject);
    }
    public override void OnAttack(GameObject newGameObject)
    {
        base.OnAttack(newGameObject);
    }

    public override void OnTakeDamage(GameObject newGameObject)
    {
        base.OnTakeDamage(newGameObject);

        GamePiece tempGamePiece = newGameObject.GetComponent<GamePiece>();

        tempGamePiece.minDamageResistRoll += minDamageResistRollModifier;
        tempGamePiece.maxDamageResistRoll += maxDamageResistRollModifier;

    }
}
