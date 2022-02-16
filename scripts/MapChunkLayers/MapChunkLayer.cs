using System.Collections.Generic;
using Godot;

public abstract class MapChunkLayer<T> : Node2D where T : Tile
{

	#region Fields

	public readonly T[,] TileMap;
	public readonly List<T> AllTiles;

	#endregion // Fields



	#region Constructors

	public MapChunkLayer () : base()
	{
		TileMap = new T[StaticGameData.MapChunkHeightInTiles, StaticGameData.MapChunkWidthInTiles];
		AllTiles = new List<T>();
	}

	#endregion // Constructors



	#region Node2D methods

	public override void _Ready ()
	{
		foreach (T tile in GetChildren())
		{
			int x = (int)tile.Position.x / StaticGameData.TileWidth;
			int y = (int)tile.Position.y / StaticGameData.TileHeight;
			AddTile(x, y, tile);
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
		tile.Position = new Vector2(x * StaticGameData.TileWidth, y * StaticGameData.TileHeight);
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
		tile.Position = new Vector2(toX * StaticGameData.TileWidth, toY * StaticGameData.TileHeight);
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
		for (int y = 0; y < StaticGameData.MapChunkHeightInTiles; y++)
		{
			for (int x = 0; x < StaticGameData.MapChunkWidthInTiles; x++)
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
