namespace BookCollection.Domain;

public class OperationResult
{
    public OperationStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
}
