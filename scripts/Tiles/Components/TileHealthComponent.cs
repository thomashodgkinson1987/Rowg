namespace Rowg.Tiles.Components
{

    public struct TileHealthComponent
    {

        public int Health;
        public int MaxHealth;

        public TileHealthComponent (int health, int maxHealth)
        {
            Health = health;
            MaxHealth = maxHealth;
        }

    }

}
