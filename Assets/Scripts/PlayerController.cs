using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class PlayerController : GamePiece
{
    bool isPlayerTurn;
    bool hasAttacked;
    int moveCount;

    HealthDisplay healthDisplay;

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

        // Connecting the health display
        healthDisplay = GameObject.Find("HealthDisplay").GetComponent<HealthDisplay>();

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
            if (!hasAttacked)
            {
                if (Attack(gameManager.selectedTile))
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
            if (IsMoveLegal(tempNextTile)){
                moveCount--;
                movePiece(tempNextTile);
            }
        }

    }

    bool IsMoveLegal(Vector3Int newTile) {

        if (tilemap.HasTile(newTile) && moveCount > 0 && gameManager.GetPieceAtTile(newTile) == null)
        {
            return true;
        }

        return false;

    }

    bool Attack(Vector3Int newTile)
    {
        Debug.Log("Attacking");
        GamePiece tempGamePiece = gameManager.GetPieceAtTile(newTile);

        if (gameManager.GetPieceAtTile(newTile) is EnemyController && ((newTile - currentTile).magnitude <= attackRadius))
        {
            EnemyController newEnemy = (EnemyController) tempGamePiece;
            newEnemy.TakeDamage();
            Debug.Log("Gotcha!");
            return true;

        }
        return false;

    }

    public override void TakeTurn()
    {
        Debug.Log("player taking turn");
        isPlayerTurn = true;
        moveCount = movesPerTurn;
        hasAttacked = false;
        //base.TakeTurn();
    }

    public override void TakeDamage()
    {
        healthDisplay.TakeDamage();
    }

}
