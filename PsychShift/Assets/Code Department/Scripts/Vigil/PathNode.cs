using UnityEngine;

namespace VigilPathfinding
{
    [System.Serializable]
    public class PathNode
    {
        public int G;
        public int H;
        public int F { get { return G + H; } }

        public bool isBlocked;
        public PathNode previousNode;

        public Vector3Int position;

        public PathNode(Vector3Int position, bool isBlocked)
        {
            this.position = position;
            this.isBlocked = isBlocked;
        }

        public override string ToString()
        {
            return $"Position: {position} \n Blocked: {isBlocked} \n (G - {G}, H - {H}, F - {F})";
        }
    }
}