package conversion.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime EnrollmentDate 
    { 
        get => _enrollmentDate; 
        set
        {
            if (value > DateTime.Now)
            {
                throw new ArgumentException("Enrollment date cannot be in the future.");
            }
            _enrollmentDate = value;
        }
    }
    private DateTime _enrollmentDate;
}