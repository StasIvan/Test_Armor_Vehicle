using Interfaces;
using UnityEngine;

namespace Configs.GameConfig
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig", order = 3)]
    public class GameConfigSO : ScriptableObject, IConfigContainer<GameConfig>
    {
        [SerializeField] private GameConfig _config;

        public GameConfig GetConfig()
        {
            return _config;
        }
    }
}