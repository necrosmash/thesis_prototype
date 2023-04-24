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

    // Start is called before the first frame update
    void Start()
    {

        gameManager.SelectedTile = new Vector3Int(-1, -1, -1);

    }

    // Update is called once per frame
    void Update()
    {
        if (OpenAiApi.isPostInProgress) return;
        
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

            if (tilemap.HasTile(posInt))
            {

                if (currentTile != null)
                {
                    Vector3Int oldTile = currentTile;
                    //tilemap.SetTileFlags(oldTile, TileFlags.None);
                    tilemap.SetTile(currentTile, defaultTile);

                }

                currentTile = posInt;
                //tilemap.SetTileFlags(currentTile, TileFlags.None);
                tilemap.SetTile(currentTile, selectedTile);
                Debug.Log(tilemap.GetTile(currentTile));

                gameManager.SelectedTile = posInt;


            }


       }
    }
}
