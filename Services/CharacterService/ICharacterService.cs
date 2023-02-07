using Tutorial_DotNet.Dto;

namespace Tutorial_DotNet.Services.CharacterService;


public interface ICharacterService {
    Task<ServiceResponse<List<GetCharacterResponseDto>>> GetAllCharacters();
    Task<ServiceResponse<GetCharacterResponseDto>> GetCharacterById(int id);
    Task<ServiceResponse<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto character);
    Task<ServiceResponse<GetCharacterResponseDto>> UpdateCharacter(UpdateCharacterDtoRequest character);
    Task<ServiceResponse<bool>> DeleteCharacter(int id);
    
}