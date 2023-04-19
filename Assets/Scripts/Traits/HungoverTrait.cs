using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungoverTrait : Trait
{

    [SerializeField]
    float attackSkipChance;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        Name = "Hungover";
        Description = "This orc really, <i>really</i> needs some Paracetamol and quiet";
        Duration = 10000;
        base.Awake();

    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void OnAttack(GameObject newGameObject)
    {

        GamePiece tempGamePiece = newGameObject.GetComponent<GamePiece>();

        if (!(tempGamePiece is EnemyController))
        {
            return;
        }

        if (((EnemyController) tempGamePiece).orc.weaponEnum != BattleInfo.Orc.Weapon.Bow)
        {
            return;
        }

        float tempNumber = Random.Range(0f, 100f);

        if (tempNumber <= attackSkipChance)
        {
            newGameObject.GetComponent<GamePiece>().skipAttack = true;
        }

        base.OnAttack(newGameObject);
    }

}
