using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class PlayerMovement : MonoBehaviour
{

    public Grid grid;
    public Tilemap tilemap;

    Vector3Int startingTile = new Vector3Int(0, 0, 0);
    Vector3Int currentTile;

    // Start is called before the first frame update
    void Start()
    {

        movePlayer(startingTile);

    }

    // Update is called once per frame
    void Update()
    {
        if (MenuCanvas.IsRendered) return;

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
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Scenes/MenuScene", LoadSceneMode.Additive);
        }

        if (tempNextTile != currentTile){
            if (checkMoveLegal(tempNextTile)){
                movePlayer(tempNextTile);
            }
        }

    }

    bool checkMoveLegal(Vector3Int newTile){

        if (tilemap.HasTile(newTile)){
            return true;
        }

        return false;

    }

    void movePlayer(Vector3Int newTile){
        transform.position = grid.GetCellCenterWorld(newTile);
        currentTile = newTile;
    }

}
