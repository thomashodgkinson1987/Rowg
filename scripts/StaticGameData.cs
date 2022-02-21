public static class StaticGameData
{

    public const int TileWidthInPixels = 20;
    public const int TileHeightInPixels = 20;

    public const int ChunkWidthInTiles = 16;
    public const int ChunkHeightInTiles = 16;

    public const int ChunkWidthInPixels = ChunkWidthInTiles * TileWidthInPixels;
    public const int MapChunkHeightInPixels = ChunkHeightInTiles * TileHeightInPixels;

}
