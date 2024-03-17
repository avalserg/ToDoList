namespace Common.Domain
{
    public class ApplicationUserApplicationRole
    {
         public int ApplicatonUserId { get; set; }
         public ApplicationUser ApplicationUser { get; set; } = default!;
         public int ApplicatonUserRoleId { get; set; }
         public ApplicationUserRole ApplicationUserRole { get; set; } = default!;
    }
}
