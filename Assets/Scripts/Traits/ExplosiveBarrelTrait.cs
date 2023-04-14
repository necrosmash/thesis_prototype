using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrelTrait : Trait
{

    [SerializeField]
    int fuseTime;
    [SerializeField]
    int explosionRadius;

    bool isLit = false;

    int leftUntilExplosion;

    // Start is called before the first frame update
    protected override void Start()
    {
        Name = "Explosive barrel";
        Description = "This is not up to workplace safety regulations";
        Duration = 10000;
        base.Start();

        leftUntilExplosion = fuseTime;

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

        if (isLit)
        {
            leftUntilExplosion -= 1;

            if (leftUntilExplosion <= 0)
            {
                Explode(newGameObject.GetComponent<GamePiece>().currentTile);
            }
        }


    }
    public override void OnAttack()
    {
        base.OnAttack();
    }

    public override void OnTakeDamage()
    {
        base.OnTakeDamage();
        isLit = true;
    }

    void Explode(Vector3Int newTile)
    {

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        foreach (EnemyController enemy in gameManager.enemies)
        {

            if ((enemy.currentTile - newTile).magnitude <= explosionRadius)
            {
                gameManager.AddTrait(enemy, "burning");
            }

        }

        foreach (ObstacleController obstacle in gameManager.obstacles)
        {

            if ((obstacle.currentTile - newTile).magnitude <= explosionRadius)
            {
                // checks to avoid adding traits to itself
                if (obstacle.currentTile != newTile)
                {
                    gameManager.AddTrait(obstacle, "burning");
                }
            }

        }

        if ((gameManager.player.currentTile - newTile).magnitude <= explosionRadius)
        {

            gameManager.AddTrait(gameManager.player, "burning");

        }

        gameManager.Kill(gameManager.GetPieceAtTile(newTile));
        RemainingDuration = 0;

    }

}
