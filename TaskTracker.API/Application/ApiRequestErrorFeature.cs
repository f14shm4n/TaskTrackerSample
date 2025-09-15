using System.Net;

namespace TaskTracker.API.Application
{
    public record ApiRequestErrorFeature
    {
        public string? Title { get; init; }
        public string? Description { get; init; }

        public ApiRequestErrorFeature(string? title)
        {
            Title = title;
        }

        public ApiRequestErrorFeature(string? title, string? description)
        {
            Title = title;
            Description = description;
        }
    }
}