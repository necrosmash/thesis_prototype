using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mouseCapture : MonoBehaviour
{
    public Grid grid;
    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       if (Input.GetMouseButtonDown(0)) {

            Vector3 mousePos = Input.mousePosition;
            // If the camera is Perspective rather than Orthographic, this needs to be included
            // I am actually currently a bit confused about what the actual value of mousePos.z is
            //mousePos.z = Camera.main.transform.position.y;

            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3Int posInt = grid.WorldToCell(pos);

            //Shows the cell reference for the grid
            Debug.Log(posInt);

            Debug.Log(tilemap.HasTile(posInt));
       }
    }
}
