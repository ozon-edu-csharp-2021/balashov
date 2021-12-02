using System;

namespace OzonEdu.MerchandiseService.Infrastructure.Repositories.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public int ClothingSize { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime HiringDate { get; set; }
    }
}
