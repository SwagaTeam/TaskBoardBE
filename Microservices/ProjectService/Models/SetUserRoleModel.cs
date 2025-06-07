using SharedLibrary.Models;
using System.Text.Json.Serialization;

namespace ProjectService.Models
{
    public class SetUserRoleModel
    {
        [JsonRequired]
        public int UserId { get; set; }
        [JsonRequired]
        public int ProjectId { get; set; }
        [JsonRequired]
        public RoleModel? Role { get; set; }
    }
}
