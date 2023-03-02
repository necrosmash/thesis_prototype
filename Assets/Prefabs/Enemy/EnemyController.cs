using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : GamePiece
{

    public enum Size{
        Small,
        Medium,
        Large
    }

    public enum Weapon{
        Sword,
        Hammer,
        Bow
    }

    public string name;
    public string description;
    public Weapon weapon;
    public Size size;

    void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
