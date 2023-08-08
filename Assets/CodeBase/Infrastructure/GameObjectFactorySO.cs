using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public abstract class GameObjectFactorySO : ScriptableObject
    {
        private Scene _scene;

        protected T SpawnGameObject<T>(T prefab) where T : MonoBehaviour 
        {
            if (_scene.isLoaded == false)
            {
#if UNITY_EDITOR
                _scene = SceneManager.GetSceneByName(name);
                if (_scene.isLoaded == false)
                    _scene = SceneManager.CreateScene(name);
#else
                _scene = SceneManager.CreateScene(name);
#endif
            }
            var instance = Lean.Pool.LeanPool.Spawn(prefab);
            SceneManager.MoveGameObjectToScene(instance.gameObject, _scene);
            return instance;
        }
    }
}