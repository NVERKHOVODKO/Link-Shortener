namespace link_shortener.DTO;

public class EditLinkRequest
{
    public Guid Id { get; set; }
    public string NewLongUrl { get; set; }
}