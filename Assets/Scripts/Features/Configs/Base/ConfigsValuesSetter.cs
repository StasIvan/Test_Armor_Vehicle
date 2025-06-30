using Core.Interfaces;

namespace Features.Configs.Base
{
    public class ConfigsValuesSetter<TSource, TType>
        where TType : class
        where TSource : IConfigContainer<TType>
    {
        public void SetValues(TSource[] copy, out TType[] paste)
        {
            paste = new TType[copy.Length];
            for (int i = 0; i < copy.Length; i++)
            {
                TType targetInstance = copy[i].GetConfig();
                paste[i] = targetInstance;
            }
        }

        public void SetValues(TSource copy, out TType paste)
        {
            paste = copy.GetConfig();
        }

    }

}