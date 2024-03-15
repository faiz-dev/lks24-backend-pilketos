namespace BackendPilketos.Exceptions
{
    public class ClientError: Exception
    {
        public int ErrorCode { get; set; }

        public ClientError() { }

        public ClientError(string message) : base(message)
        {
            ErrorCode = 400;
        }
    }
}
