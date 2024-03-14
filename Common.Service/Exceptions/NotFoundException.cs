namespace Common.Service.Exceptions
{
    public class NotFoundException: Exception
    {
        //public override string Message => $"Not Found";
        public NotFoundException()
        {
            
        }
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
