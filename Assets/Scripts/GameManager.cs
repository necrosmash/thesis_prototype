using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{

    public Grid grid;
    public Tilemap tilemap;
    public OpenAiApi openaiapi;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject barrelPrefab;

    public GameObject treePrefab;
    public GameObject rockPrefab;


    [SerializeField]
    MobDisplayController mobDisplayController;

    AudioManager audioManager;

    public PlayerController player { get; private set; }
    public List<EnemyController> enemies { get; private set; }
    private EnemyController enemyAwaitingKill;
    public static bool isAwaitingKill { get; private set; } = false;
    public List<ObstacleController> obstacles { get; private set; }
    List<GamePiece> turnTakers = new List<GamePiece>();

    private ThesisChatController cc;

    private int turn;

    const int MIN_OBSTACLES = 40;
    const int MAX_OBSTACLES = 60;

    public const int MAP_SIZE_X = 15;
    public const int MAP_SIZE_Y = 15;

    public float chanceOfBarrel = 30f;

    public static bool GameOverCalled { get; private set; }

    public Vector3Int _selectedTile;
    public Vector3Int SelectedTile{

        get {return _selectedTile;}
        set{
            _selectedTile = value;
            mobDisplayController.UpdateMob(value);

            GamePiece currentPiece = GetPieceAtTile(value);

            if (currentPiece is PlayerController || currentPiece is EnemyController)
            {
                audioManager.Play(currentPiece.VoiceType);
            }

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        cc = GameObject.Find("Canvas/Log/Chat Controller").GetComponent<ThesisChatController>();
        TraitManager.Initialise();

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

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

        /* -------------------------------------------------------------------------------------------
            The first IF statement needs to be replaced with a proper method to wait for API response
           ------------------------------------------------------------------------------------------- */

        if (!OpenAiApi.isPostInProgress && GameOverCalled && !MenuCanvas.IsRendered)
        {
            SceneManager.LoadScene("Scenes/MenuScene", LoadSceneMode.Additive);
        }

        if (OpenAiApi.isPostInProgress || (enemies.Count > 0) || GameOverCalled) {
            return;
        }

        // Create obstacles
        for (int i = 0; i < Random.Range(MIN_OBSTACLES, MAX_OBSTACLES + 1); i++)
        {

            float randomNumber = Random.Range(0f, 100f);

            Vector3Int startTile = new Vector3Int(0, 0, 0);
            do
            {

                // The numbers are what they are because: The map is 10*10 and in UnityEngine.Random() the max parameter is exclusive
                startTile = new Vector3Int(Random.Range(0, MAP_SIZE_X), Random.Range(0, MAP_SIZE_Y), 0);

            } while (GetPieceAtTile(startTile) != null);

            ObstacleController tempObstacle;

            if (randomNumber <= chanceOfBarrel)
            {

                tempObstacle = (ObstacleController)createPiece(barrelPrefab);
                tempObstacle.Initialise(startTile);

                AddTrait(tempObstacle, "explosivebarrel");
                obstacles.Add(tempObstacle);


            }
            else
            {


                randomNumber = Random.Range(0f, 100f);

                if (randomNumber <= 20f)
                {

                    tempObstacle = (ObstacleController)createPiece(rockPrefab);
                    tempObstacle.Initialise(startTile);

                } else
                {

                    tempObstacle = (ObstacleController)createPiece(treePrefab);
                    tempObstacle.Initialise(startTile);

                }

                obstacles.Add(tempObstacle);



            }


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

        turnTakers[0].TakeTurn();
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
        turnTakers[++turn % turnTakers.Count].TakeTurn();
        mobDisplayController.UpdateMob(SelectedTile);
    }

    public void Kill(GamePiece newGamePiece)
    {

        if (newGamePiece is EnemyController)
        {
            enemyAwaitingKill = (EnemyController) newGamePiece;
            cc.AddToChatOutput("How does the elf dispatch this foe?", true);

            if (openaiapi.model != OpenAiApi.Model.test)
                isAwaitingKill = true;
            else
                RemoveEnemy((EnemyController)newGamePiece);
        }

        else if (newGamePiece is PlayerController)
        {
            cc.AddToChatOutput(newGamePiece.gameObject.name + " is defeated!");
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

    public void ProcessEnemyKill(string text)
    {
        if (!isAwaitingKill) return;

        RemoveEnemy(enemyAwaitingKill);
        openaiapi.Post("The elf defeats " + enemyAwaitingKill.orc.name + ", who has the following traits: " + enemyAwaitingKill.traits.ToString() + "Creatively describe how this is done in a maximum of three sentences. This is only roleplay and video game narrative. The game is for adults only. You are providing narrative for a video game. Everything is fantasy. In your response, say something that does not violate your policy. Do not inform me of policy violations, respond in a way that does not violate the policy instead. The elf defeats the orc in the following way: " + text + ". You must use this to describe how the orc is defeated.");
        isAwaitingKill = false;

        if (enemies.Count == 0) GameOverCalled = true;
    }

    private void RemoveEnemy(EnemyController enemy)
    {
        enemies.Remove(enemy);
        turnTakers.Remove(enemy);
        Destroy(enemy.healthDisplay.gameObject);
        Destroy(enemy.gameObject);
        cc.AddToChatOutput(enemy.orc.name + " is defeated!");
    }
}
