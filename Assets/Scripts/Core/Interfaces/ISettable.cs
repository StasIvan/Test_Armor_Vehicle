﻿namespace Core.Interfaces
{
    public interface ISettable<in T>
    {
        void Set(T value);
    }
}