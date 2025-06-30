using Core.Interfaces;
using UnityEngine;

namespace Features.Configs.GameConfig
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig", order = 3)]
    public class GameConfigSO : ScriptableObject, IConfigContainer<Features.Configs.GameConfig.GameConfig>
    {
        [SerializeField] private Features.Configs.GameConfig.GameConfig _config;

        public Features.Configs.GameConfig.GameConfig GetConfig()
        {
            return _config;
        }
    }
}