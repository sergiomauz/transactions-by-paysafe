namespace Application
{
    public class ApplicationSettings
    {
        public double Timezone { get; set; }
        public decimal TopCurrentValue { get; set; }
        public decimal TopAccumulatedValueByDay { get; set; }
        public List<ApplicationClientsSettings> Clients { get; set; } = new();
    }

    public class ApplicationClientsSettings
    {
        public double ClientId { get; set; }
        public double PublicKey { get; set; }
        public double Signature { get; set; }
    }
}
