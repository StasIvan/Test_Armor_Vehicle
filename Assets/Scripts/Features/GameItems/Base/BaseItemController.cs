using Core.Interfaces;
using Core.Interfaces.ManagerInterfaces;
using Zenject;

namespace Features.GameItems.Base
{
    public abstract class BaseItemController<TView, TModel> : IItemController where TView : IItemView where TModel : BaseItemModel
    {
        protected TView View;
        protected TModel Model;
        
        protected ISpawner Spawner;
        protected IConfigManager ConfigManager;
        protected SignalBus SignalBus;
        
        [Inject]
        public void Construct(ISpawner spawner, IConfigManager configManager,  SignalBus signalBus)
        {
            Spawner = spawner;
            ConfigManager = configManager;
            SignalBus = signalBus;
        }

        public abstract void Initialize();
        public abstract void Dispose();
        public abstract void Bind(TView view, TModel model);
        public abstract GameItemType Type { get; }

        public  T GetView<T>() where T : class
        {
            return View as T;
        }
    }

    public enum GameItemType
    {
        None,
        Player,
        Bullet,
        Enemy
    }
}