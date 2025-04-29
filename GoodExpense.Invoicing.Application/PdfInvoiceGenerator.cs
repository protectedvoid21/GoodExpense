using GoodExpense.Invoicing.Domain;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Logging;

namespace GoodExpense.Invoicing.Application;

public class PdfInvoiceGenerator : IInvoiceGenerator
{
    private readonly ILogger<PdfInvoiceGenerator> _logger;
    private readonly string _savePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoices");

    public PdfInvoiceGenerator(ILogger<PdfInvoiceGenerator> logger)
    {
        _logger = logger;
    }

    public async Task<string> GenerateExpenseReportAsync(CreateInvoiceRequest request)
    {
        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }
        
        string fileName = $"{request.AuthorUserName}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        string filePath = Path.Combine(_savePath, fileName);
        
        using var writer = new PdfWriter(filePath);
        using var pdf = new PdfDocument(writer);
        using var document = new Document(pdf);
        try
        {
            var titleFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
            var headerFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
            var normalFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);
                
            document.Add(new Paragraph("Raport Wydatków").SetFont(titleFont).SetFontSize(16));
            document.Add(new Paragraph($"Wygenerowano dla użytkownika: {request.AuthorUserName}").SetFont(normalFont).SetFontSize(10));
            document.Add(new Paragraph($"Okres: {request.FromDate.ToShortDateString()} - {request.ToDate.ToShortDateString()}").SetFont(normalFont).SetFontSize(10));
            document.Add(new Paragraph(" "));

            if (request.Expenses.Any())
            {
                var table = new Table(4);
                table.SetWidth(UnitValue.CreatePercentValue(100));

                table.AddHeaderCell(new Cell().Add(new Paragraph("ID").SetFont(headerFont).SetFontSize(12)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Opis").SetFont(headerFont).SetFontSize(12)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Kwota").SetFont(headerFont).SetFontSize(12)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Data").SetFont(headerFont).SetFontSize(12)));

                foreach (var expense in request.Expenses)
                {
                    var expenseSumAmount = expense.ParticipantUsers.Sum(p => p.Amount);
                        
                    table.AddCell(new Cell().Add(new Paragraph(expense.Title).SetFont(normalFont).SetFontSize(10)));
                    table.AddCell(new Cell().Add(new Paragraph(expense.Description).SetFont(normalFont).SetFontSize(10)));
                    table.AddCell(new Cell().Add(new Paragraph($"{expenseSumAmount:C}").SetFont(normalFont).SetFontSize(10)).SetTextAlignment(TextAlignment.RIGHT));
                    table.AddCell(new Cell().Add(new Paragraph($"{expense.CreatedDate}").SetFont(normalFont).SetFontSize(10)).SetTextAlignment(TextAlignment.RIGHT));
                }
                document.Add(table);

                decimal amountSum = request.Expenses.Sum(e => e.ParticipantUsers.Sum(p => p.Amount));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph($"Suma wydatków w okresie: {amountSum:C}").SetFont(headerFont).SetFontSize(12));
            }
            else
            {
                document.Add(new Paragraph("Brak wydatków w wybranym okresie.").SetFont(normalFont).SetFontSize(10));
            }

            document.Close();
            return Convert.ToBase64String(await File.ReadAllBytesAsync(filePath));
        }
        catch (Exception ex)
        {
            _logger.LogError("Could not generate PDF document: {ExMessage}", ex.Message);
            return null;
        }
    }
}