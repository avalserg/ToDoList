namespace Common.Service.Exceptions;

public class BadRequestException:Exception
{
    //public override string Message => "Bad Request";
    public BadRequestException()
    {
        
    }
    public BadRequestException(string message):base(message)
    {
        
    }
        
}