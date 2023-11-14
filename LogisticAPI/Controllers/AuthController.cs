using Microsoft.AspNetCore.Mvc;
using LogisticApi.Dtos;
using LogisticApi.Helpers;
using LogisticApi.Models;
using LogisticApi.Dtos.Auth;
using LogisticApi.Dtos.Result;
using Microsoft.AspNetCore.Http.HttpResults;
using NuGet.Protocol;

namespace LogisticApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		private readonly UzserLojistikContext _context;

		IConfiguration _configuration;

		public AuthController(UzserLojistikContext context)
		{
			_context = context;
			_configuration = new ConfigurationBuilder()
							.AddJsonFile("appsettings.json")
							.Build();
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromForm] AuthDto request)
		{
			try
			{
				
				
				if (_context.Companies.Where(c=>c.FirmNumber == request.FirmNumber).Count() == 0)
				{
					return BadRequest(new ErrorResult("Tanımsız Firma Kodu"));
				}
				Company _company = _context.Companies.Where(c => c.FirmNumber == request.FirmNumber).First();

				//kullanıcı tanımlı mı ?
				if (_context.Users.Where(u=>u.UserName == request.Username).Count() == 0)
				{
					return BadRequest(new ErrorResult("Tanımsız Kullanıcı"));
				}

				//parola doğru mu
				request.Password = HashingHelper.Md5Hash(request.Password);
				
				if (_context.Users.Where(u => u.UserName == request.Username && u.Password == request.Password).Count() == 0)
				{
					return BadRequest(new ErrorResult("Parola hatalı"));
				}
				//kullanıcıyı al
				User _user = _context.Users.Where(u => u.UserName == request.Username && u.Password == request.Password).First();
				
				//kullanıcı rolü var mı
				if (_context.UserRoles.Where(u => u.UserId == _user.Id && u.CompanyId == _company.Id).Count() ==0)
				{
					return BadRequest(new ErrorResult("Kullanıcı rolü tanımsız"));
				}

				//admin mi
				//Kullanıcı rolünü al
				Role _roleMain = _context.Roles.Where(r => r.Id == _context.UserRoles.Where(u => u.UserId == _user.Id).First().RoleId).First()  ;

				bool _blIsAdmin = _context.Roles.Where(r => r.Id == _roleMain.Id).First().IsAdmin;

				//şirket 0 ve kullanıcı admin ise giriş yapabilir

				if (_blIsAdmin == true && request.FirmNumber == 0)
				{
					//sistem admin
					//şirket yetkilendirmesine gerek yok...
				}
				else
				{
					//kullanıcıya şirket tanımlanmış mı?
					if (_context.UserCompanies.Where(c => c.UserId == _user.Id && c.CompanyId == _company.Id).Count()==0)
					{
						return BadRequest(new ErrorResult("Kullanıcının seçilen şirkete giriş yetkisi yok"));
					}
				}

				TokenHandler tokenHandler = new TokenHandler(_configuration);
				AuthResponse response = new AuthResponse();
				response.Token = tokenHandler.CreateAccessToken();
				response.UserInfo = _context.Users.Where(u => u.UserName == request.Username && u.Password == request.Password).First();
				response.UserInfo.UserModules = new List<UserModule>();
				if (_blIsAdmin)
				{
					foreach (Module _mdl in _context.Modules)
					{
						UserModule _umd = new UserModule();
						_umd.UserId = response.UserInfo.Id;
						_umd.ModuleId = _mdl.Id;
						response.UserInfo.UserModules.Add(_umd);
					}
				}
				else
				{
					//kullanıcını rolüne göre yetkileri donecek
					List<RoleModule> _RolModul = _context.RoleModules.Where(r => r.RoleId == _roleMain.Id).ToList();
					List<string> parents = new List<string>();
					foreach (RoleModule _mdl in _RolModul)
                    {
                        UserModule _umd = new UserModule();
                        _umd.UserId = response.UserInfo.Id;
                        _umd.ModuleId = _mdl.ModuleId;
                        response.UserInfo.UserModules.Add(_umd);
                    }
                }
				
				response.UserInfo.RefreshToken = tokenHandler.CreateRefreshToken();
				response.UserInfo.RefreshTokenEndDate = response.Token.Expiration.AddHours(1);

				//var userUpdateResult = await _authService.SaveRefreshTokenAsync(response.User);
				//if (!userUpdateResult.Success)
				//	return BadRequest(userUpdateResult);

				return Ok(new SuccessDataResult<AuthResponse>(response, string.Empty));
			}
			catch (Exception e)
			{
				var error = new ErrorDataResult<AuthResponse>(e.Message);
				return BadRequest(error);
			}
		}

		//[HttpPost("LoginWithRefreshToken")]
		//public async Task<IActionResult> RefreshToken([FromForm] string refreshToken)
		//{
		//	var user = await _authService.GetRefreshTokenAsync(refreshToken);
		//	var test = user;
		//	if (user.Data != null)
		//	{
		//		var tokenHandler = new TokenHandler(_configuration);
		//		var token = tokenHandler.CreateAccessToken();
		//		user.Data.RefreshToken = token.RefreshToken;
		//		user.Data.RefreshTokenEndDate = token.Expiration.AddMinutes(30);
		//		var tokenResult = await _authService.SaveRefreshTokenAsync(user.Data);
		//		if (!tokenResult.Success)
		//			return BadRequest(new ErrorResult(ControllerMessages.AuthAccessTokenError!));

		//		AuthResponse response = new AuthResponse
		//		{
		//			Token = token,
		//			User = user.Data
		//		};

		//		return Ok(new SuccessDataResult<AuthResponse>(response, ControllerMessages.AuthLoginSuccess!));
		//	}
		//	return BadRequest(new ErrorDataResult<TokenDto>(ControllerMessages.RefreshTokenTimeout!));
		//}

	}
}
