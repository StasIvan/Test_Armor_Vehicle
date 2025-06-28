using Interfaces;
using UnityEngine;

namespace Configs.BulletConfig
{
    [CreateAssetMenu(fileName = "Bullet", menuName = "Configs/BulletConfig", order = 2)]
    public class BulletConfigSO : ScriptableObject, IConfigContainer<BulletConfig>
    {
        [SerializeField] private BulletConfig _config;

        public BulletConfig GetConfig()
        {
            return _config;
        }
    }
}