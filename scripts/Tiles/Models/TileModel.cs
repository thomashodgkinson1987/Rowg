using Rowg.Tiles.Views;

namespace Rowg.Tiles.Models
{

    public abstract class TileModel<T> where T : TileView
    {

        public string Name;
        public string Description;

        public int X;
        public int Y;

    }

}
