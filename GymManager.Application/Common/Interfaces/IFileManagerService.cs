using Microsoft.AspNetCore.Http;

namespace GymManager.Application.Common.Interfaces;

public interface IFileManagerService
{
	Task Upload(IEnumerable<IFormFile> files);
	void Delete(string name);
}