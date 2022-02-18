using System.Collections.Generic;
using Godot;

public class WallTile : Tile
{

    #region Nodes

    private Node2D node_autoTilePieces;

    #endregion // Nodes



    #region Fields

    private readonly Dictionary<string, Texture> m_textureDictionary;
    private readonly Dictionary<string, bool> m_collisionDictionary;

    private readonly List<WallTile> m_surroundingWallTiles;

    #endregion // Fields



    #region Constructors

    public WallTile () : base()
    {
        m_textureDictionary = new Dictionary<string, Texture>();
        m_collisionDictionary = new Dictionary<string, bool>();

        m_surroundingWallTiles = new List<WallTile>();
    }

    #endregion // Constructors



    #region Node2D methods

    public override void _EnterTree ()
    {
        AddToGroup("WallTile");

        node_autoTilePieces = GetNode<Node2D>("AutoTilePieces");
    }

    public override void _Ready ()
    {
        foreach (Sprite sprite in node_autoTilePieces.GetChildren())
        {
            m_textureDictionary.Add(sprite.Name, sprite.Texture);
            m_collisionDictionary.Add(sprite.Name, false);
        }
    }

    #endregion // Node2D methods



    #region Public methods

    public void UpdateSurroundingWallTiles ()
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

    public void UpdateCollisionDictionary ()
    {
        foreach (KeyValuePair<string, RayCast2D> kvp in m_rayCastDictionary)
        {
            Vector2 point = Vector2.Zero;


            m_collisionDictionary[kvp.Key] = kvp.Value.IsColliding();
        }
    }

    public void Update ()
    {
        UpdateSurroundingWallTiles();
        UpdateCollisionDictionary();
    }

    #endregion // Public methods

}
