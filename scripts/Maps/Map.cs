using System.Collections.Generic;
using System.Linq;
using Godot;

public class Map : Node2D
{

	#region Signals

	[Signal] public delegate void BoundsChanged ();

	#endregion // Signals



	#region Properties

	public List<FloorTile> AllFloorTiles
	{
		get
		{
			List<FloorTile> tiles = new List<FloorTile>();
			AllMapChunks.ForEach(_ => tiles.AddRange(_.AllFloorTiles));
			return tiles;
		}
	}
	public List<ItemTile> AllItemTiles
	{
		get
		{
			List<ItemTile> tiles = new List<ItemTile>();
			AllMapChunks.ForEach(_ => tiles.AddRange(_.AllItemTiles));
			return tiles;
		}
	}
	public List<DoorTile> AllDoorTiles
	{
		get
		{
			List<DoorTile> tiles = new List<DoorTile>();
			AllMapChunks.ForEach(_ => tiles.AddRange(_.AllDoorTiles));
			return tiles;
		}
	}
	public List<WallTile> AllWallTiles
	{
		get
		{
			List<WallTile> tiles = new List<WallTile>();
			AllMapChunks.ForEach(_ => tiles.AddRange(_.AllWallTiles));
			return tiles;
		}
	}
	public List<ActorTile> AllActorTiles
	{
		get
		{
			List<ActorTile> tiles = new List<ActorTile>();
			AllMapChunks.ForEach(_ => tiles.AddRange(_.AllActorTiles));
			return tiles;
		}
	}

	#endregion // Properties



	#region Fields

	public readonly List<MapChunk> AllMapChunks;

	public readonly List<MapChunk> LoadedMapChunks;

	public readonly List<FloorTile> LoadedFloorTiles;
	public readonly List<ItemTile> LoadedItemTiles;
	public readonly List<DoorTile> LoadedDoorTiles;
	public readonly List<WallTile> LoadedWallTiles;
	public readonly List<ActorTile> LoadedActorTiles;

	public Rect2 Bounds;

	#endregion // Fields



	#region Constructors

	public Map () : base()
	{
		AllMapChunks = new List<MapChunk>();
		LoadedMapChunks = new List<MapChunk>();
		LoadedFloorTiles = new List<FloorTile>();
		LoadedItemTiles = new List<ItemTile>();
		LoadedDoorTiles = new List<DoorTile>();
		LoadedWallTiles = new List<WallTile>();
		LoadedActorTiles = new List<ActorTile>();
		Bounds = new Rect2();
	}

	#endregion // Constructors



	#region Node2D methods

	public override void _Ready ()
	{
		foreach (MapChunk mapChunk in GetChildren())
		{
			mapChunk.X = (int)mapChunk.Position.x / StaticGameData.MapChunkWidthInPixels;
			mapChunk.Y = (int)mapChunk.Position.y / StaticGameData.MapChunkWidthInPixels;
			AllMapChunks.Add(mapChunk);
			RemoveChild(mapChunk);
		}

		RecalculateBounds();
	}

	#endregion // Node2D methods



	#region Public methods

	public virtual void InputTick (InputEvent @event)
	{
		LoadedActorTiles.ForEach(_ => _.InputTick(@event, this));
	}
	public virtual void FixedTick (float delta)
	{
		LoadedActorTiles.ForEach(_ => _.FixedTick(delta, this));
	}
	public virtual void Tick (float delta)
	{
		LoadedActorTiles.ForEach(_ => _.Tick(delta, this));
	}

	public MapChunk CreateMapChunk (int x, int y)
	{
		PackedScene mapChunkPackedScene = GD.Load<PackedScene>("res://packed_scenes/map_chunks/MapChunk.tscn");
		MapChunk mapChunk = mapChunkPackedScene.Instance<MapChunk>();
		AddChild(mapChunk);
		AddMapChunk(x, y, mapChunk);

		return mapChunk;
	}
	public void AddMapChunk (int x, int y, MapChunk mapChunk)
	{
		mapChunk.X = x;
		mapChunk.Y = y;
		mapChunk.Position = new Vector2(x * StaticGameData.MapChunkWidthInPixels, y * StaticGameData.MapChunkHeightInPixels);
		AllMapChunks.Add(mapChunk);

		RecalculateBounds();
	}
	public void RemoveMapChunk (int x, int y)
	{
		UnloadMapChunk(x, y);
		GetMapChunk(x, y).QueueFree();

		RecalculateBounds();
	}
	public MapChunk GetMapChunk (int x, int y)
	{
		return AllMapChunks.Find(_ => _.X == x && _.Y == y);
	}
	public bool IsMapChunk (int x, int y)
	{
		return GetMapChunk(x, y) != null;
	}
	public void LoadMapChunk (int x, int y)
	{
		if (!IsMapChunkLoaded(x, y))
		{
			MapChunk mapChunk = GetMapChunk(x, y);

			LoadedMapChunks.Add(mapChunk);
			LoadedFloorTiles.AddRange(mapChunk.AllFloorTiles);
			LoadedItemTiles.AddRange(mapChunk.AllItemTiles);
			LoadedDoorTiles.AddRange(mapChunk.AllDoorTiles);
			LoadedWallTiles.AddRange(mapChunk.AllWallTiles);
			LoadedActorTiles.AddRange(mapChunk.AllActorTiles);

			AddChild(mapChunk);
		}
	}
	public void UnloadMapChunk (int x, int y)
	{
		if (IsMapChunkLoaded(x, y))
		{
			MapChunk mapChunk = GetMapChunk(x, y);

			LoadedMapChunks.Remove(mapChunk);
			LoadedFloorTiles.RemoveAll(_ => mapChunk.AllFloorTiles.Contains(_));
			LoadedItemTiles.RemoveAll(_ => mapChunk.AllItemTiles.Contains(_));
			LoadedDoorTiles.RemoveAll(_ => mapChunk.AllDoorTiles.Contains(_));
			LoadedWallTiles.RemoveAll(_ => mapChunk.AllWallTiles.Contains(_));
			LoadedActorTiles.RemoveAll(_ => mapChunk.AllActorTiles.Contains(_));

			RemoveChild(mapChunk);
		}
	}

	public bool IsMapChunkLoaded (int x, int y)
	{
		MapChunk mapChunk = GetMapChunk(x, y);
		return LoadedMapChunks.Contains(mapChunk);
	}

	//public void AddFloorTile (int x, int y, FloorTile floor) { }
	//public void AddItemTile (int x, int y, ItemTile item) { }
	//public void AddDoorTile (int x, int y, DoorTile door) { }
	//public void AddWallTile (int x, int y, WallTile wall) { }
	//public void AddActorTile (int x, int y, ActorTile actor) { }

	//public void RemoveFloorTile (int x, int y) { }
	//public void RemoveItemTile (int x, int y) { }
	//public void RemoveDoorTile (int x, int y) { }
	//public void RemoveWallTile (int x, int y) { }
	//public void RemoveActorTile (int x, int y) { }

	//public void RemoveFloorTile (FloorTile floor) { }
	//public void RemoveItemTile (ItemTile item) { }
	//public void RemoveDoorTile (DoorTile door) { }
	//public void RemoveWallTile (WallTile wall) { }
	//public void RemoveActorTile (ActorTile actor) { }

	//public FloorTile GetFloorTile (int x, int y) { }
	//public ItemTile GetItemTile (int x, int y) { }
	//public DoorTile GetDoorTile (int x, int y) { }
	//public WallTile GetWallTile (int x, int y) { }
	//public ActorTile GetActorTile (int x, int y) { }

	//public void SetFloorTile (int x, int y, FloorTile floor) { }
	//public void SetItemTile (int x, int y, ItemTile item) { }
	//public void SetDoorTile (int x, int y, DoorTile door) { }
	//public void SetWallTile (int x, int y, WallTile wall) { }
	//public void SetActorTile (int x, int y, ActorTile actor) { }

	//public bool IsFloorTile (int x, int y) { }
	//public bool IsItemTile (int x, int y) { }
	//public bool IsDoorTile (int x, int y) { }
	//public bool IsWallTile (int x, int y) { }
	//public bool IsActorTile (int x, int y) { }

	public void RecalculateBounds ()
	{
		Rect2 oldBounds = Bounds;

		int oldMinX = (int)Bounds.Position.x;
		int oldMaxX = (int)Bounds.End.x;
		int oldMinY = (int)Bounds.Position.y;
		int oldMaxY = (int)Bounds.End.y;

		int minX = AllMapChunks.Min(_ => _.X) * StaticGameData.MapChunkWidthInPixels;
		int maxX = (AllMapChunks.Max(_ => _.X) * StaticGameData.MapChunkWidthInPixels) + StaticGameData.MapChunkWidthInPixels;
		int minY = AllMapChunks.Min(_ => _.Y) * StaticGameData.MapChunkHeightInPixels;
		int maxY = (AllMapChunks.Max(_ => _.Y) * StaticGameData.MapChunkHeightInPixels) + StaticGameData.MapChunkHeightInPixels;

		Bounds = new Rect2(minX, minY, maxX - minX, maxY - minY);

		if (oldMinX != minX || oldMaxX != maxX || oldMinY != minY || oldMaxY != maxY)
		{
			EmitSignal(nameof(BoundsChanged));
		}
	}

	public void MoveFloorTile (int dx, int dy, FloorTile tile) { }
	public void MoveItemTile (int dx, int dy, ItemTile tile) { }
	public void MoveDoorTile (int dx, int dy, DoorTile tile) { }
	public void MoveWallTile (int dx, int dy, WallTile tile) { }
	public void MoveActorTile (int dx, int dy, ActorTile tile)
	{
		int originalX = (int)tile.Position.x / StaticGameData.TileWidth;
		int originalY = (int)tile.Position.y / StaticGameData.TileHeight;

		int projectedX = originalX + dx;
		int projectedY = originalY + dy;

		int originalMapChunkX = Mathf.FloorToInt(tile.GlobalPosition.x / StaticGameData.MapChunkWidthInPixels);
		int originalMapChunkY = Mathf.FloorToInt(tile.GlobalPosition.y / StaticGameData.MapChunkHeightInPixels);
		MapChunk originalMapChunk = GetMapChunk(originalMapChunkX, originalMapChunkY);

		if (projectedX < 0 || projectedX > StaticGameData.MapChunkWidthInTiles - 1 ||
			projectedY < 0 || projectedY > StaticGameData.MapChunkHeightInTiles - 1)
		{
			int projectedMapChunkX = (int)((tile.GlobalPosition.x + (dx * StaticGameData.TileWidth)) - (tile.Position.x + (dx * StaticGameData.TileWidth))) / StaticGameData.MapChunkWidthInPixels;
			int projectedMapChunkY = (int)((tile.GlobalPosition.y + (dy * StaticGameData.TileHeight)) - (tile.Position.y + (dy * StaticGameData.TileHeight))) / StaticGameData.MapChunkHeightInPixels;

			if (IsMapChunk(projectedMapChunkX, projectedMapChunkY))
			{
				MapChunk projectedMapChunk = GetMapChunk(projectedMapChunkX, projectedMapChunkY);

				int newX = Mathf.Wrap(projectedX, 0, StaticGameData.MapChunkWidthInTiles - 1);
				int newY = Mathf.Wrap(projectedY, 0, StaticGameData.MapChunkHeightInTiles - 1);

				if (projectedMapChunk.IsFloorTile(newX, newY) &&
					(!projectedMapChunk.IsDoorTile(newX, newY) || projectedMapChunk.GetDoorTile(newX, newY).IsOpen) &&
					!projectedMapChunk.IsWallTile(newX, newY) &&
					!projectedMapChunk.IsActorTile(newX, newY))
				{
					originalMapChunk.RemoveActorTile(originalX, originalY);
					projectedMapChunk.AddActorTile(projectedX, projectedY, tile);
				}

				if (!IsMapChunkLoaded(projectedMapChunkX, projectedMapChunkY))
				{
					LoadedActorTiles.Remove(tile);
				}
			}
		}
		else
		{
			if (originalMapChunk.IsFloorTile(projectedX, projectedY) &&
				(!originalMapChunk.IsDoorTile(projectedX, projectedY) || originalMapChunk.GetDoorTile(projectedX, projectedY).IsOpen) &&
				!originalMapChunk.IsWallTile(projectedX, projectedY) &&
				!originalMapChunk.IsActorTile(projectedX, projectedY))
			{
				originalMapChunk.MoveActorTile(originalX, originalY, projectedX, projectedY);
			}
		}
	}

	#endregion // Public methods

}
