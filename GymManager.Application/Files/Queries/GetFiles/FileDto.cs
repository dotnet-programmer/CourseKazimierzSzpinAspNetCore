namespace GymManager.Application.Files.Queries.GetFiles;

public class FileDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public long Bytes { get; set; }
	public string Url { get; set; }
}