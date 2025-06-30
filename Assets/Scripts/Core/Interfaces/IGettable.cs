namespace Core.Interfaces
{
    public interface IGettable<out T>
    {
        T Get();
    }
}