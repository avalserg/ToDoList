namespace Todos.Service.Exceptions;

public class BadRequestException:Exception
{
    public override string Message => "Bad Request";
        
}