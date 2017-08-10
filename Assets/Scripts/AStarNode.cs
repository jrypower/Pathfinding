using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : MonoBehaviour
{
    [SerializeField]
    private bool mIsTraversable;
    public bool IsTraversable { get { return mIsTraversable; } }
    [SerializeField]
    private int mGridX;
    public int GridX { get { return mGridX; } }
    [SerializeField]
    private int mGridY;
    public int GridY { get { return mGridY; } }

    private int mG;
    public int G { get { return mG; } }
    private int mH;
    public int H { get { return mH; } }
    private int mF;
    public int F { get { return mF; } }

    private bool mIsDiagonal;
    public bool IsDiagonal { get { return mIsDiagonal; } }

    private AStarNode mParentNode;
    public AStarNode ParentNode { get { return mParentNode; } set { mParentNode = value; } }

    private List<AStarNode> mAdjacentNodes;
    public List<AStarNode> AdjacentNodes { get { return mAdjacentNodes; } }

    private void Awake()
    { 
        mAdjacentNodes = new List<AStarNode>();

        mG = int.MaxValue;
        mH = int.MaxValue;
        mF = int.MaxValue;
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void DebugSetStartTile()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void DebugSetEndTile()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void DebugSetPathTile()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void DebugSetCurrentTile()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public void SetGridData(int x, int y, bool isTraversable)
    {
        mGridX = x;
        mGridY = y;
        mIsTraversable = isTraversable;
    }

    public void SetAdjacentNodes(List<AStarNode> nodes)
    {
        mAdjacentNodes.Clear();
        mAdjacentNodes.AddRange(nodes);
    }

    public void CalculateCosts(int h, AStarNode parent)
    {
        if (mParentNode != null)
        {
            mH = h;
            mIsDiagonal = (Mathf.Abs(mGridX - mParentNode.GridX) != 0 && Mathf.Abs(mGridY - mParentNode.GridY) != 0);
            mG = (mIsDiagonal) ? AStarSearch.DIAGONAL_MOVEMENT_COST : AStarSearch.DIRECT_MOVEMENT_COST;
            mG += parent.G;
            mF = mH + mG;
        }
        else
        {
            mH = h;
            mG = 0;
            mF = mH + mG;
        }
    }
}
