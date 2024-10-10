namespace DailyCashFlow.Domain.Features.Users
{
	public class User : BasicEntity
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; private set; }
		public bool IsAdministrator { get; private set; }

		public void SetPassword(string password)
		{
			Password = BCrypt.Net.BCrypt.HashPassword(password);
		}
	}
}
