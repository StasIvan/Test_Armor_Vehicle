using Features.Configs;

namespace Core.Interfaces
{
    public interface IConfigGetter
    {
        public BaseConfig GetConfig(int id);
    }
}