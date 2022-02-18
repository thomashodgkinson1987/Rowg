using System.Collections.Generic;
using System.Linq;
using Godot;

public class WallTile : Tile
{

	#region Nodes

	private Node2D node_autoTilePieces;

	#endregion // Nodes



	#region Fields

	private readonly Dictionary<string, Texture> m_textureDictionary;
	private readonly Dictionary<Vector2, WallTile> m_collisionDictionary;
	private readonly List<WallTile> m_surroundingWallTiles;

	#endregion // Fields



	#region Constructors

	public WallTile () : base()
	{
		m_textureDictionary = new Dictionary<string, Texture>();
		m_collisionDictionary = new Dictionary<Vector2, WallTile>();
		m_surroundingWallTiles = new List<WallTile>();
	}

	#endregion // Constructors



	#region Node2D methods

	public override void _EnterTree ()
	{
		base._EnterTree();

		AddToGroup("WallTile");

		node_autoTilePieces = GetNode<Node2D>("AutoTilePieces");
	}

	public override void _Ready ()
	{
		base._Ready();

		foreach (Node node in node_autoTilePieces.GetChildren())
		{
			if (node is Sprite sprite)
			{
				m_textureDictionary.Add(sprite.Name, sprite.Texture);
			}
		}

		m_collisionDictionary.Add(new Vector2(-1, 0), null);
		m_collisionDictionary.Add(new Vector2(1, 0), null);
		m_collisionDictionary.Add(new Vector2(0, -1), null);
		m_collisionDictionary.Add(new Vector2(0, 1), null);
		m_collisionDictionary.Add(new Vector2(-1, -1), null);
		m_collisionDictionary.Add(new Vector2(1, -1), null);
		m_collisionDictionary.Add(new Vector2(-1, 1), null);
		m_collisionDictionary.Add(new Vector2(1, 1), null);
	}

	#endregion // Node2D methods



	#region Public methods

	public void UpdateAutoTileSprite ()
	{
		UpdateSurroundingWallTiles();
		UpdateCollisionDictionary();
		UpdateCurrentSprite();
	}

	#endregion // Public methods



	#region Private methods

	private void UpdateSurroundingWallTiles ()
	{
		m_surroundingWallTiles.Clear();

		Godot.Collections.Array nodesInWallTileGroup = GetTree().GetNodesInGroup("WallTile");
		List<WallTile> wallTiles = new List<WallTile>();

		foreach (Node node in nodesInWallTileGroup)
		{
			if (node is WallTile wallTile)
			{
				wallTiles.Add(wallTile);
			}
		}

		wallTiles.Remove(this);

		for (int i = 0; i < wallTiles.Count; i++)
		{
			WallTile wallTile = wallTiles[i];
			if (wallTile.GlobalPosition.x >= GlobalPosition.x - StaticGameData.TileWidth &&
				wallTile.GlobalPosition.x <= GlobalPosition.x + StaticGameData.TileWidth &&
				wallTile.GlobalPosition.y >= GlobalPosition.y - StaticGameData.TileHeight &&
				wallTile.GlobalPosition.y <= GlobalPosition.y + StaticGameData.TileHeight)
			{
				m_surroundingWallTiles.Add(wallTile);
			}
		}
	}

	private void UpdateCollisionDictionary ()
	{
		m_collisionDictionary.Clear();

		m_collisionDictionary.Add(new Vector2(-1, 0), null);
		m_collisionDictionary.Add(new Vector2(1, 0), null);
		m_collisionDictionary.Add(new Vector2(0, -1), null);
		m_collisionDictionary.Add(new Vector2(0, 1), null);
		m_collisionDictionary.Add(new Vector2(-1, -1), null);
		m_collisionDictionary.Add(new Vector2(1, -1), null);
		m_collisionDictionary.Add(new Vector2(-1, 1), null);
		m_collisionDictionary.Add(new Vector2(1, 1), null);

		List<Vector2> vectors = m_collisionDictionary.Keys.ToList();

		for (int i = 0; i < vectors.Count; i++)
		{
			Vector2 vector = vectors[i];
			for (int j = 0; j < m_surroundingWallTiles.Count; j++)
			{
				WallTile wallTile = m_surroundingWallTiles[j];

				if (wallTile.GlobalPosition.x >= GlobalPosition.x + (vector.x * StaticGameData.TileWidth) &&
					wallTile.GlobalPosition.x <= GlobalPosition.x + (vector.x * StaticGameData.TileWidth) &&
					wallTile.GlobalPosition.y >= GlobalPosition.y + (vector.y * StaticGameData.TileHeight) &&
					wallTile.GlobalPosition.y <= GlobalPosition.y + (vector.y * StaticGameData.TileHeight))
				{
					m_collisionDictionary[vector] = wallTile;
					break;
				}
			}
		}
	}

	private void UpdateCurrentSprite ()
	{
		bool isLeft = m_collisionDictionary[new Vector2(-1, 0)] != null;
		bool isRight = m_collisionDictionary[new Vector2(1, 0)] != null;
		bool isUp = m_collisionDictionary[new Vector2(0, -1)] != null;
		bool isDown = m_collisionDictionary[new Vector2(0, 1)] != null;
		//bool isUpLeft = m_collisionDictionary[new Vector2(-1, -1)] != null;
		//bool isUpRight = m_collisionDictionary[new Vector2(1, -1)] != null;
		//bool isDownLeft = m_collisionDictionary[new Vector2(-1, 1)] != null;
		//bool isDownRight = m_collisionDictionary[new Vector2(1, 1)] != null;

		if (isLeft && isRight && isUp && isDown)
		{
			node_sprite.Texture = m_textureDictionary["Center"];
		}
		else if (!isLeft && !isRight && !isUp && !isDown)
		{
			node_sprite.Texture = m_textureDictionary["Single"];
		}
		else
		{
			if (!isLeft && isRight && !isUp && !isDown)
			{
				node_sprite.Texture = m_textureDictionary["LeftCap"];
			}
			else if (isLeft && isRight && !isUp && !isDown)
			{
				node_sprite.Texture = m_textureDictionary["HorizontalCenter"];
			}
			else if (isLeft && !isRight && !isUp && !isDown)
			{
				node_sprite.Texture = m_textureDictionary["RightCap"];
			}
			else if (!isLeft && !isRight && !isUp && isDown)
			{
				node_sprite.Texture = m_textureDictionary["TopCap"];
			}
			else if (!isLeft && !isRight && isUp && isDown)
			{
				node_sprite.Texture = m_textureDictionary["VerticalCenter"];
			}
			else if (!isLeft && !isRight && isUp && !isDown)
			{
				node_sprite.Texture = m_textureDictionary["BottomCap"];
			}
			else if (!isLeft && isRight && !isUp && isDown)
			{
				node_sprite.Texture = m_textureDictionary["TopLeftCap"];
			}
			else if (isLeft && isRight && !isUp && isDown)
			{
				node_sprite.Texture = m_textureDictionary["Top"];
			}
			else if (isLeft && !isRight && !isUp && isDown)
			{
				node_sprite.Texture = m_textureDictionary["TopRightCap"];
			}
			else if (!isLeft && isRight && isUp && !isDown)
			{
				node_sprite.Texture = m_textureDictionary["BottomLeftCap"];
			}
			else if (isLeft && isRight && isUp && !isDown)
			{
				node_sprite.Texture = m_textureDictionary["Bottom"];
			}
			else if (isLeft && !isRight && isUp && !isDown)
			{
				node_sprite.Texture = m_textureDictionary["BottomRightCap"];
			}
		}
	}

	#endregion // Private methods

}
