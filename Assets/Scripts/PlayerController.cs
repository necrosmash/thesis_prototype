using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerController : GamePiece
{

    override protected void Awake(){
        base.Awake();
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override protected void Update()
    {
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

        if (tempNextTile != currentTile){
            if (checkMoveLegal(tempNextTile)){
                movePiece(tempNextTile);
            }
        }

    }

    bool checkMoveLegal(Vector3Int newTile){

        if (tilemap.HasTile(newTile)){
            return true;
        }

        return false;

    }

}
