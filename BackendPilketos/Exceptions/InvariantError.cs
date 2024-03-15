namespace BackendPilketos.Exceptions
{
    public class InvariantError : ClientError
    {
        public InvariantError(string message) : base(message) { ErrorCode = 404; }
    }
}
