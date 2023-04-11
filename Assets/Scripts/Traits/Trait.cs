using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait : MonoBehaviour
{

    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public int Duration { get; protected set; }
    public int RemainingDuration { get; protected set; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        RemainingDuration = Duration;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    void Tick()
    {

        RemainingDuration -= 1;
        //if (RemainingDuration <= 0)
        //{
        //    Destroy(this.gameObject);
        //}

    }

    public virtual void OnStartTurn(GameObject newGameObject)
    {

        Debug.Log("Base start turn");
        return;

    }

    public virtual void OnAttack()
    {

        Debug.Log("Base attack");
        return;

    }

    public virtual void OnTakeTurn(GameObject newGameObject)
    {

        Debug.Log("Base take turn");
        Tick();
        return;

    }

    public virtual void OnTakeDamage()
    {
        Debug.Log("Base take damage");
        return;
    }

}
