using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class PlayerController : GamePiece
{
    private bool isPlayerTurn;
    private int moveCount;

    [SerializeField]
    private int movesPerTurn;

    override protected void Awake(){
        base.Awake();
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        moveCount = 0;
        isPlayerTurn = false;
    }

    // Update is called once per frame
    override protected void Update()
    {
        if (MenuCanvas.IsRendered || !isPlayerTurn) return;

        Vector3Int tempNextTile = currentTile;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            tempNextTile.y += 1;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tempNextTile.y -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tempNextTile.x -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tempNextTile.x += 1;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            isPlayerTurn = false;
            base.TakeTurn();
        }

        if (tempNextTile != currentTile){
            if (checkMoveLegal(tempNextTile)){
                moveCount--;
                movePiece(tempNextTile);
            }
        }

    }

    bool checkMoveLegal(Vector3Int newTile) {

        if (tilemap.HasTile(newTile) && moveCount > 0 && gameManager.getPieceAtTile(newTile) == null)
        {
            return true;
        }

        return false;

    }

    public override void TakeTurn()
    {
        Debug.Log("player taking turn");
        isPlayerTurn = true;
        moveCount = movesPerTurn;
        //base.TakeTurn();
    }

}
