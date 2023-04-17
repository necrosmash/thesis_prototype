using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcoholTrait : Trait
{
    [SerializeField]
    protected int minDamageRollModifier;
    [SerializeField]
    protected int maxDamageRollModifier;

    [SerializeField]
    protected int minDamageResistRollModifier;
    [SerializeField]
    protected int maxDamageResistRollModifier;

    protected int oldMinDamageRoll;
    protected int oldMaxDamageRoll;

    protected int oldMinDamageResistRoll;
    protected int oldMaxDamageResistRoll;

    // Start is called before the first frame update
    protected override void Awake()
    {
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
