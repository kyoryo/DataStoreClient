using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreClient
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Board> Boards { get; set; } 
    }
    public class Board
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
