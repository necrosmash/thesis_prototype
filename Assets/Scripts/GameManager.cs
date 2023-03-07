using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    public Grid grid;
    public Tilemap tilemap;
    public OpenAiApi openaiapi;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject obstaclePrefab;

    PlayerController player;
    List<EnemyController> enemies = new List<EnemyController>();
    List<ObstacleController> obstacles = new List<ObstacleController>();
    List<GamePiece> turnTakers = new List<GamePiece>();

    private int turn;

    const int MIN_OBSTACLES = 6;
    const int MAX_OBSTACLES = 12;


    // Start is called before the first frame update
    void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<PlayerController>();
        player.initialise(new Vector3Int(0, 0, 0));

        turnTakers.Add(player);
        turn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !MenuCanvas.IsRendered)
        {
            SceneManager.LoadScene("Scenes/MenuScene", LoadSceneMode.Additive);
        }

        /* -------------------------------------------------------------------------------------------
            The first IF statement needs to be replaced with a proper method to wait for API response
           ------------------------------------------------------------------------------------------- */

        if ((openaiapi.response == null) || (enemies.Count > 0)){
            return;
        }

        // Create enemies
        BattleInfo.Orc[] orcs = openaiapi.response.battleInfo.orcs;
        for (int i = 0; i < orcs.Length; i++){


            Vector3Int startTile = new Vector3Int(0, 0, 0);
            do{
                
                // The numbers are what they are because: The map is 10*10 and in UnityEngine.Random() the max parameter is exclusive
                startTile = new Vector3Int(Random.Range(0, 10), Random.Range(0, 10), 0);

            }while (getPieceAtTile(startTile) != null);

            EnemyController tempEnemy = (EnemyController) createPiece(startTile, enemyPrefab);

            tempEnemy.orc = orcs[i];

            enemies.Add(tempEnemy);

        }

        turnTakers.AddRange(enemies);

        // Create obstacles
        for (int i = 0; i < Random.Range(MIN_OBSTACLES, MAX_OBSTACLES + 1); i++){

            Vector3Int startTile = new Vector3Int(0, 0, 0);
            do{

                // The numbers are what they are because: The map is 10*10 and in UnityEngine.Random() the max parameter is exclusive
                startTile = new Vector3Int(Random.Range(0, 10), Random.Range(0, 10), 0);

            }while (getPieceAtTile(startTile) != null);

            ObstacleController tempObstacle = (ObstacleController) createPiece(startTile, obstaclePrefab);

            obstacles.Add(tempObstacle);

        }

        FinishTurn();
    }

    GamePiece createPiece(Vector3Int newStartingTile, GameObject newPrefab){

        GamePiece tempPiece = Instantiate(newPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<GamePiece>();
        tempPiece.initialise(newStartingTile);
        return tempPiece;

    }

    public GamePiece getPieceAtTile(Vector3Int newTile){

        if (player.currentTile == newTile){
            return player;
        }

        foreach (EnemyController enemy in enemies){
            if (enemy.currentTile == newTile){
                return enemy;
            }
        }

        foreach (ObstacleController obstacle in obstacles){
            if (obstacle.currentTile == newTile){
                return obstacle;
            }
        }

        return null;

    }

    public void FinishTurn()
    {
        if (turn <= 100)
            turnTakers[turn++ % turnTakers.Count].TakeTurn();
    }

}
