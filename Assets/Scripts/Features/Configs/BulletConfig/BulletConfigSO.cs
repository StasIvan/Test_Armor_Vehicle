using Core.Interfaces;
using UnityEngine;

namespace Features.Configs.BulletConfig
{
    [CreateAssetMenu(fileName = "Bullet", menuName = "Configs/BulletConfig", order = 2)]
    public class BulletConfigSO : ScriptableObject, IConfigContainer<Features.Configs.BulletConfig.BulletConfig>
    {
        [SerializeField] private Features.Configs.BulletConfig.BulletConfig _config;

        public Features.Configs.BulletConfig.BulletConfig GetConfig()
        {
            return _config;
        }
    }
}