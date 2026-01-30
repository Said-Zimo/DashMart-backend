

namespace DashMart.Domain.Abstraction
{
    public abstract class Entity
    {
        public Entity()
        {
            PublicId = Guid.NewGuid();
        }

        public int Id { get; set; }
        public Guid PublicId { get;}
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CratedAt { get; set; }
    }
}
