using MediatR;

namespace GymManager.Application.Settings.Queries.GetSettings;

// zwraca listę nagłówków wraz z listą pozycji dla każdego nagłówka
public class GetSettingsQuery : IRequest<IList<SettingsDto>>
{
}