using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GamePiece : MonoBehaviour
{

    protected Vector3Int startingTile;
    public Vector3Int currentTile {get; protected set;}

    // This is a gibberish value required to check if the piece was initialised
    Vector3Int DEFAULT_STARTING_VALUE = new Vector3Int(-1, -1, -1);

    protected GameManager gameManager;
    protected Grid grid;
    protected Tilemap tilemap;

    public int health, maxHealth = -1;
    public List<Trait> traits;

    [SerializeField]
    public int baseDamage;
    [SerializeField]
    public int minDamageRoll;
    [SerializeField]
    public int maxDamageRoll;

    public int minDamageResistRoll = 0;
    public int maxDamageResistRoll = 0;

    public bool skipAttack = false;

    protected virtual void Awake(){

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        grid = gameManager.grid;
        tilemap = gameManager.tilemap;
        
        currentTile = DEFAULT_STARTING_VALUE;

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public virtual void TakeTurn()
    {


        foreach (Trait trait in traits)
        {

            trait.OnTakeTurn(this.gameObject);

        }

        List<Trait> traitsToRemove = new List<Trait>();
        for (int i = 0; i < traits.Count; i++)
        {
            if (traits[i].RemainingDuration <= 0)
            {
                traitsToRemove.Add(traits[i]);
            }
        }

        foreach (Trait t in traitsToRemove)
        {
            traits.Remove(t);
            Destroy(t.gameObject);
        }


        gameManager.FinishTurn();
    }

    public virtual void StartTurn()
    {
        foreach (Trait trait in traits)
        {
            trait.OnStartTurn(this.gameObject);

        }
    }

    protected virtual void movePiece(Vector3Int newTile){

        transform.position = grid.GetCellCenterWorld(newTile);
        currentTile = newTile;

    }

    protected virtual void Attack(GamePiece newGamePiece)
    {

        foreach (Trait trait in traits)
        {

            trait.OnAttack(this.gameObject);

        }

        if (!skipAttack)
        {
            int newDamage = baseDamage + Random.Range(minDamageRoll, maxDamageRoll + 1);

            Debug.Log("Attacking! My base damage is: " + baseDamage + ". My min roll is: " + minDamageRoll + ". My max roll is: " + maxDamageRoll + ". Total damage output: " + newDamage);

            newGamePiece.TakeDamage(newDamage);
        }
        skipAttack = false;

    }

    public virtual void Initialise(Vector3Int newStartingTile)
    {
        if (currentTile != DEFAULT_STARTING_VALUE)
        {
            throw new BadInitialisationException("Already initialised");
        }

        startingTile = newStartingTile;
        movePiece(startingTile);
    }

    public virtual void TakeDamage(int damage = 10)
    {
        foreach (Trait trait in traits)
        {

            trait.OnTakeDamage(this.gameObject);

        }

        damage -= Random.Range(minDamageResistRoll, maxDamageResistRoll + 1);
        Debug.Log("Taking damage! My min damage resist roll: " + minDamageResistRoll + ". My max damage resist roll: " + maxDamageResistRoll + " Total damage received: " + damage);
        
        if (damage < 0)
        {
            damage = 0;
        }

        health -= damage;

        if (health <= 0)
            gameManager.Kill(this);
    }

    protected bool checkMoveLegal(Vector3Int newTile, GamePiece acceptableGamePiece = null)
    {
        if (!tilemap.HasTile(newTile)) return false;

        if (gameManager.GetPieceAtTile(newTile) == null ||
            gameManager.GetPieceAtTile(newTile).Equals(acceptableGamePiece))
            return true;

        return false;
    }

}

public class BadInitialisationException : System.Exception
{

    public BadInitialisationException() : base() { }
    public BadInitialisationException(string message) : base(message)
    {

    }
}
