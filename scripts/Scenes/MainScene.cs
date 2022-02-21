using Godot;
using Rowg.Maps;
using Rowg.Tiles.Views;
using Rowg.UI;

namespace Rowg.Scenes
{

	public class MainScene : Node2D
	{

		#region Nodes

		private Map node_map;
		private PlayerTileView node_playerTile;
		private Camera2D node_camera;

		private MessageLogUI node_bottomUI;

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
			node_camera = GetNode<Camera2D>("ViewportContainer/Viewport/Camera");
			node_playerTile = GetNode<PlayerTileView>("ViewportContainer/Viewport/PlayerTile");
			node_map = GetNode<Map>("ViewportContainer/Viewport/Map");

			node_bottomUI = GetNode<MessageLogUI>("BottomUI");
		}

		public override void _Ready ()
		{
			node_map.AllChunks.ForEach(_ => node_map.LoadChunk(_.X, _.Y));
			node_camera.Position = node_playerTile.Position - new Vector2(160, 160);
			OnPlayerTilePositionChanged();
			OnMapBoundsChanged();

			foreach (WallTileView wallTile in node_map.AllWallTiles)
			{
				wallTile.UpdateAutoTileSprite();
			}
		}

		public override void _Input (InputEvent @event)
		{
			if (@event is InputEventKey key)
			{
				if (key.Pressed && !key.Shift && !key.Control)
				{
					if (key.Scancode == (int)KeyList.Escape)
					{
						GetTree().Quit();
					}
					else if (key.Scancode == (int)KeyList.Space)
					{
						node_bottomUI.AddEntry("Hello, World!");
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

}
