using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : GamePiece
{
    public BattleInfo.Orc orc;
    float attackRadius;

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

        if (IsPlayerInAttackDistance())
        {
            gameManager.player.TakeDamage();
        }

        //Debug.Log("enemy taking turn");
        base.TakeTurn();
    }

    public override void Initialise(Vector3Int newStartingTile)
    {
        base.Initialise(newStartingTile);

        switch (orc.weaponEnum)
        {
            case BattleInfo.Orc.Weapon.Bow:
                {
                    attackRadius = 3f;
                    break;
                }
            case BattleInfo.Orc.Weapon.Hammer:
                {
                    attackRadius = 2f;
                    break;
                }
            case BattleInfo.Orc.Weapon.Sword:
                {
                    attackRadius = 1f;
                    break;
                }
            default:
                break;
        }

    }

    bool IsPlayerInAttackDistance()
    {

        Vector3Int distanceToPlayer = gameManager.player.currentTile - currentTile;

        // Checking if the player is in radius and on the same row/column
        if (distanceToPlayer.magnitude <= attackRadius && (gameManager.player.currentTile.x == currentTile.x || gameManager.player.currentTile.y == currentTile.y))
        {

            for (int i = 0; i < (int) distanceToPlayer.magnitude; i++)
            {
                // Checking if any objects on the way are obstacles
                // It takes the unit vector of distance to the player, and checks whether
                //  the piece at (currentTile + (unit vector of distance to the player * steps) is an obstacle

                if (gameManager.GetPieceAtTile(currentTile + ((distanceToPlayer / (int)distanceToPlayer.magnitude) * (i+1))) is ObstacleController)
                {
                    return false;
                }
            }

            return true;

        }

        return false;
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        gameManager.Kill(this);
    }


}
