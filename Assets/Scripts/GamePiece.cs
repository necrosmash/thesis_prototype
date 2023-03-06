using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GamePiece : MonoBehaviour
{

    public Vector3Int startingTile;
    public Vector3Int currentTile {get; protected set;}

    protected GameManager gameManager;
    protected Grid grid;
    protected Tilemap tilemap;


    protected virtual void Awake(){

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        grid = gameManager.grid;
        tilemap = gameManager.tilemap;

        if (startingTile == null){
            startingTile = new Vector3Int(0, 0, 0);
        }

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        movePiece(startingTile);
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    protected virtual void movePiece(Vector3Int newTile){

        transform.position = grid.GetCellCenterWorld(newTile);
        currentTile = newTile;

    }
}
