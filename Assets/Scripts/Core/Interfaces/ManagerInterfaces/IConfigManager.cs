using Cysharp.Threading.Tasks;
using Features.Configs;

namespace Core.Interfaces.ManagerInterfaces
{
    public interface IConfigManager
    {
        TV GetConfig<T, TV>(int id = 0) where T : IConfigGetter where TV : BaseConfig;

        UniTask LoadConfigs();
    }
}