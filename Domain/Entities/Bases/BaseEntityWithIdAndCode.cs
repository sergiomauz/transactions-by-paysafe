namespace Domain.Entities.Bases
{
    public abstract class BaseEntityWithIdAndCode : BaseEntityWithId
    {
        public string? Code { get; set; }
    }
}
