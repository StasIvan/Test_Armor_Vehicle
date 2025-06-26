using System;
using Managers;

namespace Configs
{
    [Serializable]
    public abstract class BaseObjectConfig : BaseConfig
    {
        public float speed;
        public float rotationSpeed;
    }
}