using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : Manager<GridManager>
{
    public Tilemap grid;
    
    Graph graph;
    Dictionary<Team, int> startPositionPerTeam;
    
    protected void Awake()
    {
        base.Awake();
        InitializeGraph();
        startPositionPerTeam = new Dictionary<Team, int>();
        startPositionPerTeam.Add(Team.Team1, 0);
        startPositionPerTeam.Add(Team.Team2, graph.nodes.Count - 1);
    }
    
    public Node GetFreeNode(Team forTeam)
    {
        int startIndex = startPositionPerTeam[forTeam];
        int currentIndex = startIndex;

        while(graph.nodes[currentIndex].IsOccupied)
        {
            if (startIndex == 0)
            {
                currentIndex++;
                if(currentIndex == graph.nodes.Count)
                    return null;
            }
            else
            {
                currentIndex--;
                if(currentIndex == -1)
                    return null;
            }
        }

        return graph.nodes[currentIndex];
    }

    public List<Node> GetPath(Node from, Node to)
    {
        return graph.GetShortestPath(from, to);
    }

    public List<Node> GetNodesCloseTo(Node to)
    {
        return graph.Neighbors(to);
    }

    private void InitializeGraph()
    {
        graph = new Graph();
        
        for(int x = grid.cellBounds.xMin; x < grid.cellBounds.xMax; x++)
        {
            for(int y = grid.cellBounds.yMin; y < grid.cellBounds.yMax; y++)
            {
                Vector3Int localPosition = new Vector3Int(x, y, (int)grid.transform.position.y);
                
                if(grid.HasTile(localPosition))
                {
                    Vector3 worldPosition = grid.CellToWorld(localPosition);
                    graph.AddNode(worldPosition, localPosition);
                }
            }
        }

        var allNodes = graph.nodes;

        foreach(Node from in allNodes)
        {
            foreach(Node to in allNodes)
            {
                if(Mathf.Abs(from.mapPosition.x - to.mapPosition.x) < 1.3f && from != to)
                {
                    graph.AddEdge(from, to);
                }
            }
        }
    }

    //Debug stuff
    public int fromIndex = 0;
    public int toIndex = 0;

    private void OnDrawGizmos()
    {
        if(graph == null)
        {    
            return;
        }

        var allEdges = graph.edges;

        foreach(Edge e in allEdges)
        {
            Debug.DrawLine(e.from.worldPosition, e.to.worldPosition, Color.black, 1);
        }

        var allNodes = graph.nodes;
        foreach(Node n in allNodes)
        {
            Gizmos.color = n.IsOccupied ? Color.red : Color.green;
            Gizmos.DrawSphere(n.worldPosition, 0.1f);
        }

        if(fromIndex < allNodes.Count && toIndex < allNodes.Count)
        {
            List<Node> path = graph.GetShortestPath(allNodes[fromIndex], allNodes[toIndex]);
            if(path.Count > 1)
            {
                for(int i = 1; i < path.Count; i++)
                {
                    Debug.DrawLine(path[i-1].worldPosition, path[i].worldPosition, Color.red, 1);
                }
            }
        }
    }
}
