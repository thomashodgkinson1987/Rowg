using System.Collections.Generic;
using Godot;

public class MapChunk : Node2D
{

	#region Nodes

	private FloorTileMapChunkLayer node_layers_floorTiles;
	private ItemTileMapChunkLayer node_layers_itemTiles;
	private DoorTileMapChunkLayer node_layers_doorTiles;
	private WallTileMapChunkLayer node_layers_wallTiles;
	private ActorTileMapChunkLayer node_layers_actorTiles;

	#endregion // Nodes



	#region Properties

	public List<FloorTile> AllFloorTiles => node_layers_floorTiles.AllTiles;
	public List<ItemTile> AllItemTiles => node_layers_itemTiles.AllTiles;
	public List<DoorTile> AllDoorTiles => node_layers_doorTiles.AllTiles;
	public List<WallTile> AllWallTiles => node_layers_wallTiles.AllTiles;
	public List<ActorTile> AllActorTiles => node_layers_actorTiles.AllTiles;

	#endregion // Properties



	#region Fields

	public int X;
	public int Y;

	#endregion // Fields



	#region Constructors

	public MapChunk () : base()
	{
		X = 0;
		Y = 0;
	}

	#endregion // Constructors



	#region Node2D methods

	public override void _EnterTree ()
	{
		node_layers_floorTiles = GetNode<FloorTileMapChunkLayer>("FloorTileMapChunkLayer");
		node_layers_itemTiles = GetNode<ItemTileMapChunkLayer>("ItemTileMapChunkLayer");
		node_layers_doorTiles = GetNode<DoorTileMapChunkLayer>("DoorTileMapChunkLayer");
		node_layers_wallTiles = GetNode<WallTileMapChunkLayer>("WallTileMapChunkLayer");
		node_layers_actorTiles = GetNode<ActorTileMapChunkLayer>("ActorTileMapChunkLayer");
	}

	#endregion // Node2D methods



	#region Public methods

	public void AddFloorTile (int x, int y, FloorTile tile)
	{
		node_layers_floorTiles.AddTile(x, y, tile);
	}
	public void AddItemTile (int x, int y, ItemTile tile)
	{
		node_layers_itemTiles.AddTile(x, y, tile);
	}
	public void AddDoorTile (int x, int y, DoorTile tile)
	{
		node_layers_doorTiles.AddTile(x, y, tile);
	}
	public void AddWallTile (int x, int y, WallTile tile)
	{
		node_layers_wallTiles.AddTile(x, y, tile);
	}
	public void AddActorTile (int x, int y, ActorTile tile)
	{
		node_layers_actorTiles.AddTile(x, y, tile);
	}

	public void RemoveFloorTile (int x, int y)
	{
		node_layers_floorTiles.RemoveTile(x, y);
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

	public FloorTile GetFloorTile (int x, int y)
	{
		return node_layers_floorTiles.GetTile(x, y);
	}
	public ItemTile GetItemTile (int x, int y)
	{
		return node_layers_itemTiles.GetTile(x, y);
	}
	public DoorTile GetDoorTile (int x, int y)
	{
		return node_layers_doorTiles.GetTile(x, y);
	}
	public WallTile GetWallTile (int x, int y)
	{
		return node_layers_wallTiles.GetTile(x, y);
	}
	public ActorTile GetActorTile (int x, int y)
	{
		return node_layers_actorTiles.GetTile(x, y);
	}

	public bool IsFloorTile (int x, int y)
	{
		return node_layers_floorTiles.IsTile(x, y);
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
