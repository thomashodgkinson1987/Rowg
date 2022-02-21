namespace Rowg.GameActions
{

    public class GameAction
    {

        public readonly string Name;
        public readonly string Description;

        public GameAction (string name, string description)
        {
            Name = name;
            Description = description;
        }

        public virtual void Execute () { }

    }

}