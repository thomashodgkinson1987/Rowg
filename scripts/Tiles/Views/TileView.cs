using Godot;
using Rowg.Maps;
using Rowg.Tiles.Controllers;

namespace Rowg.Tiles.Views
{

	public class TileView : Node2D
	{

		#region Nodes

		protected Sprite node_backgroundSprite;
		protected Sprite node_sprite;
		protected Sprite node_foregroundSprite;

		#endregion // Nodes



		#region Fields

		protected TileController m_tileController;

		#endregion // Fields



		#region Node2D methods

		public override void _EnterTree ()
		{
			node_backgroundSprite = GetNode<Sprite>("BackgroundSprite");
			node_sprite = GetNode<Sprite>("Sprite");
			node_foregroundSprite = GetNode<Sprite>("ForegroundSprite");
		}

		#endregion // Node2D methods



		#region Public methods

		public virtual void InputTick (InputEvent @event, Map map) { }
		public virtual void FixedTick (float delta, Map map) { }
		public virtual void Tick (float delta, Map map) { }

		#endregion // Public methods

	}

}
