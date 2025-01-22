using GymManager.Application.GymInvoices.Queries.GetPdfGymInvoice;
using GymManager.UI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize]
public class InvoiceController(ILogger<InvoiceController> logger) : BaseController
{
	private readonly ILogger<InvoiceController> _logger = logger;

	public async Task<IActionResult> InvoiceToPdf(int id)
	{
		try
		{
			var invoicePdfVm = await Mediator.Send(new GetPdfGymInvoiceQuery { Id = id });

			TempData.Put(invoicePdfVm.Handle, invoicePdfVm.PdfContent);

			return Json(new
			{
				success = true,
				fileGuid = invoicePdfVm.Handle,
				fileName = invoicePdfVm.FileName
			});
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, null);
			return Json(new { success = false });
		}
	}

	public IActionResult DownloadInvoicePdf(string fileGuid, string fileName)
		=> TempData[fileGuid] != null
			? (IActionResult)File(TempData.Get<byte[]>(fileGuid), "application/pdf", fileName)
			: throw new Exception("Błąd przy próbie eksportu faktury do PDF.");
}