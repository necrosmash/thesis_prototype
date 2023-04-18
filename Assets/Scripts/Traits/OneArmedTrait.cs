using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneArmedTrait : Trait
{

    [SerializeField]
    float attackSkipChance;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        Name = "One-armed";
        Description = "This orc a one-armed bandit";
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

        if (((EnemyController) tempGamePiece).orc.weaponEnum != BattleInfo.Orc.Weapon.Hammer && ((EnemyController) tempGamePiece).orc.weaponEnum != BattleInfo.Orc.Weapon.Sword)
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
