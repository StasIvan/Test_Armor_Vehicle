using Core.Interfaces;

namespace Features.Configs.Base
{
    public abstract class BaseConfigs<TSource, TType> where TSource : IConfigContainer<TType> where TType : class
    {
        protected ConfigsValuesSetter<TSource, TType> valuesSetter = new();

        public abstract void SetValues(TSource[] items);
    }
}