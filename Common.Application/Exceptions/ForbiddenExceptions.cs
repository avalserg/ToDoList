namespace Common.Application.Exceptions
{
    public class ForbiddenExceptions:Exception
    {
        public override string Message => $"Forbidden";

        public ForbiddenExceptions() 
        {

        }
    }
}
