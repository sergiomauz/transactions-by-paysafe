namespace Application
{
    public class ApplicationSettings
    {
        public double Timezone { get; set; }
        public decimal TopCurrentValue { get; set; }
        public decimal TopAccumulatedValueByDay { get; set; }
        public List<ApplicationClientsSettings> Clients { get; set; }
    }

    public class ApplicationClientsSettings
    {
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
