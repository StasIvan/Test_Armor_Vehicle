using Core.Interfaces;
using Features.GameItems.Base;

namespace Core.Factories.ItemControllerFactories
{
    public class FactoryContainer
    {
        public BaseItemModel Model { get; set; }
        public IItemController Controller { get; set; }
    }
}