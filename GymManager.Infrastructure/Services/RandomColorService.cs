using GymManager.Application.Common.Interfaces;

namespace GymManager.Infrastructure.Services;

public class RandomColorService : IRandomColorService
{
	private readonly Random _random = new();

	// zwraca kolor w formacie #ffffff
	public string GetColor()
		=> string.Format("#{0:X6}", _random.Next(0x1_000_000));
}