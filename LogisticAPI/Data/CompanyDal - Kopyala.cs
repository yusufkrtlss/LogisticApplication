using Dapper;
using LogisticApi.Data;
using LogisticApi.Models;
using LogisticApi.Dtos.Result;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LogisticApi.Data
{
    public class RoleDal
	{
		private DapperRepository<Role> repository;

		public RoleDal()
		{
			repository = new DapperRepository<Role>();
		}
		public async Task<IDataResult<IEnumerable<Role>>> GetRolesListAsync()
		{
			return await repository.GetAllAsync("SELECT * FROM Roles");
		}

		public async Task<IDataResult<Role>> GetRoleByIdAsync(Guid roleId)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("@RoleID", roleId);
			return await repository.FirstOrDefaultAsync($"SELECT * FROM Roles where Id=@RoleID", p);
		}

		public async Task<LogisticApi.Dtos.Result.IResult> SaveCompanyAsync(Role role)
		{
			//Sp_SaveCompany(@Id uniqueidentifier, @CompanyName nvarchar(512), @TaxNumber varchar(11), @Address nvarchar(512), @FirmNumber int)
			DynamicParameters p = new DynamicParameters();
			p.Add("@Id", role.Id);
            p.Add("@RoleName", role.RoleName);
            p.Add("@RoleNameEn", role.RoleNameEn);
            p.Add("@IsAdmin", role.IsAdmin);
            p.Add("@IsDeleted", role.IsDeleted);
			p.Add("@Lastupdated", role.LastUpdated);
			p.Add("@LastUpdatedBy", role.LastUpdatedBy);
			return await repository.ExecuteProcedureAsync("Sp_SaveCompany", p);
		}

		public async Task<LogisticApi.Dtos.Result.IResult> DeleteCompanyAsync(Guid companyId)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("@CompanyId", companyId);
			return await repository.ExecuteQueryAsync("UPDATE Companies SET IsDeleted=1 where Id=@CompanyId", p);
		}

	}
}
