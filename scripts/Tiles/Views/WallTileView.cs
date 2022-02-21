using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Rowg.Tiles.Views
{

	public class WallTileView : TileView
	{

		#region Nodes

		private Node2D node_autoTilePieces;

		#endregion // Nodes



		#region Fields

		private readonly Dictionary<string, Texture> m_textureDictionary;
		private readonly Dictionary<Vector2, WallTileView> m_collisionDictionary;
		private readonly List<WallTileView> m_surroundingWallTiles;

		#endregion // Fields



		#region Constructors

		public WallTileView () : base()
		{
			m_textureDictionary = new Dictionary<string, Texture>();
			m_collisionDictionary = new Dictionary<Vector2, WallTileView>();
			m_surroundingWallTiles = new List<WallTileView>();
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
			List<WallTileView> wallTiles = new List<WallTileView>();

			foreach (Node node in nodesInWallTileGroup)
			{
				if (node is WallTileView wallTile)
				{
					wallTiles.Add(wallTile);
				}
			}

			wallTiles.Remove(this);

			for (int i = 0; i < wallTiles.Count; i++)
			{
				WallTileView wallTile = wallTiles[i];
				if (wallTile.GlobalPosition.x >= GlobalPosition.x - StaticGameData.TileWidthInPixels &&
					wallTile.GlobalPosition.x <= GlobalPosition.x + StaticGameData.TileWidthInPixels &&
					wallTile.GlobalPosition.y >= GlobalPosition.y - StaticGameData.TileHeightInPixels &&
					wallTile.GlobalPosition.y <= GlobalPosition.y + StaticGameData.TileHeightInPixels)
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
					WallTileView wallTile = m_surroundingWallTiles[j];

					if (wallTile.GlobalPosition.x >= GlobalPosition.x + (vector.x * StaticGameData.TileWidthInPixels) &&
						wallTile.GlobalPosition.x <= GlobalPosition.x + (vector.x * StaticGameData.TileWidthInPixels) &&
						wallTile.GlobalPosition.y >= GlobalPosition.y + (vector.y * StaticGameData.TileHeightInPixels) &&
						wallTile.GlobalPosition.y <= GlobalPosition.y + (vector.y * StaticGameData.TileHeightInPixels))
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

			if (!isLeft && !isRight && !isUp && !isDown)
			{
				node_sprite.Texture = m_textureDictionary["Single"];
			}
			else
			{
				if (!isLeft && isRight && !isUp && !isDown)
				{
					node_sprite.Texture = m_textureDictionary["Horizontal_Left"];
				}
				else if (isLeft && isRight && !isUp && !isDown)
				{
					node_sprite.Texture = m_textureDictionary["Horizontal_Center"];
				}
				else if (isLeft && !isRight && !isUp && !isDown)
				{
					node_sprite.Texture = m_textureDictionary["Horizontal_Right"];
				}

				else if (!isLeft && !isRight && !isUp && isDown)
				{
					node_sprite.Texture = m_textureDictionary["Vertical_Top"];
				}
				else if (!isLeft && !isRight && isUp && isDown)
				{
					node_sprite.Texture = m_textureDictionary["Vertical_Center"];
				}
				else if (!isLeft && !isRight && isUp && !isDown)
				{
					node_sprite.Texture = m_textureDictionary["Vertical_Bottom"];
				}

				else if (!isLeft && isRight && !isUp && isDown)
				{
					node_sprite.Texture = m_textureDictionary["Top_Left"];
				}
				else if (isLeft && isRight && !isUp && isDown)
				{
					node_sprite.Texture = m_textureDictionary["Top_Center"];
				}
				else if (isLeft && !isRight && !isUp && isDown)
				{
					node_sprite.Texture = m_textureDictionary["Top_Right"];
				}

				else if (!isLeft && isRight && isUp && isDown)
				{
					node_sprite.Texture = m_textureDictionary["Middle_Left"];
				}
				else if (isLeft && isRight && isUp && isDown)
				{
					node_sprite.Texture = m_textureDictionary["Middle_Center"];
				}
				else if (isLeft && !isRight && isUp && isDown)
				{
					node_sprite.Texture = m_textureDictionary["Middle_Right"];
				}

				else if (!isLeft && isRight && isUp && !isDown)
				{
					node_sprite.Texture = m_textureDictionary["Bottom_Left"];
				}
				else if (isLeft && isRight && isUp && !isDown)
				{
					node_sprite.Texture = m_textureDictionary["Bottom_Center"];
				}
				else if (isLeft && !isRight && isUp && !isDown)
				{
					node_sprite.Texture = m_textureDictionary["Bottom_Right"];
				}
			}
		}

		#endregion // Private methods

	}

}
