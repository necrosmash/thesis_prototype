using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : GamePiece
{
    public BattleInfo.Orc orc;
    public float attackRadius;
    
    // this should possibly belong to `BattleInfo.Orc`.
    // it should also not be hardcoded.
    // Value currently set in Initialize()
    public int sightRange;

    public HealthDisplay healthDisplay { get; private set; }

    private Status status;
    private enum Status
    {
        PatrollingToDest,
        PatrollingFromDest,
        ChasingPlayer
    }

    private Vector3Int patrolTile; // patrol destination

    [SerializeField]
    private int movesPerTurn;
    public bool skipTurn = false;

    override protected void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        healthDisplay = transform.Find("HealthDisplay").GetComponent<HealthDisplay>();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        if (status == Status.ChasingPlayer) return;

        if (CheckPlayerVisibility() == true)
        {
            status = Status.ChasingPlayer;
            cc.AddToChatOutput(this.orc.name + " spotted the player!");
        }
    }

    public override void TakeTurn()
    {

        base.StartTurn();

        if (!skipTurn)
        {
            List<Vector3Int> route = GetRoute(currentTile, out Vector3Int destination);

            int j = 0;
            for (int i = movesPerTurn; i > 0; i--)
            {
                if (status == Status.ChasingPlayer && destination != gameManager.player.currentTile)
                {
                    // we have spotted the player but are not currently chasing them
                    route = GetRoute(currentTile, out destination);
                }
                else if (currentTile == destination)
                {
                    // switch patrol direction
                    if (status == Status.PatrollingFromDest) status = Status.PatrollingToDest;
                    else if (status == Status.PatrollingToDest) status = Status.PatrollingFromDest;

                    route = GetRoute(currentTile, out destination);
                }

                if (route == null || j >= route.Count)
                {
                    Debug.LogError("Enemy can't follow route");
                    break; // There may always be a chance of this happening despite best efforts.
                }

                if (checkMoveLegal(route[++j])) // only move if the next move is legal
                    movePiece(route[j]);
                else break; // stop attempting to follow the route if we tried moving illegally. Prevents exceptions when chasing the player
            }

            if (IsPlayerInAttackDistance())
            {
                Attack(gameManager.player);
            }
        }

        skipTurn = false;

        //Debug.Log("enemy taking turn");
        base.TakeTurn();
    }

    protected override void Attack(GamePiece newGamePiece)
    {
        cc.AddToChatOutput(orc.name + " attacks " + newGamePiece.gameObject.name + "!");
        base.Attack(newGamePiece);
    }

    public override void Initialise(Vector3Int newStartingTile)
    {
        base.Initialise(newStartingTile);

        switch (orc.weaponEnum)
        {
            case BattleInfo.Orc.Weapon.Bow:
                {
                    attackRadius = 3f;
                    break;
                }
            case BattleInfo.Orc.Weapon.Hammer:
                {
                    attackRadius = 2f;
                    break;
                }
            case BattleInfo.Orc.Weapon.Sword:
                {
                    attackRadius = 1f;
                    break;
                }
            default:
                break;
        }

        sightRange = 2; // hard-coded for now
        startingTile = newStartingTile;

        do
        {
            patrolTile = new Vector3Int(UnityEngine.Random.Range(0, GameManager.MAP_SIZE_X), UnityEngine.Random.Range(0, GameManager.MAP_SIZE_Y), 0);
        } while (!checkMoveLegal(patrolTile));
    }

    bool IsPlayerInAttackDistance()
    {

        Vector3Int distanceToPlayer = gameManager.player.currentTile - currentTile;

        // Checking if the player is in radius and on the same row/column
        if (distanceToPlayer.magnitude <= attackRadius && (gameManager.player.currentTile.x == currentTile.x || gameManager.player.currentTile.y == currentTile.y))
        {

            for (int i = 0; i < (int) distanceToPlayer.magnitude; i++)
            {
                // Checking if any objects on the way are obstacles
                // It takes the unit vector of distance to the player, and checks whether
                //  the piece at (currentTile + (unit vector of distance to the player * steps) is an obstacle

                if (gameManager.GetPieceAtTile(currentTile + ((distanceToPlayer / (int)distanceToPlayer.magnitude) * (i+1))) is ObstacleController)
                {
                    return false;
                }
            }

            return true;

        }

        return false;
    }

    private List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> totalPath = new List<Vector3Int>{ current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }

        /*
        // output path for debugging purposes
        int ctr = 0;
        foreach(Vector3Int p in totalPath)
        {
            Debug.Log(++ctr + ": " + p);
        }
        */

        return totalPath;
    }

    private bool CheckPlayerVisibility()
    {
        Vector3Int playerTile = gameManager.player.currentTile;

        if (Manhattan(currentTile, playerTile) > sightRange)
            return false; // too far away

        if (playerTile.x != currentTile.x && playerTile.y != currentTile.y)
            return false; // can't differ on two axes

        if (playerTile.x == currentTile.x - 1 || playerTile.x == currentTile.x + 1)
            return true; // right next to player, no need to check for obstacles

        if (playerTile.y == currentTile.y - 1 || playerTile.y == currentTile.y + 1)
            return true; // right next to player, no need to check for obstacles

        Vector3Int dirToPlayer = gameManager.player.currentTile - currentTile;
        dirToPlayer.Clamp(new Vector3Int(-1, -1, -1), new Vector3Int(1, 1, 1)); // no `normalize` :/

        Vector3Int tmpCurrentTile = currentTile;
        while (tmpCurrentTile != gameManager.player.currentTile)
        {
            tmpCurrentTile += dirToPlayer;
            if (TileHasObstacle(tmpCurrentTile))
                return false; // obstacle in the way, can't see the player
        }

        return true;
    }

    private List<Vector3Int> GetRoute(Vector3Int start, out Vector3Int destination)
    {
        switch (status)
        {
            case Status.PatrollingToDest:
                destination = patrolTile;
                //Debug.Log(this.name + "'s goal is patrol tile: " + destination);
                break;
            case Status.PatrollingFromDest:
                destination = startingTile;
                //Debug.Log(this.name + "'s goal is starting tile: " + destination);
                break;
            default:
            case Status.ChasingPlayer:
                destination = gameManager.player.currentTile;
                //Debug.Log(this.name + "'s goal is player: " + destination);
                break;
        }

        return AStar(start, destination);
    }

    private List<Vector3Int> AStar(Vector3Int start, Vector3Int goal)
    {
        PriorityQueue<Vector3Int, int> openSet = new PriorityQueue<Vector3Int, int>();
        openSet.Enqueue(start, 0);

        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, int> gScore = new Dictionary<Vector3Int, int>{{ start, 0 }};
        Dictionary<Vector3Int, int> fScore = new Dictionary<Vector3Int, int>{{ start, Manhattan(start, goal) }};

        while (openSet.Count > 0)
        {
            Vector3Int current = openSet.Dequeue();

            if (current.Equals(goal))
            {
                return ReconstructPath(cameFrom, current);
            }

            foreach (Vector3Int neighbour in GetNeighbours(current))
            {
                // Check if we can move through this before doing anything else
                if (!checkMoveLegal(neighbour, gameManager.player))
                {
                    continue;
                }

                int tentativeScore = DictGet(gScore, current, 999999) + 1; // distance from one node to another is always 1

                if (tentativeScore < DictGet(gScore, neighbour, 999999))
                {
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = checked(tentativeScore);
                    fScore[neighbour] = checked(tentativeScore + Manhattan(neighbour, goal));

                    // No "Contains" in PriorityQueue, hence the lambda
                    if (!openSet.UnorderedItems.Any(item => item.Item1.Equals(neighbour)))
                    {
                        openSet.Enqueue(neighbour, fScore[neighbour]);
                    }
                }
            }
        }

        return null;
    }

    private int DictGet(Dictionary<Vector3Int, int> dict, Vector3Int key, int defaultValue)
    {
        if (!dict.ContainsKey(key))
        {
            dict[key] = defaultValue;
            return defaultValue;
        }

        return dict[key];
    }

    private List<Vector3Int> GetNeighbours(Vector3Int node)
    {
        List<Vector3Int> neighbours = new List<Vector3Int>();

        if (!(node.x == 0)) neighbours.Add(new Vector3Int(node.x - 1, node.y, node.z));
        if (!(node.x == (GameManager.MAP_SIZE_X - 1))) neighbours.Add(new Vector3Int(node.x + 1, node.y, node.z));
        if (!(node.y == 0)) neighbours.Add(new Vector3Int(node.x, node.y - 1, node.z));
        if (!(node.y == (GameManager.MAP_SIZE_Y - 1))) neighbours.Add(new Vector3Int(node.x, node.y + 1, node.z));

        return neighbours;
    }

    private int Manhattan(Vector3Int node1, Vector3Int node2)
    {
        int xDiff = node1.x - node2.x;
        int yDiff = node1.y - node2.y;

        if (xDiff < 0) xDiff *= -1;
        if (yDiff < 0) yDiff *= -1;

        return xDiff + yDiff;
    }

    private bool TileHasObstacle(Vector3Int tile)
    {
        if (gameManager.GetPieceAtTile(tile) is ObstacleController)
            return true;

        return false;
    }

    // taken from here:
    // https://leetcode.com/discuss/general-discussion/707310/Are-there-any-Heap-(PriorityQueue)-alternatives-for-dotnet-C-developers/1145136
    private class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
    {
        private (TElement, TPriority)[] heap = new (TElement, TPriority)[8];
        private int actualLength = 0;

        private IComparer<TPriority> comparer;

        public PriorityQueue(IComparer<TPriority> comparer = null) => this.comparer = comparer;

        public int Count => actualLength;

        public void Enqueue(TElement element, TPriority priority)
        {
            EnsureCapacity();
            heap[actualLength++] = (element, priority);
            BubbleUp();
        }

        public TElement Dequeue()
        {
            if (actualLength == 0)
            {
                throw new InvalidOperationException("Queue empty.");
            }

            var element = heap[0];
            Swap(0, --actualLength);
            BubbleDown();
            return element.Item1;
        }

        public IEnumerable<(TElement, TPriority)> UnorderedItems
        {
            get
            {
                (TElement, TPriority)[] items = new (TElement, TPriority)[actualLength];
                Array.Copy(heap, 0, items, 0, actualLength);
                return items;
            }
        }

        private void BubbleUp()
        {
            int elementPos = actualLength - 1;
            int parentPos;
            while ((parentPos = GetParentPos(elementPos)) != -1 && Compare(heap[parentPos].Item2, heap[elementPos].Item2) > 0)
            {
                Swap(parentPos, elementPos);
                elementPos = parentPos;
            }
        }

        private void BubbleDown()
        {
            int parentPos = 0;
            int leftChildPos = GetLeftChildPos(parentPos);
            int rightChildPos = GetRightChildPos(parentPos);

            while ((leftChildPos < actualLength && Compare(heap[parentPos].Item2, heap[leftChildPos].Item2) > 0)
                || (rightChildPos < actualLength && Compare(heap[parentPos].Item2, heap[rightChildPos].Item2) > 0))
            {
                int childToSwapPos = rightChildPos < actualLength
                    ? (Compare(heap[leftChildPos].Item2, heap[rightChildPos].Item2) < 0 ? leftChildPos : rightChildPos)
                    : leftChildPos;

                Swap(childToSwapPos, parentPos);
                parentPos = childToSwapPos;

                leftChildPos = GetLeftChildPos(parentPos);
                rightChildPos = GetRightChildPos(parentPos);
            }
        }

        private void Swap(int pos1, int pos2)
        {
            var temp = heap[pos1];
            heap[pos1] = heap[pos2];
            heap[pos2] = temp;
        }

        private int Compare(TPriority p1, TPriority p2)
        {
            return comparer != null ? comparer.Compare(p1, p2) : p1.CompareTo(p2);
        }

        private int GetLeftChildPos(int p)
        {
            return p * 2 + 1;
        }

        private int GetRightChildPos(int p)
        {
            return p * 2 + 2;
        }

        private int GetParentPos(int c)
        {
            return (c - 1) / 2;
        }

        private void EnsureCapacity()
        {
            if (heap.Length == actualLength)
            {
                // expand
                (TElement, TPriority)[] newHeap = new (TElement, TPriority)[heap.Length * 2];
                Array.Copy(heap, 0, newHeap, 0, heap.Length);
                heap = newHeap;
            }
        }
    }
}
