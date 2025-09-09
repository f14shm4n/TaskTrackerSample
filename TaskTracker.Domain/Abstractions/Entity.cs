namespace TaskTracker.Domain.Abstractions
{
    public abstract class Entity
    {
        private int _id;

        public virtual int Id
        {
            get => _id;
            protected set => _id = value;
        }
    }
}
