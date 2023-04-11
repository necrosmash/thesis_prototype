using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public List<Trait> traits;


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

        Trait traitToRemove = null;

        do
        {
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].RemainingDuration <= 0)
                {
                    traitToRemove = traits[i];
                    break;
                }
            }

            if (traitToRemove != null)
            {
                traits.Remove(traitToRemove);
                Destroy(traitToRemove.gameObject);
                traitToRemove = null;
            }

        } while (traitToRemove != null);


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

    protected virtual void Attack()
    {

        foreach (Trait trait in traits)
        {

            trait.OnAttack();

        }

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

    public virtual void TakeDamage()
    {
        foreach (Trait trait in traits)
        {

            trait.OnTakeDamage();

        }
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
