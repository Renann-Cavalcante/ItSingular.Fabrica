using ItSingular.RH.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItSingular.RH.Application.Interfaces
{
	public interface IProfissionaisService
    {
		Task<ProfissionaisDTO> AddProfissionais(ProfissionaisDTO newProfissionaisDTO);

		Task<ProfissionaisDTO> UpdateProfissionais(ProfissionaisDTO ProfissionaisDTO);

		Task<bool> DeleteProfissionais(ProfissionaisDTO ProfissionaisDTO);

		Task<IEnumerable<ProfissionaisDTO>> GetProfissionaisList();

		Task<ProfissionaisDTO> GetProfissionaisById(int id);

	}
}
