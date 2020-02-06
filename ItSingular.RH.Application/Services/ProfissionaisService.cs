using AutoMapper;
using ItSingular.RH.Application.DTOs;
using ItSingular.RH.Application.Interfaces;
using ItSingular.RH.Domain.Entities;
using ItSingular.RH.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItSingular.RH.Application.Services
{
	public class ProfissionaisService : IProfissionaisService
    {
		private readonly IProfissionaisRepository _profissionaisRepository;
		private readonly IMapper _mapper;

		public ProfissionaisService(IProfissionaisRepository profissionaisRepository, IMapper mapper)
		{
			_profissionaisRepository = profissionaisRepository;
			_mapper = mapper;
		}

		public async Task<ProfissionaisDTO> AddProfissionais(ProfissionaisDTO newProfissionaisDTO)
		{
			var Profissionais = _mapper.Map<Profissionais>(newProfissionaisDTO);

			_profissionaisRepository.Add(Profissionais);

			var success = await _profissionaisRepository.SavelAll();

			return newProfissionaisDTO;
		}

		public async Task<ProfissionaisDTO> UpdateProfissionais(ProfissionaisDTO ProfissionaisDTO)
		{
			var Profissionais = _mapper.Map<Profissionais>(ProfissionaisDTO);

			_profissionaisRepository.Update(Profissionais);

			var success = await _profissionaisRepository.SavelAll();

			return ProfissionaisDTO;

		}

		public async Task<bool> DeleteProfissionais(ProfissionaisDTO ProfissionaisDTO)
		{
			var Profissionais = _mapper.Map<Profissionais>(ProfissionaisDTO);

			_profissionaisRepository.Delete(Profissionais);

			var success = await _profissionaisRepository.SavelAll();

			return success;
		}

		public async Task<IEnumerable<ProfissionaisDTO>> GetProfissionaisList()
		{
			var Profissionaiss = await _profissionaisRepository.GetProfissionaisList();

			var retorno = _mapper.Map<IEnumerable<ProfissionaisDTO>>(Profissionaiss);

			return retorno;

		}
		 
		public async Task<ProfissionaisDTO> GetProfissionaisById(int id)
		{
			var Profissionais = await _profissionaisRepository.GetProfissionaisById(id);

			var retorno = _mapper.Map<ProfissionaisDTO>(Profissionais);

			return retorno;

		}
	}
}
