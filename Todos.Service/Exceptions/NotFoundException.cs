namespace Todos.Service.Exceptions
{
    public class NotFoundException: Exception
    {
        public override string Message => $"Not Found";
        
    }
}
