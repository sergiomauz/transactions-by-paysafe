namespace Application.ErrorCatalog
{
    public static class ErrorConstants
    {
        // Not documented error
        public static string Generic00000 = "G00000";

        // Paginated
        public static ErrorTuple PaginatedFormat00001 = new("Paginated-F00001", "current_page", "Paginated-F00001");
        public static ErrorTuple PaginatedFormat00002 = new("Paginated-F00002", "page_size", "Paginated-F00002");

        // BasicSearch
        public static ErrorTuple BasicSearchFormat00001 = new("BasicSearch-F00001", "text_filter", "BasicSearch-F00001");

        // Code
        public static ErrorTuple CodeFormat00001 = new("Code-F00001", "code", "Code-F00001");

        // Id
        public static ErrorTuple IdFormat00001 = new("Id-F00001", "id", "Id-F00001");
        public static ErrorTuple IdFormat00002 = new("Id-F00002", "id", "Id-F00002");

        // Ids
        public static ErrorTuple IdsFormat00001 = new("Ids-F00001", "ids", "Ids-F00001");
        public static ErrorTuple IdsFormat00002 = new("Ids-F00002", "ids", "Ids-F00002");
        public static ErrorTuple IdsFormat00003 = new("Ids-F00003", "ids", "Ids-F00003");
        public static ErrorTuple IdsFormat00004 = new("Ids-F00004", "ids", "Ids-F00004");

        // Guid
        public static ErrorTuple GuidFormat00001 = new("Guid-F00001", "id", "Guid-F00001");
        public static ErrorTuple GuidFormat00002 = new("Guid-F00002", "id", "Guid-F00002");

        // Guids
        public static ErrorTuple GuidsFormat00001 = new("Guids-F00001", "ids", "Guids-F00001");
        public static ErrorTuple GuidsFormat00002 = new("Guids-F00002", "ids", "Guids-F00002");
        public static ErrorTuple GuidsFormat00003 = new("Guids-F00003", "ids", "Guids-F00003");
        public static ErrorTuple GuidsFormat00004 = new("Guids-F00004", "ids", "Guids-F00004");

        // CreateAccountTransaction
        public static ErrorTuple CreateAccountTransactionFormat00001 = new("CreateAccountTransaction-F00001", "sourceAccountId", "CreateAccountTransaction-F00001");
        public static ErrorTuple CreateAccountTransactionFormat00002 = new("CreateAccountTransaction-F00002", "sourceAccountId", "CreateAccountTransaction-F00002");
        public static ErrorTuple CreateAccountTransactionFormat00003 = new("CreateAccountTransaction-F00003", "targetAccountId", "CreateAccountTransaction-F00003");
        public static ErrorTuple CreateAccountTransactionFormat00004 = new("CreateAccountTransaction-F00004", "targetAccountId", "CreateAccountTransaction-F00004");
        public static ErrorTuple CreateAccountTransactionFormat00005 = new("CreateAccountTransaction-F00005", "transferTypeId", "CreateAccountTransaction-F00005");
        public static ErrorTuple CreateAccountTransactionFormat00006 = new("CreateAccountTransaction-F00006", "transferTypeId", "CreateAccountTransaction-F00006");
        public static ErrorTuple CreateAccountTransactionFormat00007 = new("CreateAccountTransaction-F00007", "transferTypeId", "CreateAccountTransaction-F00007");
        public static ErrorTuple CreateAccountTransactionFormat00008 = new("CreateAccountTransaction-F00008", "value", "CreateAccountTransaction-F00008");
        public static ErrorTuple CreateAccountTransactionFormat00009 = new("CreateAccountTransaction-F00009", "value", "CreateAccountTransaction-F00009");
        public static ErrorTuple CreateAccountTransactionFormat00010 = new("CreateAccountTransaction-F00010", "ticketValidator", "CreateAccountTransaction-F00010");
        public static ErrorTuple CreateAccountTransactionFormat00011 = new("CreateAccountTransaction-F00011", "transferTypeId", "CreateAccountTransaction-F00011");
        public static ErrorTuple CreateAccountTransactionFormat00012 = new("CreateAccountTransaction-F00012", "transferTypeId", "CreateAccountTransaction-F00012");
        public static ErrorTuple CreateAccountTransactionFormat00013 = new("CreateAccountTransaction-F00013", "transferTypeId", "CreateAccountTransaction-F00013");
        public static ErrorTuple CreateAccountTransactionContent00001 = new("CreateAccountTransaction-C00001", "sourceAccountId", "CreateAccountTransaction-C00001");
        public static ErrorTuple CreateAccountTransactionContent00002 = new("CreateAccountTransaction-C00002", "targetAccountId", "CreateAccountTransaction-C00002");
        public static ErrorTuple CreateAccountTransactionContent00003 = new("CreateAccountTransaction-C00003", "ticketValidator", "CreateAccountTransaction-C00003");
        public static ErrorTuple CreateAccountTransactionContent00004 = new("CreateAccountTransaction-C00004", "-", "CreateAccountTransaction-C00004");

        // UpdateAccountTransaction
        public static ErrorTuple UpdateAccountTransactionFormat00001 = new("UpdateAccountTransaction-F00001", "id", "UpdateAccountTransaction-F00001");
        public static ErrorTuple UpdateAccountTransactionFormat00002 = new("UpdateAccountTransaction-F00002", "id", "UpdateAccountTransaction-F00002");
        public static ErrorTuple UpdateAccountTransactionFormat00003 = new("UpdateAccountTransaction-F00003", "transactionStatus", "UpdateAccountTransaction-F00003");
        public static ErrorTuple UpdateAccountTransactionFormat00004 = new("UpdateAccountTransaction-F00004", "transactionStatus", "UpdateAccountTransaction-F00004");
        public static ErrorTuple UpdateAccountTransactionFormat00005 = new("UpdateAccountTransaction-F00005", "transactionStatus", "UpdateAccountTransaction-F00005");
        public static ErrorTuple UpdateAccountTransactionContent00001 = new("UpdateAccountTransaction-C00001", "id", "UpdateAccountTransaction-C00001");
        public static ErrorTuple UpdateAccountTransactionContent00002 = new("UpdateAccountTransaction-C00002", "transactionStatus", "UpdateAccountTransaction-C00002");
    }
}
