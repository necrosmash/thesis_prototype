using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : GamePiece
{
    public BattleInfo.Orc orc;

    override protected void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override protected void Update()
    {

    }

    public override void TakeTurn()
    {
        Debug.Log("enemy taking turn");
        base.TakeTurn();
    }
}
