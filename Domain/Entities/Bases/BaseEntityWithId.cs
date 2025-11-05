namespace Domain.Entities.Bases
{
    public abstract class BaseEntityWithId
    {
        public int? Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
