using System;

namespace Configs.PlayerConfig
{
    [Serializable]
    public class PlayerConfig : BaseObjectConfig
    {
        public float health;
        public float bulletPerMinute;
    }
}