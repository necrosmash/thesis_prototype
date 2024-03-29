using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrelTrait : Trait
{

    [SerializeField]
    int fuseTime;
    [SerializeField]
    int explosionRadius;
    [SerializeField]
    GameObject explosion;

    private ThesisChatController cc;

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
        cc = GameObject.Find("Canvas/Log/Chat Controller").GetComponent<ThesisChatController>();

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
                cc.AddToChatOutput("Barrel explodes!");
                Explode(newGameObject.GetComponent<GamePiece>().currentTile);
                gameManager.Kill(newGameObject.GetComponent<GamePiece>());

            }
        }


    }

    public override void OnTakeDamage(GameObject newGameObject)
    {
        base.OnTakeDamage(newGameObject);
        if (!isLit)
        {
            AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

            audioManager.PlayDelayed("match", 0.25f);
        }
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
                cc.AddToChatOutput(enemy.orc.name + " is engulfed in flames!");
            }

        }

        foreach (ObstacleController obstacle in gameManager.obstacles)
        {

            // checks to avoid adding traits to itself, and only adding to those in explosion range
            if ((obstacle.currentTile - newTile).magnitude <= explosionRadius && (obstacle.currentTile != newTile))
            {
                gameManager.AddTrait(obstacle, "burning");
            }

        }

        if ((gameManager.player.currentTile - newTile).magnitude <= explosionRadius)
        {

            gameManager.AddTrait(gameManager.player, "burning");
            cc.AddToChatOutput(gameManager.player.gameObject.name + " is engulfed in flames!");
        }

        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        audioManager.Play("explosion");
        audioManager.Play("shatter");
        Instantiate(explosion, this.gameObject.transform.parent.transform.position, Quaternion.identity);

        RemainingDuration = 0;

    }

}
