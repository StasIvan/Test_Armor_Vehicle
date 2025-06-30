using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Features.Configs.Base
{
    public class ConfigsLoader
    {
        private LoadConfig _load;

        public async UniTask<T[]> LoadConfigs<T>(string searchingWord) where T : Object
        {
            _load = new LoadConfig();

            return await _load.LoadConfigs<T>(searchingWord);
        }

    }


    public class LoadConfig
    {
        public async UniTask<T[]> LoadConfigs<T>(string searchingWord) where T : Object
        {
            var configs = new List<T>();

            var handle = Addressables.LoadAssetsAsync<T>(
                new AssetLabelReference { labelString = searchingWord },
                c => configs.Add(c)
            );

            try
            {
                await handle.ToUniTask();

                if (handle is { Status: AsyncOperationStatus.Succeeded, Result: not null })
                {
                    return configs.ToArray();
                }
                else
                {
                    Debug.LogError($"Config {searchingWord} was not loaded!");
                    return Array.Empty<T>();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception while loading configs: {ex.Message}");
                return Array.Empty<T>();
            }
        }

    }
}