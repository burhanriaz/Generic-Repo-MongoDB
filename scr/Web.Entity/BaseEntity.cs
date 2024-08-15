namespace Web.Entity
{
    public class BaseEntity  : IBaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}