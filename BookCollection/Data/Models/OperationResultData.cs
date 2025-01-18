namespace BookCollection.Data.Models;

public class OperationResultData<T>
{
    public T? Data { get; set; }
    public OperationStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
}
