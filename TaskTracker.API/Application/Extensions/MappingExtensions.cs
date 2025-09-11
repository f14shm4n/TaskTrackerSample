using Azure.Core;
using System.Threading.Tasks;
using TaskTracker.API.Application.Commands;
using TaskTracker.API.Application.Dto;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Extensions
{
    public static class MappingExtensions
    {
        public static WorkAssignment ToWorkAssignment(this CreateWorkAssignmentCommand request)
        {
            return new WorkAssignment(request.Title, request.Status, request.Priority, request.Author, request.Worker);
        }

        public static WorkAssignmentDTO ToWorkAssignmentDto(this WorkAssignment workAssignment, bool skipHeadAssignment = false)
        {
            var dto = CreateWorkAssignmentDto(workAssignment);

            if (!skipHeadAssignment && workAssignment.HeadAssignment is not null)
            {
                dto.HeadAssignment = CreateWorkAssignmentDto(workAssignment.HeadAssignment);
            }

            if (workAssignment.SubAssignment.Count > 0)
            {
                dto.SubAssignments = workAssignment.SubAssignment.Select(x => x.ToWorkAssignmentDto(skipHeadAssignment: true)).ToList();
            }
            if (workAssignment.OutRelations.Count > 0)
            {
                dto.OutRelations = workAssignment.OutRelations.Select(x => x.ToWorkAssignmentRelationshipDto()).ToList();
            }
            if (workAssignment.InRelations.Count > 0)
            {
                dto.InRelations = workAssignment.InRelations.Select(x => x.ToWorkAssignmentRelationshipDto()).ToList();
            }
            return dto;
        }

        private static WorkAssignmentDTO CreateWorkAssignmentDto(WorkAssignment workAssignment)
        {
            return new WorkAssignmentDTO
            {
                Id = workAssignment.Id,
                Title = workAssignment.Title,
                Status = workAssignment.Status,
                Priority = workAssignment.Priority,
                Author = workAssignment.Author,
                Worker = workAssignment.Worker,
                HeadAssignmentId = workAssignment.HeadAssignemtId
            };
        }

        public static WorkAssignmentRelationshipDto ToWorkAssignmentRelationshipDto(this WorkAssignmentRelationship relationship)
        {
            return new WorkAssignmentRelationshipDto
            {
                RelationType = relationship.Relation,
                SourceId = relationship.SourceWorkAssignmentId,
                TargetId = relationship.TargetWorkAssignmentId
            };
        }
    }
}
