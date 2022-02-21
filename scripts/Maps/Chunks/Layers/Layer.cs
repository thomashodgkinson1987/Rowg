using Godot;
using Rowg.Tiles.Views;
using System.Collections.Generic;

namespace Rowg.Maps.Chunks.Layers
{

    public class Layer<T> : Node2D where T : TileView
    {

        #region Fields

        public readonly T[,] TileMap;
        public readonly List<T> AllTiles;

        #endregion // Fields



        #region Constructors

        public Layer () : base()
        {
            TileMap = new T[StaticGameData.ChunkHeightInTiles, StaticGameData.ChunkWidthInTiles];
            AllTiles = new List<T>();
        }

        #endregion // Constructors



        #region Node2D methods

        public override void _Ready ()
        {
            foreach (object obj in GetChildren())
            {
                if (obj is T tile)
                {
                    int x = (int)tile.Position.x / StaticGameData.TileWidthInPixels;
                    int y = (int)tile.Position.y / StaticGameData.TileHeightInPixels;
                    AddTile(x, y, tile);
                }
            }
        }

        #endregion // Node2D methods



        #region Public methods

        public void AddTile (int x, int y, T tile)
        {
            AllTiles.Add(tile);
            TileMap[y, x] = tile;
            if (tile.GetParent() == null)
            {
                AddChild(tile);
            }
            tile.Position = new Vector2(x * StaticGameData.TileWidthInPixels, y * StaticGameData.TileHeightInPixels);
        }

        public void RemoveTile (int x, int y)
        {
            T tile = GetTile(x, y);
            AllTiles.Remove(tile);
            TileMap[y, x] = null;
            RemoveChild(tile);
        }

        public void MoveTile (int fromX, int fromY, int toX, int toY)
        {
            T tile = TileMap[fromY, fromX];
            tile.Position = new Vector2(toX * StaticGameData.TileWidthInPixels, toY * StaticGameData.TileHeightInPixels);
            TileMap[toY, toX] = tile;
            TileMap[fromY, fromX] = null;
        }

        public T GetTile (int x, int y)
        {
            return TileMap[y, x];
        }

        public bool IsTile (int x, int y)
        {
            return TileMap[y, x] != null;
        }

        public void Clear ()
        {
            for (int y = 0; y < StaticGameData.ChunkHeightInTiles; y++)
            {
                for (int x = 0; x < StaticGameData.ChunkWidthInTiles; x++)
                {
                    if (IsTile(x, y))
                    {
                        RemoveTile(x, y);
                    }
                }
            }
        }

        #endregion // Public methods

    }

}
