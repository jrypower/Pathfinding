using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class TileSetParser
{
    public static readonly string LEVEL_ASSETS_DIR = "/LevelData/";
    private static readonly char DEBUG_TILE_CHAR = '~';

    private char[,] mTileData;
    public char[,] TileData { get { return mTileData; } }

    public TileSetParser(int height, int width)
    {
        mTileData = new char[height, width];

        for (int x = 0; x < mTileData.GetLength(0); ++x)
        {
            for (int y = 0; y < mTileData.GetLength(1); ++y)
            {
                mTileData[x, y] = DEBUG_TILE_CHAR;
            }
        }
    }

    public bool ReadTileData(string filePath)
    {
        try
        {
            StreamReader reader = new StreamReader(filePath, Encoding.ASCII);

            using (reader)
            {
                string line = reader.ReadLine();
                int yIndex = mTileData.GetLength(1) - 1;
                while (line != null)
                {
                    char[] input = line.ToCharArray();
                    for (int i = 0; i < input.Length; ++i)
                    {
                        mTileData[i, yIndex] = input[i];
                    }
                    line = reader.ReadLine();

                    yIndex--;
                }

                reader.Close();
                return true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }
}