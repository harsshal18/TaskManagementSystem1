using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem1.Models
{
    public class TaskModel
    {
        [Key] // ✅ tells MVC this is the identity key
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(250)]
        public string Title { get; set; }

        public string Description { get; set; }             


        [Required(ErrorMessage = "Assigned To is required.")]

        [StringLength(150)]
        public string AssignedTo { get; set; }

        [Required(ErrorMessage = "DueDate is required.")]

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }


        [Required(ErrorMessage = "Status is required.")]

        [StringLength(50)]
        public string Status { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
