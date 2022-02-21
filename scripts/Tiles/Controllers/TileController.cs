using Godot;
using Rowg.GameActions;
using Rowg.Tiles.Views;
using System.Collections.Generic;

namespace Rowg.Tiles.Controllers
{

    public abstract class TileController
    {

        #region Fields

        protected readonly TileView m_tile;
        protected readonly Dictionary<string, GameAction> m_gameActionsDictionary;

        #endregion // Fields



        #region Constructors

        public TileController (TileView tile)
        {
            m_tile = tile;
            m_gameActionsDictionary = new Dictionary<string, GameAction>();
        }

        #endregion // Constructors



        #region Public methods

        public void AddGameAction (GameAction gameAction)
        {
            m_gameActionsDictionary.Add(gameAction.Name, gameAction);
        }

        public void ExecuteGameAction (string name)
        {
            m_gameActionsDictionary[name].Execute();
        }

        public virtual void InputTick (InputEvent @event) { }
        public virtual void Tick (float delta) { }
        public virtual void FixedTick (float delta) { }

        #endregion // Public methods

    }

}
