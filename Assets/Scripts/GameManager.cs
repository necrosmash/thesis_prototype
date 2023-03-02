using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    public Grid grid;
    public Tilemap tilemap;
    public OpenAiApi openaiapi;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    // public GameObject obstaclePrefab;

    PlayerController player;
    List<EnemyController> enemies = new List<EnemyController>();

    // Start is called before the first frame update
    void Start()
    {

        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {   /* -------------------------------------------------------------------------------------------
            The first IF statement needs to be replaced with a proper method to wait for API response
           ------------------------------------------------------------------------------------------- */

        if ((openaiapi.response == null) || (enemies.Count > 0)){
            return;
        }

        BattleInfo.Orc[] orcs = openaiapi.response.battleInfo.orcs;
        for (int i = 0; i < orcs.Length; i++){

            // TODO: Replace positions with data from API or randomised data
            EnemyController tempEnemy = createEnemy(new Vector3Int(i+1, i+1, 0));

            tempEnemy.name = orcs[i].name;
            tempEnemy.description = orcs[i].description;

            switch (orcs[i].weapon){
                case "sword":{
                    tempEnemy.weapon = EnemyController.Weapon.Sword;
                    break;
                }
                case "hammer":{
                    tempEnemy.weapon = EnemyController.Weapon.Hammer;
                    break;
                }
                case "bow":{
                    tempEnemy.weapon = EnemyController.Weapon.Bow;
                    break;
                }
                default:{
                    throw new BadBattleInfoException("Weapon does not exist: " + orcs[i].weapon);
                }
            }

            switch (orcs[i].size){
                case "large":{
                    tempEnemy.size = EnemyController.Size.Large;
                    break;
                }
                case "medium":{
                    tempEnemy.size = EnemyController.Size.Medium;
                    break;
                }
                case "small":{
                    tempEnemy.size = EnemyController.Size.Small;
                    break;
                }
                default:{
                    throw new BadBattleInfoException("Size does not exist: " + orcs[i].size);
                }
            }

            enemies.Add(tempEnemy);

        }

        
    }

    EnemyController createEnemy(Vector3Int newStartingTile){

        EnemyController tempEnemy = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<EnemyController>();
        tempEnemy.startingTile = newStartingTile;
        return tempEnemy;

    }



}

public class BadBattleInfoException : System.Exception { 

    public BadBattleInfoException()
    {
    }

    public BadBattleInfoException(string message)
        : base(message)
    {
    }

}
