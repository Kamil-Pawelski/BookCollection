namespace BookCollection.Domain;

public enum OperationStatusCode
{
    Ok = 200,
    NotFound = 404,
    NotAllowed = 405,
    InternalError = 500
}
