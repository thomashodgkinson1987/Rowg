using Godot;
using Rowg.Maps.Chunks;
using Rowg.Tiles.Views;
using System.Collections.Generic;
using System.Linq;

namespace Rowg.Maps
{

	public class Map : Node2D
	{

		#region Signals

		[Signal] public delegate void BoundsChanged ();

		#endregion // Signals



		#region Properties

		public List<FloorTileView> AllFloorTiles
		{
			get
			{
				List<FloorTileView> tiles = new List<FloorTileView>();
				AllChunks.ForEach(_ => tiles.AddRange(_.AllFloorTiles));
				return tiles;
			}
		}
		public List<FurnitureTileView> AllFurnitureTiles
		{
			get
			{
				List<FurnitureTileView> tiles = new List<FurnitureTileView>();
				AllChunks.ForEach(_ => tiles.AddRange(_.AllFurnitureTiles));
				return tiles;
			}
		}
		public List<ItemTileView> AllItemTiles
		{
			get
			{
				List<ItemTileView> tiles = new List<ItemTileView>();
				AllChunks.ForEach(_ => tiles.AddRange(_.AllItemTiles));
				return tiles;
			}
		}
		public List<DoorTileView> AllDoorTiles
		{
			get
			{
				List<DoorTileView> tiles = new List<DoorTileView>();
				AllChunks.ForEach(_ => tiles.AddRange(_.AllDoorTiles));
				return tiles;
			}
		}
		public List<WallTileView> AllWallTiles
		{
			get
			{
				List<WallTileView> tiles = new List<WallTileView>();
				AllChunks.ForEach(_ => tiles.AddRange(_.AllWallTiles));
				return tiles;
			}
		}
		public List<ActorTileView> AllActorTiles
		{
			get
			{
				List<ActorTileView> tiles = new List<ActorTileView>();
				AllChunks.ForEach(_ => tiles.AddRange(_.AllActorTiles));
				return tiles;
			}
		}

		#endregion // Properties



		#region Fields

		public readonly List<Chunk> AllChunks;

		public readonly List<Chunk> LoadedChunks;

		public readonly List<FloorTileView> LoadedFloorTiles;
		public readonly List<FurnitureTileView> LoadedFurnitureTiles;
		public readonly List<ItemTileView> LoadedItemTiles;
		public readonly List<DoorTileView> LoadedDoorTiles;
		public readonly List<WallTileView> LoadedWallTiles;
		public readonly List<ActorTileView> LoadedActorTiles;

		public Rect2 Bounds;

		#endregion // Fields



		#region Constructors

		public Map () : base()
		{
			AllChunks = new List<Chunk>();
			LoadedChunks = new List<Chunk>();
			LoadedFloorTiles = new List<FloorTileView>();
			LoadedFurnitureTiles = new List<FurnitureTileView>();
			LoadedItemTiles = new List<ItemTileView>();
			LoadedDoorTiles = new List<DoorTileView>();
			LoadedWallTiles = new List<WallTileView>();
			LoadedActorTiles = new List<ActorTileView>();
			Bounds = new Rect2();
		}

		#endregion // Constructors



		#region Node2D methods

		public override void _Ready ()
		{
			foreach (object obj in GetChildren())
			{
				if (obj is Chunk chunk)
				{
					chunk.X = (int)chunk.Position.x / StaticGameData.ChunkWidthInPixels;
					chunk.Y = (int)chunk.Position.y / StaticGameData.ChunkWidthInPixels;
					AllChunks.Add(chunk);
					RemoveChild(chunk);
				}
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

		public void AddChunk (int x, int y, Chunk chunk)
		{
			chunk.X = x;
			chunk.Y = y;
			chunk.Position = new Vector2(x * StaticGameData.ChunkWidthInPixels, y * StaticGameData.MapChunkHeightInPixels);
			AllChunks.Add(chunk);

			RecalculateBounds();
		}
		public void RemoveChunk (int x, int y)
		{
			UnloadChunk(x, y);
			GetChunk(x, y).QueueFree();

			RecalculateBounds();
		}
		public Chunk GetChunk (int x, int y)
		{
			return AllChunks.Find(_ => _.X == x && _.Y == y);
		}
		public bool IsChunk (int x, int y)
		{
			return GetChunk(x, y) != null;
		}
		public void LoadChunk (int x, int y)
		{
			if (!IsChunkLoaded(x, y))
			{
				Chunk chunk = GetChunk(x, y);

				LoadedChunks.Add(chunk);
				LoadedFloorTiles.AddRange(chunk.AllFloorTiles);
				LoadedFurnitureTiles.AddRange(chunk.AllFurnitureTiles);
				LoadedItemTiles.AddRange(chunk.AllItemTiles);
				LoadedDoorTiles.AddRange(chunk.AllDoorTiles);
				LoadedWallTiles.AddRange(chunk.AllWallTiles);
				LoadedActorTiles.AddRange(chunk.AllActorTiles);

				AddChild(chunk);
			}
		}
		public void UnloadChunk (int x, int y)
		{
			if (IsChunkLoaded(x, y))
			{
				Chunk chunk = GetChunk(x, y);

				LoadedChunks.Remove(chunk);
				LoadedFloorTiles.RemoveAll(_ => chunk.AllFloorTiles.Contains(_));
				LoadedFurnitureTiles.RemoveAll(_ => chunk.AllFurnitureTiles.Contains(_));
				LoadedItemTiles.RemoveAll(_ => chunk.AllItemTiles.Contains(_));
				LoadedDoorTiles.RemoveAll(_ => chunk.AllDoorTiles.Contains(_));
				LoadedWallTiles.RemoveAll(_ => chunk.AllWallTiles.Contains(_));
				LoadedActorTiles.RemoveAll(_ => chunk.AllActorTiles.Contains(_));

				RemoveChild(chunk);
			}
		}

		public bool IsChunkLoaded (int x, int y)
		{
			Chunk chunk = GetChunk(x, y);
			return LoadedChunks.Contains(chunk);
		}

		//public void AddFloorTile (int x, int y, FloorTile floor) { }
		//public void AddFurnitureTile (int x, int y, FurnitureTile floor) { }
		//public void AddItemTile (int x, int y, ItemTile item) { }
		//public void AddDoorTile (int x, int y, DoorTile door) { }
		//public void AddWallTile (int x, int y, WallTile wall) { }
		//public void AddActorTile (int x, int y, ActorTile actor) { }

		//public void RemoveFloorTile (int x, int y) { }
		//public void RemoveFurnitureTile (int x, int y) { }
		//public void RemoveItemTile (int x, int y) { }
		//public void RemoveDoorTile (int x, int y) { }
		//public void RemoveWallTile (int x, int y) { }
		//public void RemoveActorTile (int x, int y) { }

		//public FloorTile GetFloorTile (int x, int y) { }
		//public FurnitureTile GetFurnitureTile (int x, int y) { }
		//public ItemTile GetItemTile (int x, int y) { }
		//public DoorTile GetDoorTile (int x, int y) { }
		//public WallTile GetWallTile (int x, int y) { }
		//public ActorTile GetActorTile (int x, int y) { }

		//public bool IsFloorTile (int x, int y) { }
		//public bool IsFurnitureTile (int x, int y) { }
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

			int minX = AllChunks.Min(_ => _.X) * StaticGameData.ChunkWidthInPixels;
			int maxX = (AllChunks.Max(_ => _.X) * StaticGameData.ChunkWidthInPixels) + StaticGameData.ChunkWidthInPixels;
			int minY = AllChunks.Min(_ => _.Y) * StaticGameData.MapChunkHeightInPixels;
			int maxY = (AllChunks.Max(_ => _.Y) * StaticGameData.MapChunkHeightInPixels) + StaticGameData.MapChunkHeightInPixels;

			Bounds = new Rect2(minX, minY, maxX - minX, maxY - minY);

			if (oldMinX != minX || oldMaxX != maxX || oldMinY != minY || oldMaxY != maxY)
			{
				EmitSignal(nameof(BoundsChanged));
			}
		}

		public void MoveFloorTile (int dx, int dy, FloorTileView tile) { }
		public void MoveFurnitureTile (int dx, int dt, FurnitureTileView tile) { }
		public void MoveItemTile (int dx, int dy, ItemTileView tile) { }
		public void MoveDoorTile (int dx, int dy, DoorTileView tile) { }
		public void MoveWallTile (int dx, int dy, WallTileView tile) { }
		public void MoveActorTile (int dx, int dy, ActorTileView tile)
		{
			int originalX = (int)tile.Position.x / StaticGameData.TileWidthInPixels;
			int originalY = (int)tile.Position.y / StaticGameData.TileHeightInPixels;

			int projectedX = originalX + dx;
			int projectedY = originalY + dy;

			int originalChunkX = Mathf.FloorToInt(tile.GlobalPosition.x / StaticGameData.ChunkWidthInPixels);
			int originalChunkY = Mathf.FloorToInt(tile.GlobalPosition.y / StaticGameData.MapChunkHeightInPixels);
			Chunk originalChunk = GetChunk(originalChunkX, originalChunkY);

			if (projectedX < 0 || projectedX > StaticGameData.ChunkWidthInTiles - 1 ||
				projectedY < 0 || projectedY > StaticGameData.ChunkHeightInTiles - 1)
			{
				int projectedChunkX = Mathf.FloorToInt((tile.GlobalPosition.x + (dx * StaticGameData.TileWidthInPixels)) / StaticGameData.ChunkWidthInPixels);
				int projectedChunkY = Mathf.FloorToInt((tile.GlobalPosition.y + (dy * StaticGameData.TileHeightInPixels)) / StaticGameData.MapChunkHeightInPixels);

				if (IsChunk(projectedChunkX, projectedChunkY))
				{
					Chunk projectedChunk = GetChunk(projectedChunkX, projectedChunkY);

					int newX = Mathf.Wrap(projectedX, 0, StaticGameData.ChunkWidthInTiles);
					int newY = Mathf.Wrap(projectedY, 0, StaticGameData.ChunkHeightInTiles);

					if (projectedChunk.IsFloorTile(newX, newY) &&
						(!projectedChunk.IsDoorTile(newX, newY) || projectedChunk.GetDoorTile(newX, newY).IsOpen) &&
						!projectedChunk.IsWallTile(newX, newY) &&
						!projectedChunk.IsActorTile(newX, newY))
					{
						originalChunk.RemoveActorTile(originalX, originalY);
						projectedChunk.AddActorTile(newX, newY, tile);

						if (!IsChunkLoaded(projectedChunkX, projectedChunkY))
						{
							LoadedActorTiles.Remove(tile);
						}
					}
				}
			}
			else
			{
				if (originalChunk.IsFloorTile(projectedX, projectedY) &&
					(!originalChunk.IsDoorTile(projectedX, projectedY) || originalChunk.GetDoorTile(projectedX, projectedY).IsOpen) &&
					!originalChunk.IsWallTile(projectedX, projectedY) &&
					!originalChunk.IsActorTile(projectedX, projectedY))
				{
					originalChunk.MoveActorTile(originalX, originalY, projectedX, projectedY);
				}
			}
		}

		#endregion // Public methods

	}

}
