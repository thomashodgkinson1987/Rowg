using Godot;

namespace Rowg.Tiles.Views
{

	public class DoorTileView : TileView
	{

		[Export] private Texture m_openTexture;
		[Export] private Texture m_closedTexture;

		[Export] public bool IsOpen;

		public DoorTileView () : base()
		{
			IsOpen = true;
		}

		public override void _Ready ()
		{
			if (IsOpen)
			{
				node_sprite.Texture = m_openTexture;
			}
			else
			{
				node_sprite.Texture = m_closedTexture;
			}
		}

		public void Open ()
		{
			IsOpen = true;
			node_sprite.Texture = m_openTexture;
		}

		public void Close ()
		{
			IsOpen = false;
			node_sprite.Texture = m_closedTexture;
		}

		public void Toggle ()
		{
			if (IsOpen)
			{
				Close();
			}
			else
			{
				Open();
			}
		}

	}

}
