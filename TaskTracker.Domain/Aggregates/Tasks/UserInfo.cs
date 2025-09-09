using TaskTracker.Domain.Abstractions;

namespace TaskTracker.Domain.Aggregates.Tasks
{
    public class UserInfo : ValueObject
    {
        protected UserInfo()
        {
        }

        public UserInfo(string name)
        {
            Name = name;
        }

        public string Name { get; private set; } = string.Empty;

        public void SetName(string name)
        {
            Name = name;
        }
    }
}
