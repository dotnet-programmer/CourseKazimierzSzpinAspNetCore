using GymManager.Application.Files.Queries.GetFiles;

namespace GymManager.Application.Files.Extensions;

public static class FileExtensions
{
	public static FileDto ToDto(this Domain.Entities.File file) =>
		file == null ?
		null :
		new FileDto
		{
			Name = file.Name,
			Bytes = file.Bytes,
			Description = file.Description,
			Id = file.FileId,
			Url = $"/Content/Files/{file.Name}"
		};
}