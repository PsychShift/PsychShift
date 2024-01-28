using UnityEngine;

namespace VigilPathfinding
{
    public class PathNode
    {
        public int G;
        public int H;
        public int F { get { return G + H; } }

        public bool isBlocked;
        public PathNode previousNode;

        public Vector2Int position;

        public PathNode(Vector2Int position, bool isBlocked)
        {
            this.position = position;
            this.isBlocked = isBlocked;
        }
    }
}