using System;
using Managers;

namespace Configs
{
    [Serializable]
    public abstract class BaseObjectConfig : BaseConfig
    {
        public ItemType type;
        public float speed;
    }
}