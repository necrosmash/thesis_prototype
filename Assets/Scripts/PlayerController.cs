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


        int tempVoiceInt = UnityEngine.Random.Range(0, 2);

        switch (tempVoiceInt)
        {
            case 0:
                {
                    VoiceType = "voiceplayermale";
                    break;
                }
            case 1:
                {
                    VoiceType = "voiceplayerfemale";
                    break;
                }
        }

        AttackSound = "sword";

        DamageSound = "playerdamage";

    }

    // Update is called once per frame
    override protected void Update()
    {
        if (MenuCanvas.IsRendered || !isPlayerTurn || OpenAiApi.isPostInProgress) return;

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
                GamePiece tempGamePiece = gameManager.GetPieceAtTile(gameManager.SelectedTile);

                if (((tempGamePiece.currentTile - currentTile).magnitude <= attackRadius) && (tempGamePiece != null))
                {
                    Attack(tempGamePiece);
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
                audioManager.Play("walk");
            }
        }

    }

    protected override void Attack(GamePiece newGamePiece)
    {

        base.Attack(newGamePiece);

        audioManager.Play(AttackSound);

    }

    public override void TakeTurn()
    {
        isPlayerTurn = true;
        moveCount = movesPerTurn;
        hasAttacked = false;
    }
}
