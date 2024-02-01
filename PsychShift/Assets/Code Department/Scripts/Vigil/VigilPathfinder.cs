using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace VigilPathfinding
{
    public static class VigilPathfinder
    {
        /* public List<Vector3Int> Search(Hallway hallway, Dictionary<Vector3Int, PathNode> map)
        {
            if(!hallway.MultiConnectionHall)
            {
                map[hallway.From].isBlocked = false;
                map[hallway.To].isBlocked = false;

                List<PathNode> pathNodes = FindPath(map[hallway.From], map[hallway.To], map);
                List<Vector3Int> pathPositions = pathNodes.Select(node => node.position).ToList();
                return pathPositions;
            }
            else
            {
                Vector3Int fromRoomValue = hallway.MultiConnectionPointList.ElementAt(0).Value;
                Vector3Int toRoomValue = hallway.MultiConnectionPointList.ElementAt(1).Value;
                map[fromRoomValue].isBlocked = false;
                map[toRoomValue].isBlocked = false;

                List<PathNode> pathNodes = FindPath(map[hallway.From], map[hallway.To], map);
                List<Vector3Int> pathPositions = pathNodes.Select(node => node.position).ToList();

                for(int i = 2; i < hallway.MultiConnectionPointList.Count-1; i++)
                {
                    Vector3Int mapIndex = pathPositions[pathPositions.Count/2];
                    Vector3Int toIndex = hallway.MultiConnectionPointList.ElementAt(i).Value;
                    map[mapIndex].isBlocked = false;
                    pathNodes.AddRange(FindPath(map[mapIndex], map[toIndex], map));
                    pathPositions = pathNodes.Select(node => node.position).ToList();
                }
                return pathPositions;    
            }
        } */
        
        public static bool FindPath(Vector3Int currentPos, Vector3Int endPos, Dictionary<Vector3Int, PathNode> map, out List<Vector3Int> path)
        {
            if(map[endPos].isBlocked)
            {
                path = null;
                return false;
            }
            List<PathNode> pathNodes = FindPath(map[currentPos], map[endPos], map);
            path = pathNodes.Select(node => node.position).ToList();
            return true;
        }
        private static List<PathNode> FindPath(PathNode startNode, PathNode endNode, Dictionary<Vector3Int, PathNode> map)
        {
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();

            openList.Add(startNode);
            while(openList.Count > 0)
            {
                PathNode currentNode = openList.OrderBy(x => x.F).First();

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if(currentNode == endNode)
                {
                    // Finalize Path
                    return GetFinishedList(startNode, endNode);
                }

                var neighborNodes = GetNeighborNodes(currentNode, map);
                foreach(var neighbor in neighborNodes)
                {
                    if(neighbor.isBlocked || closedList.Contains(neighbor))
                        continue;
                       
                    neighbor.G = GetManhattenDistance(startNode, neighbor);
                    neighbor.H = GetManhattenDistance(endNode, neighbor);

                    neighbor.previousNode = currentNode;

                    if(!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }

            return new List<PathNode>();
        }

        private static List<PathNode> GetFinishedList(PathNode startNode, PathNode endNode)
        {
            List<PathNode> finishedList = new();

            PathNode currentNode = endNode;

            while(currentNode != startNode)
            {
                finishedList.Add(currentNode);
                currentNode = currentNode.previousNode;
            }

            finishedList.Reverse();

            return finishedList;
        }

        private static int GetManhattenDistance(PathNode startNode, PathNode neighbor)
        {
            return Mathf.Abs(startNode.position.x - neighbor.position.x) + Mathf.Abs(startNode.position.y - neighbor.position.y);
        }

        private static List<PathNode> GetNeighborNodes(PathNode currentNode, Dictionary<Vector3Int, PathNode> map)
        {
            List<PathNode> neighbors = new();

            Vector3Int pos = currentNode.position;

            if(map.ContainsKey(pos+Vector3Int.up))
                neighbors.Add(map[pos+Vector3Int.up]);
            if(map.ContainsKey(pos+Vector3Int.down))
                neighbors.Add(map[pos+Vector3Int.down]);
            if(map.ContainsKey(pos+Vector3Int.left))
                neighbors.Add(map[pos+Vector3Int.left]);
            if(map.ContainsKey(pos+Vector3Int.right))
                neighbors.Add(map[pos+Vector3Int.right]);

            return neighbors;
        }
    }
}
