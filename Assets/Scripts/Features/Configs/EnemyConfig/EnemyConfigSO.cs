using Core.Interfaces;
using UnityEngine;

namespace Features.Configs.EnemyConfig
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Configs/EnemyConfig", order = 1)]
    public class EnemyConfigSO : ScriptableObject, IConfigContainer<Features.Configs.EnemyConfig.EnemyConfig>
    {
        [SerializeField] private Features.Configs.EnemyConfig.EnemyConfig _config;

        public Features.Configs.EnemyConfig.EnemyConfig GetConfig()
        {
            return _config;
        }
    }
}