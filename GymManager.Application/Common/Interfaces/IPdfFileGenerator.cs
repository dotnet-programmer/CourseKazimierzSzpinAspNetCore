using GymManager.Application.Common.Models;

namespace GymManager.Application.Common.Interfaces;

public interface IPdfFileGenerator
{
	Task<byte[]> GetAsync(FileGeneratorParams @params);
}