using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MobDisplayController : MonoBehaviour
{
    
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    TMP_InputField inputField;

    GamePiece currentPiece = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // inputField.text = "HALLO";
    }

    public void UpdateMob(Vector3Int newTile){

        currentPiece = gameManager.GetPieceAtTile(newTile);

        if (currentPiece is EnemyController)
        {
            this.gameObject.SetActive(true);
            EnemyController tempEnemy = (EnemyController) currentPiece;
            inputField.text = "<b>Name</b>: " + tempEnemy.orc.name + "\n\n<b>Description</b>: " + tempEnemy.orc.description + "\n\n<b>Weapon</b>: " + tempEnemy.orc.weapon + "\n\n<b>Size</b>: " + tempEnemy.orc.size;

        } else {
            this.gameObject.SetActive(false);
        }

    }

    // public void Activate(GamePiece piece){
    //     this.setActive = true;
    // }

    // public void Deactivate(){
    //     this.setActive = false;
    // }

}