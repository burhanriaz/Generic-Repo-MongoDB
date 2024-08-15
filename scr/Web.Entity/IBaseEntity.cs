namespace Web.Entity
{ 
    public interface IBaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }

    //public class BaseEntity 
    //{
    //    public string Id { get; set; }

    //    public DateTime CreateTime { get; set; }

    //    public DateTime UpdateTime { get; set; }
    //}
   
}