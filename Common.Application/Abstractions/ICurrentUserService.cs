namespace Common.Application.Abstractions
{
    public interface ICurrentUserService
    {
        public string CurrentUserId { get; }
        public string[] CurrentUserRoles { get; }
    }
}
