using Godot;

public class PlayerTile : ActorTile
{

	[Signal] public delegate void PositionChanged ();

	public override void InputTick (InputEvent @event, Map map)
	{
		if (@event is InputEventKey key)
		{
			int dx = 0;
			int dy = 0;

			if (key.Scancode == (int)KeyList.Kp1)
			{
				dx = -1;
				dy = 1;
			}
			else if (key.Scancode == (int)KeyList.Kp2)
			{
				dy = 1;
			}
			else if (key.Scancode == (int)KeyList.Kp3)
			{
				dx = 1;
				dy = 1;
			}
			else if (key.Scancode == (int)KeyList.Kp4)
			{
				dx = -1;
			}
			else if (key.Scancode == (int)KeyList.Kp5)
			{
			}
			else if (key.Scancode == (int)KeyList.Kp6)
			{
				dx = 1;
			}
			else if (key.Scancode == (int)KeyList.Kp7)
			{
				dx = -1;
				dy = -1;
			}
			else if (key.Scancode == (int)KeyList.Kp8)
			{
				dy = -1;
			}
			else if (key.Scancode == (int)KeyList.Kp9)
			{
				dx = 1;
				dy = -1;
			}

			dx *= StaticGameData.TileWidth;
			dy *= StaticGameData.TileWidth;
			Translate(new Vector2(dx, dy));

			if (dx != 0 || dy != 0)
			{
				EmitSignal(nameof(PositionChanged));
			}
		}
	}

}
