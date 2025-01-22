using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Files.Commands.UploadFile;

public class UploadFileCommandHandler(IApplicationDbContext context, IFileManagerService fileManagerService) : IRequestHandler<UploadFileCommand>
{
	private readonly IApplicationDbContext _context = context;
	private readonly IFileManagerService _fileManagerService = fileManagerService;

	// wszystkie szczegółowe informacje o pliku są w bazie danych, ale sam plik będzie w folderze na serwerze
	// dodanie wybranych plików na serwer do konkretnego folderu
	// dodać infomacje o pliku do bazy danych
	public async Task Handle(UploadFileCommand request, CancellationToken cancellationToken)
	{
		// przesłanie wszystkich plików na serwer
		await _fileManagerService.Upload(request.Files);

		// dodanie wszystkich plików do bazy danych
		foreach (var file in request.Files)
		{
			// sprawdzenie czy istnieje taki plik w bazie danych
			var fileInDb = await _context.Files.FirstOrDefaultAsync(x => x.Name == file.FileName, cancellationToken);

			// jeśli nie istnieje, to dodanie do bazy
			if (fileInDb == null)
			{
				AddFile(file);
			}
			// jeśli istnieje, to aktualizacja danych o tym pliku
			else
			{
				UpdateFile(file, fileInDb);
			}
		}

		// zapis zmian w bazie
		await _context.SaveChangesAsync(cancellationToken);
	}

	private void AddFile(IFormFile file) => 
		_context.Files.Add(new()
		{
			Bytes = file.Length,
			Description = file.FileName,
			Name = file.FileName
		});

	private void UpdateFile(IFormFile file, Domain.Entities.File fileInDb)
	{
		fileInDb.Description = file.FileName;
		fileInDb.Bytes = file.Length;
	}
}