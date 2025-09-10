namespace TaskTracker.Domain.Abstractions
{
    public abstract class Entity
    {
        public virtual int Id { get; protected set; }
    }
}
