public static class StaticGameData
{

    public const int TileWidth = 20;
    public const int TileHeight = 20;

    public const int MapChunkWidthInTiles = 16;
    public const int MapChunkHeightInTiles = 16;

    public const int MapChunkWidthInPixels = MapChunkWidthInTiles * TileWidth;
    public const int MapChunkHeightInPixels = MapChunkHeightInTiles * TileHeight;

}
