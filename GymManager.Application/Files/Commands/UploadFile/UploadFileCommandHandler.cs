﻿using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Files.Commands.UploadFile;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand>
{
	private readonly IApplicationDbContext _context;
	private readonly IFileManagerService _fileManagerService;

	public UploadFileCommandHandler(IApplicationDbContext context, IFileManagerService fileManagerService)
	{
		_context = context;
		_fileManagerService = fileManagerService;
	}

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
			var fileInDb = await _context.Files.FirstOrDefaultAsync(x => x.Name == file.FileName);

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

		return;
	}

	private void AddFile(IFormFile file)
	{
		var newFile = new Domain.Entities.File
		{
			Bytes = file.Length,
			Description = file.FileName,
			Name = file.FileName
		};

		_context.Files.Add(newFile);
	}

	private void UpdateFile(IFormFile file, Domain.Entities.File fileInDb)
	{
		fileInDb.Description = file.FileName;
		fileInDb.Bytes = file.Length;
	}
}