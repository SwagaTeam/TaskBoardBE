using SharedLibrary.Models;

namespace SharedLibrary.ProjectModels
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public string Head { get; private set; } = "";

        public void SetHead(string head)
        {
            Head = head;
        }

        public int Priority { get; set; }
        public string Status
        {
            get
            {
                var status = Constants.ProjectStatuses.Names[Priority];

                if (status is not null)
                {
                    return status;
                }
                else
                {
                    return "Без статуса";
                }
            }
        }

        public virtual ICollection<UserProjectModel> UserProjects { get; set; } = new List<UserProjectModel>();
    }
}
