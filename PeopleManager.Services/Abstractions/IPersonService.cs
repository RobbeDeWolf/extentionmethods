using PeopleManager.Services.Model.Requests;
using PeopleManager.Services.Model.Results;
using Vives.Services.Model;

namespace PeopleManager.Services.Abstractions
{
	public interface IPersonService
	{
		Task<PersonResult?> GetAsync(int id);
		Task<IList<PersonResult>> FindAsync();
        Task<ServiceResult<PersonResult>> CreateAsync(PersonRequest person);
        Task<ServiceResult<PersonResult>> UpdateAsync(int id, PersonRequest person);
        Task<ServiceResult> DeleteAsync(int id);
	}
}
