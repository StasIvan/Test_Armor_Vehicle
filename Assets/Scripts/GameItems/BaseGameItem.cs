using System;
using Configs;
using Configs.PlayerConfig;
using Interfaces;
using Pool;
using UnityEngine;
using Zenject;

namespace GameItems
{
    public abstract class BaseGameItem : MonoBehaviour, IDisposable
    {
        [SerializeField] protected Rigidbody _rigidbody;
        protected IConfigManager Configs;
        protected SignalBus SignalBus;
        protected ISpawner _spawner;

        [Inject]
        public void Construct(IConfigManager configs, ISpawner spawner, SignalBus signalBus)
        {
            Configs = configs;
            SignalBus = signalBus;
            _spawner = spawner;
        }

        public abstract void Initialize();
        public abstract void Dispose();

        protected abstract void Release();
    }
}