namespace OzonEdu.MerchandiseService.Infrastructure.Repositories.Models
{
    class HrAssignTasks
    {
        public int? Id { get; set; }

        public int HrManagerId { get; set; }

        public int AssignedTasks { get; set; }
    }
}
