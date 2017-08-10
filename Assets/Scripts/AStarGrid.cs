using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class AStarGrid : MonoBehaviour
{
    private int mGridWidth;
    private int mGridHeight;

    private int mElementSize;

    private AStarNode[,] mElements;
    public AStarNode[,] Elements { get { return mElements; } }

    [SerializeField]
    private GameObject mElementPrefab;

    [SerializeField]
    private GameObject mWallPrefab;

    [SerializeField]
    private Transform mGridOrigin;

    public void IntializeGrid(int width, int height, int size, char[,] levelData)
    {
        mGridHeight = height;
        mGridWidth = width;
        mElementSize = size;

        mElements = new AStarNode[mGridWidth, mGridHeight];

        for(int x = 0; x < mGridWidth; ++x)
        {
            for (int y = 0; y < mGridHeight; ++y)
            {
                // GameObject spawn - TODO: Position, Rotation, based on gridmath - this assumes a uniform element size
                Vector3 elementPos = new Vector3(mGridOrigin.transform.position.x + (x * mElementSize), mGridOrigin.transform.position.y + (y * mElementSize), 0.0f);

                GameObject spawnedElement = GameObject.Instantiate<GameObject>(GetPrefabFromChar(levelData[x,y]), elementPos, mGridOrigin.rotation, mGridOrigin);
                if(spawnedElement != null && spawnedElement.GetComponent<AStarNode>() != null)
                {
                    mElements[x, y] = spawnedElement.GetComponent<AStarNode>();
                    mElements[x, y].name = string.Format("[{0},{1}]", x, y);
                    // TODO: When I grab this from a map or data source, put the correct data here for is traversable.
                    mElements[x, y].SetGridData(x, y, spawnedElement.GetComponent<AStarNode>().IsTraversable);
                }
            }
        }

        CalculateAdjacency();
    }

    private void CalculateAdjacency()
    {
        List<AStarNode> adjacentNodes = new List<AStarNode>();
        for (int x = 0; x < mElements.GetLength(0); ++x)
        {
            for (int y = 0; y < mElements.GetLength(1); ++y)
            {
                adjacentNodes.Clear();

                for (int x0 = x - 1; x0 <= x + 1; ++x0)
                {
                    for (int y0 = y - 1; y0 <= y + 1; ++y0)
                    {
                        if ((x0 != x || y0 != y)) // don't include THIS node
                        {
                            if (x0 >= 0 && y0 >= 0 && x0 < mElements.GetLength(0) && y0 <mElements.GetLength(1)) // don't include nodes that fall out of bounds on the inner side
                            {
                                adjacentNodes.Add(mElements[x0, y0]);
                            }
                        }

                    }
                }
                mElements[x, y].SetAdjacentNodes(adjacentNodes);
            }
        }
    }

    private GameObject GetPrefabFromChar(char data)
    {
        if(data == 'f')
        {
            return mElementPrefab;
        }
        else
        {
            return mWallPrefab;
        }
    }
}
