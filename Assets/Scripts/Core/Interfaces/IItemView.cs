using Features.GameItems.Base;

namespace Core.Interfaces
{
    public interface IItemView
    {
        void OnModelChanged(ChangedFields field);
    }
}