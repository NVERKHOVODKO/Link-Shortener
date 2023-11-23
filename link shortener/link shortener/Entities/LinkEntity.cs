using System.ComponentModel.DataAnnotations;
using Entities;

namespace TestApplication.Models;

public class LinkEntity : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public string LongUrl { get; set; }
    [Required] public string ShortUrl { get; set; }
    [Required] public int ClickCount { get; set; }
}