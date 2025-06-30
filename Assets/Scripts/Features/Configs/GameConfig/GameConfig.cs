using System;
using UnityEngine;

namespace Features.Configs.GameConfig
{
    [Serializable]
    public class GameConfig : BaseConfig
    {
        public Vector2 levelSize;
        public int enemyCount;
    }
}