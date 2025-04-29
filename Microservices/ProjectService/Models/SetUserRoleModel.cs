namespace ProjectService.Models
{
    public class SetUserRoleModel
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public RoleModel Role { get; set; }
    }
}
