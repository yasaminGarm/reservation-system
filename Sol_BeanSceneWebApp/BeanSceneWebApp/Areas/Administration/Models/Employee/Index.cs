namespace BeanSceneWebApp.Areas.Administration.Models.Employee
{
    public class Index
    {
        public class UserInfo
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public bool IsAdmin { get; set; }
        }
        public List<UserInfo> Users { get; set; }
    }
}
