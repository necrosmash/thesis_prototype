using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : GamePiece
{
    // Currently a stub, but in the future can be used similarly to EnemyController
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

    public override void StartTurn()
    {
        base.StartTurn();
    }

    public override void TakeTurn() {

        base.TakeTurn();
        
    }

    public override void TakeDamage(int damage = 10)
    {
        base.TakeDamage(damage);
    }


}
