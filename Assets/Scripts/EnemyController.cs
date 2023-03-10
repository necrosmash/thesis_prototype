using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : GamePiece
{
    public BattleInfo.Orc orc;

    [SerializeField]
    private int movesPerTurn;

    override protected void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override protected void Update()
    {

    }

    public override void TakeTurn()
    {   
        List<Vector3Int> route = AStar(currentTile, gameManager.player.currentTile);

        int j = 0;
        for (int i = movesPerTurn; i > 0; i--)
        {
            if (checkMoveLegal(route[++j])) // only move if the next move is legal
                movePiece(route[j]);
        }

        base.TakeTurn();
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
                if (TileHasObstacle(neighbour))
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
        if (!(node.x == 9)) neighbours.Add(new Vector3Int(node.x + 1, node.y, node.z));
        if (!(node.y == 0)) neighbours.Add(new Vector3Int(node.x, node.y - 1, node.z));
        if (!(node.y == 9)) neighbours.Add(new Vector3Int(node.x, node.y + 1, node.z));

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
        if (gameManager.getPieceAtTile(tile) is ObstacleController)
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
