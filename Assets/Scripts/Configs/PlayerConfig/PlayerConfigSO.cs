using Interfaces;
using UnityEngine;

namespace Configs.PlayerConfig
{
    [CreateAssetMenu(fileName = "Player", menuName = "Configs/PlayerConfig", order = 0)]
    public class PlayerConfigSO  : ScriptableObject, IConfigContainer<PlayerConfig>
    {
        [SerializeField] private PlayerConfig _config;
        
        public PlayerConfig GetConfig()
        {
            return _config;
        }
    }
}