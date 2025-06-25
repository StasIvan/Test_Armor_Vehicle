﻿namespace Interfaces
{
    public interface IConfigContainer<T> where T : class
    {
        T GetConfig();
    }
}