using UnityEngine;

namespace Interfaces
{
    public interface IPool
    {
        T GetMonoBehaviour<T>() where T : MonoBehaviour;
        void ReleaseAll();
        void Release<T>(T component) where T : Component;
    }
}