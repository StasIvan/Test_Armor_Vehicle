using System;

namespace GameItems.Base
{
    public abstract class BaseItemModel
    {
        public abstract event Action<ChangedFields> OnChanged;
    }
    
    public enum ChangedFields
    {
        None,
        Position,
        Rotation,
        Animate,
        Health,
        MaxHealth,
        Speed,
        ResetSpeed,
        Damage,
    }
}