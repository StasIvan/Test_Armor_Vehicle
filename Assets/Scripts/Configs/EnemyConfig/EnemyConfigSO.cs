using Interfaces;
using UnityEngine;

namespace Configs.EnemyConfig
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Configs/EnemyConfig", order = 1)]
    public class EnemyConfigSO : ScriptableObject, IConfigContainer<EnemyConfig>
    {
        [SerializeField] private EnemyConfig _config;

        public EnemyConfig GetConfig()
        {
            return _config;
        }
    }
}