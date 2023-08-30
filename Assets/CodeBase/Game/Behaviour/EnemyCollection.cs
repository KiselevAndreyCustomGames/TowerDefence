using CodeBase.Game.Character.Enemy;
using System.Collections.Generic;

namespace CodeBase.Game
{
    [System.Serializable]
    public class EnemyCollection
    {
        private List<IEnemy> _enemies = new();

        public int Count => _enemies.Count;

        public void Add(IEnemy enemy) =>
            _enemies.Add(enemy);

        public void GameUpdate()
        {
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                if (_enemies[i].GameUpdate() == false)
                    _enemies.RemoveAt(i);
            }
        }
    }
}