using Godot;

public class EnemyTile : ActorTile
{

	private readonly System.Random m_rng;

	public EnemyTile () : base()
	{
		m_rng = new System.Random();
	}

	public override void InputTick (InputEvent @event, Map map)
	{
		int dx = m_rng.Next(-1, 2);
		int dy = m_rng.Next(-1, 2);
		map.MoveActorTile(dx, dy, this);
	}

}
