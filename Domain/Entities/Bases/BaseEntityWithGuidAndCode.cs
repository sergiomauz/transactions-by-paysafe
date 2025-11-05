namespace Domain.Entities.Bases
{
    public abstract class BaseEntityWithGuidAndCode : BaseEntityWithGuid
    {
        public string? Code { get; set; }
    }
}
