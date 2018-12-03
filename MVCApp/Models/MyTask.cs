using System.Data.Entity;

namespace MVCApp.Models
{
    public class MyTask
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public bool IsDone { get; set; }
    }

    public class MyTaskDbContext : DbContext
    {
        public DbSet<MyTask> MyTasks { get; set; }
    }
}