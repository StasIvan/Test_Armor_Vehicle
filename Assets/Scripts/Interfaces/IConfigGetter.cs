using Configs;

namespace Interfaces
{
    public interface IConfigGetter
    {
        public BaseConfig GetConfig(int id);
    }
}