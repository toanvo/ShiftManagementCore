namespace ShiftManagement.Web.Models
{
    using System;    
    using System.ComponentModel.DataAnnotations;    

    public class EmployeeModel
    {
        public int Id { get; set; }

        [Required]        
        public string UserName { get; set; }
        public string Email { get; set; }

        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
