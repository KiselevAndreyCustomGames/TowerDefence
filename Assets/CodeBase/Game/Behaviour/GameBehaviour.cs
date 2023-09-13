using UnityEngine;

namespace CodeBase.Game
{
    public class GameBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemySpawnBehaviour _enemySpawner;
        [SerializeField] private MapBehaviour _map;
        [SerializeField] private ProjectileGameBehaviour _projectiles;
        [SerializeField] private GameWinFallBehaviour _winFall;

        private bool _isPlaing = true;
        private bool _isWave = false;

        private void Awake()
        {
            _map.Awake();
            _winFall.Restart();
        }

        private void OnEnable()
        {
            _winFall.EndGame += EndGame;
            _winFall.OnEnable(_enemySpawner);
        }

        private void OnDisable()
        {
            _winFall.EndGame -= EndGame;
            _winFall.OnDisable(_enemySpawner);
        }

        private void Start()
        {
            _map.Start(_projectiles);
            _enemySpawner.Start(_map.EnemySpawnTiles, _winFall.TakeDamage);
            _projectiles.Start();
        }

        private void Update()
        {
            CheckKeys();
            GameUpdate();
        }

        private void CheckKeys()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
                _isPlaing = !_isPlaing;
            else if (Input.GetKeyUp(KeyCode.Space))
                _isWave = !_isWave;
            else if (Input.GetKeyUp(KeyCode.R))
                Restart();
        }

        private void GameUpdate()
        {
            if (_isPlaing)
            {
                if (_isWave)
                {
                    _enemySpawner.GameUpdate();
                    Physics.SyncTransforms();
                    _projectiles.GameUpdate();
                    _map.GameUpdate();
                }
                else
                {
                    _map.PlacementUpdate();
                }
            }
        }

        private void Restart()
        {
            _enemySpawner.Restart();
            _map.Restart();
            _winFall.Restart();

            _isPlaing = true;
            _isWave = false;
        }

        private void EndGame()
        {
            _isPlaing = false;
            
        }
    }
}