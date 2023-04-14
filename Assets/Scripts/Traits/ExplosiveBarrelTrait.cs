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

    protected override void Awake()
    {
        Name = "Explosive barrel";
        Description = "This is not up to workplace safety regulations";
        Duration = 10000;
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
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

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (isLit)
        {

            leftUntilExplosion -= 1;

            if (leftUntilExplosion <= 0)
            {
                Explode(newGameObject.GetComponent<GamePiece>().currentTile);
                gameManager.Kill(newGameObject.GetComponent<GamePiece>());

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
        //if (!isLit)
        //{
        //    GameObject.Find("GameManager").GetComponent<GameManager>().AddTrait(this.gameObject.transform.parent.GetComponent<GamePiece>(), "burning");
        //}
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

            if ((obstacle.currentTile - newTile).magnitude <= explosionRadius && (obstacle.currentTile - newTile).magnitude > 0.1)
            {
                // checks to avoid adding traits to itself
                gameManager.AddTrait(obstacle, "burning");
            }

        }

        if ((gameManager.player.currentTile - newTile).magnitude <= explosionRadius)
        {

            gameManager.AddTrait(gameManager.player, "burning");

        }

        //gameManager.Kill(gameManager.GetPieceAtTile(newTile));
        RemainingDuration = 0;

    }

}
