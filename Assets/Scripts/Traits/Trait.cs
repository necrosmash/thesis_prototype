using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait : MonoBehaviour
{

    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public string LLMDescription { get; protected set; } = "";
    public int Duration { get; protected set; }
    public int RemainingDuration { get; protected set; }

    protected virtual void Awake()
    {
        RemainingDuration = Duration;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    void Tick()
    {

        RemainingDuration -= 1;

    }

    public virtual void OnStartTurn(GameObject newGameObject)
    {

        return;

    }

    public virtual void OnAttack(GameObject newGameObject)
    {

        return;

    }

    public virtual void OnTakeTurn(GameObject newGameObject)
    {

        Tick();
        return;

    }

    public virtual void OnTakeDamage(GameObject newGameObject)
    {
        return;
    }

}
