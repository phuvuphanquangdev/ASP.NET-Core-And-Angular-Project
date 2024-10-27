using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CourseLibrary.API.ValidationAttributes;

namespace CourseLibrary.API.Models
{
    [CourseTitleMustBeDifferentFromDescription(ErrorMessage = "Title must be different from description.")]
    public abstract class CourseForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out the title")]
        [MaxLength(100, ErrorMessage = "The title shouldn't have more than 100 characters")]
        public string Title { get; set; }
        [MaxLength(1500, ErrorMessage = "The description shouldn't have more than 1500 characters")]
        public virtual string Description { get; set; }
    }
}
