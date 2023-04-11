using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class PlayerController : GamePiece
{
    bool isPlayerTurn;
    bool hasAttacked;
    int moveCount;

    [SerializeField]
    int movesPerTurn;

    [SerializeField]
    int attackRadius = 1;

    override protected void Awake(){
        base.Awake();
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        moveCount = 0;
        isPlayerTurn = false;
        hasAttacked = false;
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
            if (!hasAttacked && moveCount > 0)
            {
                if (Attack(gameManager.SelectedTile))
                {
                    moveCount--;
                    hasAttacked = true;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isPlayerTurn = false;
            base.TakeTurn();
        }

        if (tempNextTile != currentTile){
            if (checkMoveLegal(tempNextTile) && moveCount > 0) {
                moveCount--;
                movePiece(tempNextTile);
            }
        }

    }

    bool Attack(Vector3Int newTile)
    {
        GamePiece tempGamePiece = gameManager.GetPieceAtTile(newTile);

        if (gameManager.GetPieceAtTile(newTile) is EnemyController && ((newTile - currentTile).magnitude <= attackRadius))
        {
            EnemyController newEnemy = (EnemyController) tempGamePiece;
            newEnemy.TakeDamage();
            return true;

        }
        return false;

    }

    public override void TakeTurn()
    {
        isPlayerTurn = true;
        moveCount = movesPerTurn;
        hasAttacked = false;
        //base.TakeTurn();
    }
}
