using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private TileSetParser mTileSetParser;

    [SerializeField]
    private AStarGrid mGrid;

    public override void Awake()
    {
        mTileSetParser = new TileSetParser(10, 10);
        mTileSetParser.ReadTileData(Application.dataPath + TileSetParser.LEVEL_ASSETS_DIR + "testlevel.txt");
    }

    void Start ()
    {
		if(mGrid != null)
        {
            mGrid.IntializeGrid(10, 10, 1, mTileSetParser.TileData);
            AStarSearch search = new AStarSearch(mGrid.Elements[0,0], mGrid.Elements[9,9], mGrid, AStarSearch.eHeuristicType.DiagonalShortcut);
            search.BeginSearch();
        }
	}

	void Update ()
    {
		
	}
}
