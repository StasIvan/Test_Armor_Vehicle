using Interfaces;

namespace GameItems
{
    public abstract class BaseItemMove : IMovable
    {
        public abstract void Move();

        public abstract void Stop();
    }
}