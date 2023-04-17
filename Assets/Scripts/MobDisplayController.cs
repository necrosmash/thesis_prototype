using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MobDisplayController : MonoBehaviour
{
    
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    TextMeshProUGUI inputField;
    [SerializeField]
    GameObject mobDisplayWindow;

    GamePiece currentPiece = null;
    [SerializeField]
    ScrollRect scrollRect;

    // Start is called before the first frame update
    void Start()
    {
        scrollRect.verticalNormalizedPosition = 1;
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

            scrollRect.verticalNormalizedPosition = 1;

            mobDisplayWindow.gameObject.SetActive(true);


            EnemyController tempEnemy = (EnemyController) currentPiece;
            inputField.text = "<b>Name</b>: " + tempEnemy.orc.name + "\n\n<b>Description</b>: " + tempEnemy.orc.description + "\n\n<b>Weapon</b>: " + tempEnemy.orc.weapon + "\n\n<b>Size</b>: " + tempEnemy.orc.size + "\n\n\t\t<b><color=#0000a0ff>Traits</color></b>:\n\n";

            string traitsString = "";

            foreach (Trait trait in tempEnemy.traits)
            {

                traitsString += "<b>Name</b>: " + trait.Name + "\n\n<b>Description</b>:\n\n" + trait.Description + "\n\n<b>Remaining duration</b>: " + trait.RemainingDuration + "\n\n";

            }

            inputField.text += traitsString;



        } else {
            mobDisplayWindow.gameObject.SetActive(false);
        }

    }

    // public void Activate(GamePiece piece){
    //     this.setActive = true;
    // }

    // public void Deactivate(){
    //     this.setActive = false;
    // }

}
