using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : GamePiece
{
    // Currently a stub, but in the future can be used similarly to EnemyController

    public List<GameObject> modelsList;

    bool first = true;

    override protected void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        if (modelsList.Count == 0) return;

        int randomNumber = Random.Range(0, modelsList.Count);

        modelsList[randomNumber].SetActive(true);

        for (int i = 0; i < modelsList.Count; i++)
        {

            if (i != randomNumber)
            {
                Destroy(modelsList[i].gameObject);
            }

        }

    }

    // Update is called once per frame
    override protected void Update()
    {


    }

    public override void StartTurn()
    {
        base.StartTurn();
    }

    public override void TakeTurn() {

        base.TakeTurn();
        
    }

    public override void TakeDamage(int damage = 10)
    {
        base.TakeDamage(damage);
    }

}
