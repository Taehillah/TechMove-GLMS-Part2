using System.ComponentModel.DataAnnotations;

namespace TechMoveGLMS.Web.Models.Entities;

public class Client
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, Display(Name = "Contact Details"), StringLength(250)]
    public string ContactDetails { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Region { get; set; } = string.Empty;

    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
