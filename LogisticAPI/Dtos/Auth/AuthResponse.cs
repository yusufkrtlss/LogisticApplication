using LogisticApi.Models;

namespace LogisticApi.Dtos.Auth
{
	public class AuthResponse 
	{
		public TokenDto Token { get; set; }
		public User UserInfo { get; set; }
	}
}
