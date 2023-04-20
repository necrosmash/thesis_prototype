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

    [SerializeField]
    MobDisplayController mobDisplayController;

    public PlayerController player { get; private set; }
    public List<EnemyController> enemies { get; private set; }
    public List<ObstacleController> obstacles { get; private set; }
    List<GamePiece> turnTakers = new List<GamePiece>();

    private int turn;

    const int MIN_OBSTACLES = 12;
    const int MAX_OBSTACLES = 20;

    public const int MAP_SIZE_X = 15;
    public const int MAP_SIZE_Y = 15;

    public static bool GameOverCalled { get; private set; }

    public Vector3Int _selectedTile;
    public Vector3Int SelectedTile{

        get {return _selectedTile;}
        set{
            _selectedTile = value;
            mobDisplayController.UpdateMob(value);
        }

    }

    // Start is called before the first frame update
    void Start()
    {

        TraitManager.Initialise();

        enemies = new List<EnemyController>();
        obstacles = new List<ObstacleController>();

        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<PlayerController>();
        player.Initialise(new Vector3Int(0, 0, 0));

        turnTakers.Add(player);
        turn = 0;

        GameOverCalled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !MenuCanvas.IsRendered)
        {
            SceneManager.LoadScene("Scenes/MenuScene", LoadSceneMode.Additive);
        }

        // for testing
        DamageKeystroke();

        /* -------------------------------------------------------------------------------------------
            The first IF statement needs to be replaced with a proper method to wait for API response
           ------------------------------------------------------------------------------------------- */

        if (OpenAiApi.isPostInProgress || (enemies.Count > 0)) {
            return;
        }

        // Create obstacles
        for (int i = 0; i < Random.Range(MIN_OBSTACLES, MAX_OBSTACLES + 1); i++)
        {

            Vector3Int startTile = new Vector3Int(0, 0, 0);
            do
            {

                // The numbers are what they are because: The map is 10*10 and in UnityEngine.Random() the max parameter is exclusive
                startTile = new Vector3Int(Random.Range(0, MAP_SIZE_X), Random.Range(0, MAP_SIZE_Y), 0);

            } while (GetPieceAtTile(startTile) != null);

            ObstacleController tempObstacle = (ObstacleController)createPiece(obstaclePrefab);
            tempObstacle.Initialise(startTile);

            AddTrait(tempObstacle, "explosivebarrel");

            obstacles.Add(tempObstacle);

        }

        // Create enemies
        BattleInfo.Orc[] orcs = openaiapi.response.battleInfo.orcs;
        for (int i = 0; i < orcs.Length; i++){


            Vector3Int startTile = new Vector3Int(0, 0, 0);
            do{
                
                // The numbers are what they are because: The map is 10*10 and in UnityEngine.Random() the max parameter is exclusive
                startTile = new Vector3Int(Random.Range(0, MAP_SIZE_X), Random.Range(0, MAP_SIZE_Y), 0);

            }while (GetPieceAtTile(startTile) != null);

            EnemyController tempEnemy = (EnemyController) createPiece(enemyPrefab);

            tempEnemy.orc = orcs[i];
            tempEnemy.Initialise(startTile);

            AddTrait(tempEnemy, TraitManager.GetRandomSpawnTrait());
            enemies.Add(tempEnemy);

        }

        turnTakers.AddRange(enemies);
        turnTakers.AddRange(obstacles);

        FinishTurn();
    }

    GamePiece createPiece(GameObject newPrefab){

        GamePiece tempPiece = Instantiate(newPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<GamePiece>();
        return tempPiece;

    }

    public void AddTrait(GamePiece newGamePiece, string newTraitName)
    {

        GameObject tempTrait = TraitManager.GetTrait(newTraitName);

        if (tempTrait == null)
        {

            throw new System.Exception("Trait does not exist: " + newTraitName);

        }

        Trait trait = Instantiate(tempTrait, newGamePiece.gameObject.transform).GetComponent<Trait>();
        newGamePiece.traits.Add(trait);

    }

    public GamePiece GetPieceAtTile(Vector3Int newTile){

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
        StartCoroutine(PollToFinishTurn());
    }

    private IEnumerator PollToFinishTurn()
    {
        while (OpenAiApi.isPostInProgress || turnTakers[turn % turnTakers.Count].isMoving)
        {
            yield return new WaitForSeconds(0.1f);
        }
        turnTakers[turn++ % turnTakers.Count].TakeTurn();
        mobDisplayController.UpdateMob(SelectedTile);
    }

    public void Kill(GamePiece newGamePiece)
    {

        if (newGamePiece is EnemyController)
        {
            enemies.Remove((EnemyController) newGamePiece);
            turnTakers.Remove((EnemyController) newGamePiece);
            Destroy(((EnemyController) newGamePiece).healthDisplay.gameObject);
            Destroy(newGamePiece.gameObject);
            openaiapi.Post("The main character kills " + ((EnemyController)newGamePiece).orc.name + ", who has the following traits: " + newGamePiece.traits.ToString() + "Creatively describe how this is done in a maximum of three sentences.");
        }

        else if (newGamePiece is PlayerController)
        {
            GameOverCalled = true;
            SceneManager.LoadScene("Scenes/MenuScene", LoadSceneMode.Additive);
        }

        else if (newGamePiece is ObstacleController)
        {
            obstacles.Remove((ObstacleController)newGamePiece);
            turnTakers.Remove((ObstacleController)newGamePiece);
            Destroy(newGamePiece.gameObject);

        }
    }

    // for testing
    private void DamageKeystroke()
    {
        if (MenuCanvas.IsRendered) return;
        if (Input.GetKeyDown(KeyCode.H)) player.TakeDamage(10);
    }
}
