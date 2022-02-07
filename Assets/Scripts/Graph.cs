using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph
{
    public List<Node> nodes;
    public List<Edge> edges;

    public Graph()
    {
        nodes = new List<Node>();
        edges = new List<Edge>();
    }

    public bool Adjacent(Node from, Node to)
    {
        foreach(Edge e in edges)
        {
            if (e.from == from && e.to == to)
                return true;
        }
        return false;
    }

    public List<Node> Neighbors(Node from)
    {
        List<Node> result = new List<Node>();

        foreach (Edge e in edges)
        {
            if (e.from == from)
                result.Add(e.to);
        }
        return result;
    }

    public void AddNode(Vector3 worldPosition, Vector3 mapPosition)
    {
        nodes.Add(new Node(nodes.Count, worldPosition, mapPosition));
    }

    public void AddEdge(Node from, Node to)
    {
        edges.Add(new Edge(from, to, 1));
    }

    public float Distance(Node from, Node to)
    {
        foreach (Edge e in edges)
        {
            if (e.from == from && e.to == to)
                return e.GetWeight();
        }

        return Mathf.Infinity;
    }

    public List<Node> GetShortestPath(Node start, Node end)
    {
        List<Node> path = new List<Node>();

        if(start == end)
        {
            path.Add(start);
            return path;
        }

        List<Node> openList = new List<Node>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
        Dictionary<Node, float> distances = new Dictionary<Node, float>();

        for(int i = 0; i<nodes.Count; i++)
        {
            openList.Add(nodes[i]);

            distances.Add(nodes[i],float.PositiveInfinity); //Default distance is infinity
        }

        distances[start] = 0f; //Distance from the same node is zero

        while(openList.Count > 0)
        {
            //Get the node with smaller distance
            openList.OrderBy(x => distances[x]).ToList();
            Node current = openList[0];
            openList.Remove(current);

            if(current == end)
            {
                //Done!
                //Build Path
                while(previous.ContainsKey(current))
                {
                    path.Insert(0, current);
                    current = previous[current];

                }

                //Add the start node too
                path.Insert(0, current);
                break;
            }

            foreach(Node neighbor in Neighbors(current))
            {
                float distance = Distance(current, neighbor);
                
                float candidateNewDistance = distances[current] + distance;

                //Is new path shorter?
                if(candidateNewDistance < distances[neighbor])
                {
                    distances[neighbor] = candidateNewDistance;
                    previous[neighbor] = current;
                }
            }
        }

        return path;
    }
}

public class Node
{
    public int index;
    public Vector3 worldPosition;
    public Vector3 mapPosition;

    private bool occupied = false;

    public Node(int index, Vector3 worldPosition, Vector3 mapPosition)
    {
        this.index = index;
        this.worldPosition = worldPosition;
        this.mapPosition = mapPosition;
        occupied = false;
    }

    public void SetOccupied(bool val)
    {
        occupied = val;
    }

    public bool IsOccupied => occupied;
}

public class Edge
{
    public Node from;
    public Node to;

    private float weight;

    public Edge(Node from, Node to, float weight)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
    }

    public float GetWeight()
    {
        if (to.IsOccupied)
            return Mathf.Infinity;

        return weight;
    }
}