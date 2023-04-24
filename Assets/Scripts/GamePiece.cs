using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GamePiece : MonoBehaviour
{

    protected Vector3Int startingTile;
    public Vector3Int currentTile {get; protected set;}

    // This is a gibberish value required to check if the piece was initialised
    Vector3Int DEFAULT_STARTING_VALUE = new Vector3Int(-1, -1, -1);

    protected GameManager gameManager;
    protected Grid grid;
    protected Tilemap tilemap;

    public int health, maxHealth = -1;
    public Traits traits = new Traits();

    [SerializeField]
    public int baseDamage;
    [SerializeField]
    public int minDamageRoll;
    [SerializeField]
    public int maxDamageRoll;
    [SerializeField]
    private float movementSpeed;

    public int minDamageResistRoll = 0;
    public int maxDamageResistRoll = 0;

    public bool skipAttack = false;

    public bool isMoving { get; private set; }
    private Vector3Int newTile;
    private Vector3 destination;

    [SerializeField]
    protected Animation anim;
    protected bool isPlayingAttackAnim = false, isPlayingDeathAnim = false;

    [SerializeField]
    private AnimationClip acIdle, acWalk, acAttack, acDeath;

    protected ThesisChatController cc;
    protected AudioManager audioManager;
    public string VoiceType { get; protected set; }
    public string AttackSound { get; protected set; }
    public string DamageSound { get; protected set; }


    protected virtual void Awake(){

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        grid = gameManager.grid;
        tilemap = gameManager.tilemap;
        
        currentTile = DEFAULT_STARTING_VALUE;

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        cc = GameObject.Find("Canvas/Log/Chat Controller").GetComponent<ThesisChatController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Animate();

        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.001f)
        {
            transform.position = destination;
            currentTile = newTile;
            isMoving = false;
        }
    }

    public virtual void TakeTurn()
    {


        foreach (Trait trait in traits)
        {

            trait.OnTakeTurn(this.gameObject);

        }

        Traits traitsToRemove = new Traits();
        for (int i = 0; i < traits.Count; i++)
        {
            if (traits[i].RemainingDuration <= 0)
            {
                traitsToRemove.Add(traits[i]);
            }
        }

        foreach (Trait t in traitsToRemove)
        {
            traits.Remove(t);
            Destroy(t.gameObject);
        }


        gameManager.FinishTurn();
    }

    public virtual void StartTurn()
    {
        foreach (Trait trait in traits)
        {
            trait.OnStartTurn(this.gameObject);

        }
    }

    protected virtual void movePiece(Vector3Int newTile, bool animate = true)
    {    
        if (animate)
        {
            isMoving = true;
            LookAt(newTile);

            this.newTile = newTile;
            destination = grid.GetCellCenterWorld(newTile);
        }
        else
        {
            transform.position = grid.GetCellCenterWorld(newTile);
            currentTile = newTile;
        }
    }

    private void LookAt(GamePiece gp) => LookAt(gp.currentTile);
    private void LookAt(Vector3Int tile)
    {
        Vector3Int gridDirection = tile - currentTile;
        Vector3Int convertedDirection = new Vector3Int(gridDirection.x, gridDirection.z, gridDirection.y);
        transform.rotation = Quaternion.LookRotation(convertedDirection, Vector3.up);
    }

    protected virtual void Attack(GamePiece newGamePiece)
    {
        isPlayingAttackAnim = true;
        LookAt(newGamePiece);

        foreach (Trait trait in traits)
        {

            trait.OnAttack(this.gameObject);

        }

        if (!skipAttack)
        {
            int newDamage = baseDamage + Random.Range(minDamageRoll, maxDamageRoll + 1);

            Debug.Log("Attacking! My base damage is: " + baseDamage + ". My min roll is: " + minDamageRoll + ". My max roll is: " + maxDamageRoll + ". Total damage output: " + newDamage);

            newGamePiece.TakeDamage(newDamage);
        }
        skipAttack = false;

    }

    public virtual void Initialise(Vector3Int newStartingTile)
    {
        if (currentTile != DEFAULT_STARTING_VALUE)
        {
            throw new BadInitialisationException("Already initialised");
        }

        startingTile = newStartingTile;
        movePiece(startingTile, false);
    }

    public virtual void TakeDamage(int damage = 10)
    {
        foreach (Trait trait in traits)
        {

            trait.OnTakeDamage(this.gameObject);

        }

        damage -= Random.Range(minDamageResistRoll, maxDamageResistRoll + 1);
        Debug.Log("Taking damage! My min damage resist roll: " + minDamageResistRoll + ". My max damage resist roll: " + maxDamageResistRoll + " Total damage received: " + damage);

        string output = (
            this is EnemyController ?
                ((EnemyController)this).orc.name :
                gameObject.name) +
            " takes " + damage + " damage!";
        ;
        cc.AddToChatOutput(output);

        if (damage < 0)
        {
            damage = 0;
        }

        if (damage > 0 && DamageSound != null)
        {
            // A delay is added so the preceeding attack sound is played first
            audioManager.PlayDelayed(DamageSound, 0.25f);
        }

        health -= damage;

        if (health <= 0)
            isPlayingDeathAnim = true;
    }

    protected bool checkMoveLegal(Vector3Int newTile, GamePiece acceptableGamePiece = null)
    {
        if (!tilemap.HasTile(newTile)) return false;

        if (gameManager.GetPieceAtTile(newTile) == null ||
            gameManager.GetPieceAtTile(newTile).Equals(acceptableGamePiece))
            return true;

        return false;
    }

    private void Animate()
    {
        if (isPlayingDeathAnim && !anim.IsPlaying(acDeath.name))
            StartCoroutine(DeathAnimate());
        if (isPlayingDeathAnim)
            return;

        if (isPlayingAttackAnim && !anim.IsPlaying(acAttack.name))
            StartCoroutine(AttackAnimate());
        if (isPlayingAttackAnim)
            return;

        if (isMoving && !anim.IsPlaying(acWalk.name))
            anim.Play(acWalk.name);
        else if (!isMoving)
            anim.Play(acIdle.name);
    }

    private IEnumerator DeathAnimate()
    {
        anim.Play(acDeath.name);
        yield return new WaitForSeconds(acDeath.length);
        isPlayingDeathAnim = false;
        gameManager.Kill(this);
    }

    private IEnumerator AttackAnimate()
    {
        anim.Play(acAttack.name);
        yield return new WaitForSeconds(acAttack.length);
        isPlayingAttackAnim = false;
    }
}

public class BadInitialisationException : System.Exception
{

    public BadInitialisationException() : base() { }
    public BadInitialisationException(string message) : base(message)
    {

    }
}

public class Traits : List<Trait>
{
    // Currently, we provide LLMDescriptions every time a trait is mentioned. If we want to 
    // save tokens by describing traits only once (e.g. at the start of the conversation), we can 
    // use this method to exclude the descriptions later on in the conversation with the API.
    //public string ToString(bool includeLLMDesc) => BuildString(includeLLMDesc);

    public override string ToString() => BuildString(true);

    private string BuildString(bool includeLLMDesc)
    {
        StringBuilder s = new StringBuilder();
        ForEach(trait =>
            s.Append(includeLLMDesc && !trait.LLMDescription.Equals(string.Empty) ?
                trait.Name + " (" + trait.LLMDescription + "), " :
                trait.Name + ", "));

        if (s.Length <= 0) return "";

        s.Replace(", ", ". ", s.Length - 2, 2);
        return s.ToString();
    }
}