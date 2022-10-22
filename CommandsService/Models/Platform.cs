using System.Collections;
using System.ComponentModel.DataAnnotations;

public class Platform
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public int ExtenralId { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<Command> Commands { get; set; } = new HashSet<Command>();
}