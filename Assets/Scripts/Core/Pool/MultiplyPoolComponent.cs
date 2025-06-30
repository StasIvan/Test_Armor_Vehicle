using Core.Interfaces;
using UnityEngine;
using Zenject;

namespace Core.Pool
{
    public class MultiplyPoolComponent : MonoBehaviour, IPool
    {
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private Transform _content;
        
        private MultiplyPool _pool;
        private DiContainer _diContainer;

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        private void Awake()
        {
            _pool = new MultiplyPool(_prefabs, _content, _diContainer);

            ReleaseAll();
        }

        public T GetMonoBehaviour<T>(Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            return _pool.Get<T>(position, rotation);
        }
        
        public void Release<T>(T component) where T : Component
        {
            _pool.Release(component);
        }

        public void ReleaseAllComponents<T>() where T : Component
        {
            _pool.ReleaseAllComponents<T>();
        }

        public void ReleaseAll()
        {
            _pool.ReleaseAll();
        }

        public void Dispose()
        {
            _pool.Dispose();
        }
    }
}