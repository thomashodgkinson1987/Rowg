using Godot;
using Rowg.Maps.Chunks.Layers;
using Rowg.Tiles.Views;
using System.Collections.Generic;

namespace Rowg.Maps.Chunks
{

	public class Chunk : Node2D
	{

		#region Nodes

		private FloorLayer node_layers_floorTiles;
		private FurnitureLayer node_layers_furnitureTiles;
		private ItemLayer node_layers_itemTiles;
		private DoorLayer node_layers_doorTiles;
		private WallLayer node_layers_wallTiles;
		private ActorLayer node_layers_actorTiles;

		#endregion // Nodes



		#region Properties

		public List<FloorTileView> AllFloorTiles => node_layers_floorTiles.AllTiles;
		public List<FurnitureTileView> AllFurnitureTiles => node_layers_furnitureTiles.AllTiles;
		public List<ItemTileView> AllItemTiles => node_layers_itemTiles.AllTiles;
		public List<DoorTileView> AllDoorTiles => node_layers_doorTiles.AllTiles;
		public List<WallTileView> AllWallTiles => node_layers_wallTiles.AllTiles;
		public List<ActorTileView> AllActorTiles => node_layers_actorTiles.AllTiles;

		#endregion // Properties



		#region Fields

		public int X;
		public int Y;

		#endregion // Fields



		#region Constructors

		public Chunk () : base()
		{
			X = 0;
			Y = 0;
		}

		#endregion // Constructors



		#region Node2D methods

		public override void _EnterTree ()
		{
			node_layers_floorTiles = GetNode<FloorLayer>("FloorLayer");
			node_layers_furnitureTiles = GetNode<FurnitureLayer>("FurnitureLayer");
			node_layers_itemTiles = GetNode<ItemLayer>("ItemLayer");
			node_layers_doorTiles = GetNode<DoorLayer>("DoorLayer");
			node_layers_wallTiles = GetNode<WallLayer>("WallLayer");
			node_layers_actorTiles = GetNode<ActorLayer>("ActorLayer");
		}

		#endregion // Node2D methods



		#region Public methods

		public void AddFloorTile (int x, int y, FloorTileView tile)
		{
			node_layers_floorTiles.AddTile(x, y, tile);
		}
		public void AddFurnitureTile (int x, int y, FurnitureTileView tile)
		{
			node_layers_furnitureTiles.AddTile(x, y, tile);
		}
		public void AddItemTile (int x, int y, ItemTileView tile)
		{
			node_layers_itemTiles.AddTile(x, y, tile);
		}
		public void AddDoorTile (int x, int y, DoorTileView tile)
		{
			node_layers_doorTiles.AddTile(x, y, tile);
		}
		public void AddWallTile (int x, int y, WallTileView tile)
		{
			node_layers_wallTiles.AddTile(x, y, tile);
		}
		public void AddActorTile (int x, int y, ActorTileView tile)
		{
			node_layers_actorTiles.AddTile(x, y, tile);
		}

		public void RemoveFloorTile (int x, int y)
		{
			node_layers_floorTiles.RemoveTile(x, y);
		}
		public void RemoveFurnitureTile (int x, int y)
		{
			node_layers_furnitureTiles.RemoveTile(x, y);
		}
		public void RemoveItemTile (int x, int y)
		{
			node_layers_itemTiles.RemoveTile(x, y);
		}
		public void RemoveDoorTile (int x, int y)
		{
			node_layers_doorTiles.RemoveTile(x, y);
		}
		public void RemoveWallTile (int x, int y)
		{
			node_layers_wallTiles.RemoveTile(x, y);
		}
		public void RemoveActorTile (int x, int y)
		{
			node_layers_actorTiles.RemoveTile(x, y);
		}

		public void MoveFloorTile (int fromX, int fromY, int toX, int toY)
		{
			node_layers_floorTiles.MoveTile(fromX, fromY, toX, toY);
		}
		public void MoveFurnitureTile (int fromX, int fromY, int toX, int toY)
		{
			node_layers_furnitureTiles.MoveTile(fromX, fromY, toX, toY);
		}
		public void MoveItemTile (int fromX, int fromY, int toX, int toY)
		{
			node_layers_itemTiles.MoveTile(fromX, fromY, toX, toY);
		}
		public void MoveDoorTile (int fromX, int fromY, int toX, int toY)
		{
			node_layers_doorTiles.MoveTile(fromX, fromY, toX, toY);
		}
		public void MoveWallTile (int fromX, int fromY, int toX, int toY)
		{
			node_layers_wallTiles.MoveTile(fromX, fromY, toX, toY);
		}
		public void MoveActorTile (int fromX, int fromY, int toX, int toY)
		{
			node_layers_actorTiles.MoveTile(fromX, fromY, toX, toY);
		}

		public FloorTileView GetFloorTile (int x, int y)
		{
			return node_layers_floorTiles.GetTile(x, y);
		}
		public FurnitureTileView GetFurnitureTile (int x, int y)
		{
			return node_layers_furnitureTiles.GetTile(x, y);
		}
		public ItemTileView GetItemTile (int x, int y)
		{
			return node_layers_itemTiles.GetTile(x, y);
		}
		public DoorTileView GetDoorTile (int x, int y)
		{
			return node_layers_doorTiles.GetTile(x, y);
		}
		public WallTileView GetWallTile (int x, int y)
		{
			return node_layers_wallTiles.GetTile(x, y);
		}
		public ActorTileView GetActorTile (int x, int y)
		{
			return node_layers_actorTiles.GetTile(x, y);
		}

		public bool IsFloorTile (int x, int y)
		{
			return node_layers_floorTiles.IsTile(x, y);
		}
		public bool IsFurnitureTile (int x, int y)
		{
			return node_layers_furnitureTiles.IsTile(x, y);
		}
		public bool IsItemTile (int x, int y)
		{
			return node_layers_itemTiles.IsTile(x, y);
		}
		public bool IsDoorTile (int x, int y)
		{
			return node_layers_doorTiles.IsTile(x, y);
		}
		public bool IsWallTile (int x, int y)
		{
			return node_layers_wallTiles.IsTile(x, y);
		}
		public bool IsActorTile (int x, int y)
		{
			return node_layers_actorTiles.IsTile(x, y);
		}

		#endregion // Public methods

	}

}
