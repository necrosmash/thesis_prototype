using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mouseCapture : MonoBehaviour
{
    [SerializeField]
    Grid grid;
    [SerializeField]
    Tilemap tilemap;
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    MobDisplayController mobDisplayController;

    Vector3Int currentTile;

    [SerializeField]
    TileBase defaultTile;
    [SerializeField]
    TileBase selectedTile;
    [SerializeField]
    TileBase radiusTile;

    // Start is called before the first frame update
    void Start()
    {

        gameManager.SelectedTile = new Vector3Int(-1, -1, -1);

    }

    // Update is called once per frame
    void Update()
    {
        if (OpenAiApi.isPostInProgress) return;

        //if (!(gameManager.GetPieceAtTile(currentTile) is EnemyController))
        //{
        //}

        if (Input.GetMouseButtonDown(0)) {

            Vector3 mousePos = Input.mousePosition;
            // If the camera is Perspective rather than Orthographic, this needs to be included
            // Yeah, I don't really know what this does, only kinda. Found it on the internet a while back.
            // Something to do with how geometry and vectors work
            if (!Camera.main.orthographic)
            {
                mousePos.z = Camera.main.transform.position.y;
            }

            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3Int posInt = grid.WorldToCell(pos);

            //Shows the cell reference for the grid
            Debug.Log(posInt);

            // Reset all previous stuff to defaultTile
            ClearCurrentRange();

            if (tilemap.HasTile(posInt))
            {

                // Setting selected tile to selectedTile and if its an enemy, set display the attack radius with radiusTile
                currentTile = posInt;

                GamePiece tempGamePiece = gameManager.GetPieceAtTile(currentTile);

                if (tempGamePiece is EnemyController)
                {

                    int enemyAttackRadius = (int)((EnemyController)tempGamePiece).attackRadius;

                    SetTileRange(currentTile, true, enemyAttackRadius, radiusTile);

                    SetTileRange(currentTile, false, enemyAttackRadius, radiusTile);

                }

                tilemap.SetTile(currentTile, selectedTile);
                Debug.Log(tilemap.GetTile(currentTile));

                gameManager.SelectedTile = posInt;


            }


       }
    }


    public void SetTileRange(Vector3Int initialPoint, bool xAxis, int range, TileBase newTile)
    {
        for (int i = 0; i <= range * 2; i++)
        {

            Vector3Int tempTile = xAxis ? new Vector3Int(initialPoint.x - range + i, currentTile.y, currentTile.z) : new Vector3Int(initialPoint.x, currentTile.y - range + i, currentTile.z);

            if (tilemap.HasTile(tempTile))
            {
                tilemap.SetTile(tempTile, newTile);
            }

        }
    }

    public void ClearCurrentRange()
    {

        SetTileRange(currentTile, true, 20, defaultTile);

        SetTileRange(currentTile, false, 20, defaultTile);

    }


}
