using System.ComponentModel.DataAnnotations;

public class Platform
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Publisher { get; set; }

    [Required]
    public string Cost { get; set; }
    public Platform(string name, string publisher, string cost)
    {
        this.Name = name;
        this.Publisher = publisher;
        this.Cost = cost;

    }


}