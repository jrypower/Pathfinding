using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO:
/// * Use an async call or an event to let the gameobject know I have a result?
/// * Implement the search
/// * Tie to game objects
/// </summary>
public class AStarSearch
{
    public static readonly int DIRECT_MOVEMENT_COST = 10;
    public static readonly int DIAGONAL_MOVEMENT_COST = 14;

    public enum eHeuristicType
    {
        ManHattan = 0,
        DiagonalShortcut
    }

    private AStarNode mStartNode;
    private AStarNode mEndNode;
    private AStarGrid mNodeGrid;
    private eHeuristicType mHeuristic;

    private List<AStarNode> mOpenSet;
    private List<AStarNode> mClosedSet;

    public AStarSearch(AStarNode start, AStarNode end, AStarGrid grid, eHeuristicType heuristic = eHeuristicType.ManHattan)
    {
        mStartNode = start;
        mEndNode = end;
        mNodeGrid = grid;
        mHeuristic = heuristic;

        mOpenSet = new List<AStarNode>();
        mClosedSet = new List<AStarNode>();
    }

    public bool BeginSearch()
    {
        mOpenSet.Clear();
        mClosedSet.Clear();

        mStartNode.DebugSetStartTile();
        mEndNode.DebugSetEndTile();

        mOpenSet.Add(mStartNode);
        AStarNode currentNode = mStartNode;
        currentNode.CalculateCosts(CalculateHeuristic(currentNode.GridX, mEndNode.GridX, currentNode.GridY, mEndNode.GridY), null);
        while (mOpenSet.Count > 0 && currentNode != mEndNode)
        {
            int lowestF = mOpenSet[0].F;
            for (int i = 0; i < mOpenSet.Count; i++)
            {
                if (mOpenSet[i].F <= lowestF)
                {
                    lowestF = mOpenSet[i].F;
                    currentNode = mOpenSet[i];
                }
            }

            if (currentNode != null)
            {
                currentNode.DebugSetCurrentTile();
                mOpenSet.Remove(currentNode);
                if (!mClosedSet.Contains(currentNode))
                {
                    mClosedSet.Add(currentNode);
                }

                if (currentNode == mEndNode)
                {
                    // we have the node!
                    break;
                }

                List<AStarNode> adjacentNodes = currentNode.AdjacentNodes;
                for (int i = 0; i < adjacentNodes.Count; i++)
                {
                    AStarNode adjacentNode = adjacentNodes[i];
                    if (adjacentNode.IsTraversable && !mClosedSet.Contains(adjacentNode))
                    {
                        if (!mOpenSet.Contains(adjacentNode))
                        {
                            mOpenSet.Add(adjacentNode);

                            adjacentNode.ParentNode = currentNode;

                            int h = CalculateHeuristic(adjacentNode.GridX, mEndNode.GridX, adjacentNode.GridY, mEndNode.GridY);
                            adjacentNode.CalculateCosts(h, currentNode);
                        }
                        else
                        {
                            bool isNewPathDiagonal = (Mathf.Abs(adjacentNode.GridX - currentNode.GridX) != 0 && Mathf.Abs(adjacentNode.GridY - currentNode.GridY) != 0);
                            if (adjacentNode.G > currentNode.G + (isNewPathDiagonal ? AStarSearch.DIAGONAL_MOVEMENT_COST : AStarSearch.DIRECT_MOVEMENT_COST))
                            {
                                // this is a better path!
                                adjacentNode.ParentNode = currentNode;
                                int h = CalculateHeuristic(adjacentNode.GridX, mEndNode.GridX, adjacentNode.GridY, mEndNode.GridY);
                                adjacentNode.CalculateCosts(h, currentNode);
                            }
                        }
                    }
                }
            }
            else
            {
                // no path.
                return false;
            }
        }

        AStarNode lastNode = currentNode;
        while (lastNode.ParentNode != null)
        {
            lastNode.DebugSetPathTile();
            Debug.Log(string.Format("Tile: [{0},{1}] has parent tile: [{2},{3}]", lastNode.GridX, lastNode.GridY, lastNode.ParentNode.GridX, lastNode.ParentNode.GridY));
            lastNode = lastNode.ParentNode;
        }

        return true;
    }

    private int CalculateHeuristic(int x0, int x1, int y0, int y1)
    {
        int h = 0;
        switch(mHeuristic)
        {
            case eHeuristicType.ManHattan:
                h = DIRECT_MOVEMENT_COST * (Mathf.Abs(x0 - x1) + Mathf.Abs(y0 - y1));
                break;
            case eHeuristicType.DiagonalShortcut:
                int deltaX = Mathf.Abs(x0 - x1);
                int deltaY = Mathf.Abs(y0 - y1);

                h = DIRECT_MOVEMENT_COST * (deltaX + deltaY) + (DIAGONAL_MOVEMENT_COST - (2 * DIRECT_MOVEMENT_COST)) * Mathf.Min(deltaX, deltaY);
                break;
            default:
                break;
        }

        return h;
    }
}
