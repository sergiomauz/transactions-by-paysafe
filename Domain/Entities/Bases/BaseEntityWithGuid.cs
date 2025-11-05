namespace Domain.Entities.Bases
{
    public abstract class BaseEntityWithGuid
    {
        public Guid? Id { get; set; } = Guid.NewGuid();
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
