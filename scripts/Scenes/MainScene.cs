using Godot;

public class MainScene : Node2D
{

	#region Nodes

	private Camera2D node_camera;
	private PlayerTile node_playerTile;
	private Map node_map;

	#endregion // Nodes



	#region Fields

	private readonly System.Random m_rng;

	#endregion // Fields



	#region Constructors

	public MainScene () : base()
	{
		m_rng = new System.Random();
	}

	#endregion // Constructors



	#region Node2D methods

	public override void _EnterTree ()
	{
		node_camera = GetNode<Camera2D>("Camera");
		node_playerTile = GetNode<PlayerTile>("PlayerTile");
		node_map = GetNode<Map>("Map");
	}

	public override void _Ready ()
	{
		node_camera.Position = node_playerTile.Position - new Vector2(160, 160);
		node_map.AllMapChunks.ForEach(_ => node_map.LoadMapChunk(_.X, _.Y));

		OnPlayerTilePositionChanged();
		OnMapBoundsChanged();
	}

	public override void _Input (InputEvent @event)
	{
		if (@event is InputEventKey key)
		{
			if (key.Pressed && !key.Echo && !key.Shift && !key.Control)
			{
				if (key.Scancode == (int)KeyList.Escape)
				{
					GetTree().Quit();
				}
				else if (key.Scancode == (int)KeyList.Key1)
				{
					if (!node_map.IsMapChunkLoaded(0, 0))
						node_map.LoadMapChunk(0, 0);
					else
						node_map.UnloadMapChunk(0, 0);
				}
				else if (key.Scancode == (int)KeyList.Key2)
				{
					if (!node_map.IsMapChunkLoaded(1, 0))
						node_map.LoadMapChunk(1, 0);
					else
						node_map.UnloadMapChunk(1, 0);
				}
				else if (key.Scancode == (int)KeyList.Key3)
				{
					if (!node_map.IsMapChunkLoaded(0, 1))
						node_map.LoadMapChunk(0, 1);
					else
						node_map.UnloadMapChunk(0, 1);
				}
				else if (key.Scancode == (int)KeyList.Key4)
				{
					if (!node_map.IsMapChunkLoaded(1, 1))
						node_map.LoadMapChunk(1, 1);
					else
						node_map.UnloadMapChunk(1, 1);
				}
				else if (key.Scancode == (int)KeyList.O)
				{
					node_map.AllDoorTiles.ForEach(_ => _.Open());
				}
				else if (key.Scancode == (int)KeyList.C)
				{
					node_map.AllDoorTiles.ForEach(_ => _.Close());
				}
				else
				{
					node_playerTile.InputTick(@event, node_map);
					node_map.InputTick(@event);
				}
			}
		}
	}

	public override void _PhysicsProcess (float delta)
	{
		node_playerTile.FixedTick(delta, node_map);
		node_map.FixedTick(delta);
	}

	public override void _Process (float delta)
	{
		node_playerTile.Tick(delta, node_map);
		node_map.Tick(delta);
	}

	#endregion // Node2D methods



	#region Private methods

	private void OnPlayerTilePositionChanged ()
	{
		node_camera.Position = node_playerTile.Position - new Vector2(160, 160);
	}

	private void OnMapBoundsChanged ()
	{
		node_camera.LimitLeft = (int)node_map.Bounds.Position.x;
		node_camera.LimitRight = (int)node_map.Bounds.End.x;
		node_camera.LimitTop = (int)node_map.Bounds.Position.y;
		node_camera.LimitBottom = (int)node_map.Bounds.End.y;
	}

	#endregion // Private methods

}
