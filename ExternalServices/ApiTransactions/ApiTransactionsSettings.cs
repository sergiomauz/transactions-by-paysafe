namespace ExternalServices.ApiTransactions
{
    public class ApiTransactionsSettings
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string RemoteBaseUrl { get; set; }
        public int RemotePort { get; set; }
        public int Timeout { get; set; }
        public ApiTransactionsSettingsResources Resources { get; set; }
    }

    public class ApiTransactionsSettingsResources
    {
        public string UpdateTransaction { get; set; }
    }
}
