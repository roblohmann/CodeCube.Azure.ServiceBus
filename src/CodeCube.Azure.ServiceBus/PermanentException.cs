namespace CodeCube.Azure.ServiceBus
{
    public class PermanentException : Exception
    {
        public PermanentException(string message) : base(message)
        {
        }
    }
}
