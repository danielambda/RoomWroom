namespace Contracts.Receipts;

public record CreateReceiptFromFiscalRequest(
    DateTime DateTime,
    decimal Sum,
    string FiscalDrive,
    string FiscalDocument,
    string FiscalSign
);
