using Configs;
using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IConfigManager
    {
        TV GetConfig<T, TV>(int id = 0) where T : IConfigGetter where TV : BaseConfig;

        UniTask LoadConfigs();
    }
}