using System.Collections.Generic;

namespace CodeBase.Game
{
    [System.Serializable]
    public class GameCollection
    {
        private readonly List<IPlayable> _behaviors = new();

        public int Count => _behaviors.Count;

        public void Add(IPlayable behaviour) =>
            _behaviors.Add(behaviour);

        public void GameUpdate()
        {
            for (int i = _behaviors.Count - 1; i >= 0; i--)
            {
                if (_behaviors[i].GameUpdate() == false)
                    _behaviors.RemoveAt(i);
            }
        }
    }
}