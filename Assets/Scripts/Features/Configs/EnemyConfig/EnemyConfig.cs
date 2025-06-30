﻿using System;

namespace Features.Configs.EnemyConfig
{
    [Serializable]
    public class EnemyConfig : BaseObjectConfig
    {
        public float rotationSpeed;
        public float health;
        public float damage;
    }
}