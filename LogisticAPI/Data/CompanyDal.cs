using Dapper;
using LogisticApi.Data;
using LogisticApi.Models;
using LogisticApi.Dtos.Result;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace LogisticManagementApi.Data
{
	public class CompanyDal
	{
		private DapperRepository<Company> repository;

		public CompanyDal()
		{
			repository = new DapperRepository<Company>();
		}
		public async Task<IDataResult<IEnumerable<Company>>> GetCompanyListAsync()
		{
			return await repository.GetAllAsync("SELECT * FROM Companies");
		}

		public async Task<IDataResult<Company>> GetCompanyByIdAsync(Guid companyId)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("@CompanyID", companyId);
			return await repository.FirstOrDefaultAsync($"SELECT * FROM Companies where Id=@CompanyID", p);
		}

		public async Task<LogisticApi.Dtos.Result.IResult> SaveCompanyAsync(Company company)
		{
			//Sp_SaveCompany(@Id uniqueidentifier, @CompanyName nvarchar(512), @TaxNumber varchar(11), @Address nvarchar(512), @FirmNumber int)
			DynamicParameters p = new DynamicParameters();
			p.Add("@Id", company.Id);
			p.Add("@CompanyName", company.CompanyName);
			p.Add("@TaxNumber", company.TaxNumber);
			p.Add("@Address", company.Address);
			p.Add("@FirmNumber", company.FirmNumber);
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
