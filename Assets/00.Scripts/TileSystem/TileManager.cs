using System;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private Dictionary<Vector2Int, Tile> _tiles = new();
    [SerializeField, TextArea]
    private string _tileMapStr = string.Empty;
    [SerializeField]
    private SGenerateTileData _generateTileData;
    private TileGenerator _tileGenerator = null;

    private void Start()
    {
        GenerateTileMap();
    }

    private void GenerateTileMap()
    {
        _tileGenerator = new TileGenerator(_generateTileData);
        _tileGenerator.GenerateTile(_tiles, _tileMapStr, Vector2Int.zero);
    }
}
