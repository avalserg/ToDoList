namespace Common.Domain
{
    public class RefreshToken
    {
        public string Id { get; set; } = default!;
        public int ApplicationUserId { get; set; }
        //user owner token
        public ApplicationUser ApplicationUser { get;set; }=default!;
    }
}
