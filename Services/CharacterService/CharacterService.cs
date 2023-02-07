using Tutorial_DotNet.Dto;

namespace Tutorial_DotNet.Services.CharacterService;
public class CharacterService : ICharacterService {
    private readonly IMapper _mapper;
    public CharacterService(IMapper mapper) {
        _mapper = mapper;
    }

    private static  List<Character> Characters = new List<Character>() {
        new Character(),
        new Character{Id = 1, Name = "Sam"}
    };
    
    public async Task<ServiceResponse<List<GetCharacterResponseDto>>> GetAllCharacters() {
        var rtr = Characters.Select(x => _mapper.Map<GetCharacterResponseDto>(x)).ToList();
        return new ServiceResponse<List<GetCharacterResponseDto>> { Data = rtr};
    }

    public async Task<ServiceResponse<GetCharacterResponseDto>> GetCharacterById(int id) {
        var rtr = Characters.FirstOrDefault(character => character.Id == id);
        if (rtr is null)
            throw new Exception("Character not found");
        return new ServiceResponse<GetCharacterResponseDto> { Data = _mapper.Map<GetCharacterResponseDto>(rtr) };
    }

    public async Task<ServiceResponse<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto character) {
        Character characterToAdd = _mapper.Map<Character>(character);
        characterToAdd.Id = Characters.Max(c => c.Id) + 1;
        Characters.Add(characterToAdd);
        var rtr = Characters.Select(x => _mapper.Map<GetCharacterResponseDto>(x)).ToList();
        return  new ServiceResponse<List<GetCharacterResponseDto>> {Data = rtr};
    }

    public async Task<ServiceResponse<GetCharacterResponseDto>> UpdateCharacter(UpdateCharacterDtoRequest characterDto) {
        var serviceResponse = new ServiceResponse<GetCharacterResponseDto>();
        try {
            var characterToUpdate = Characters.FirstOrDefault(character => character.Id == characterDto.Id);
            if (characterToUpdate is null)
                throw new Exception($"Character with ID '{characterDto.Id}'not found");
            _mapper.Map(characterDto, characterToUpdate);
            serviceResponse.Data = _mapper.Map<GetCharacterResponseDto>(characterToUpdate);
        }
        catch (Exception e) {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<bool>> DeleteCharacter(int id) {
        var rtr = new ServiceResponse<bool>();
        try {
            var characterToDelete = Characters.FirstOrDefault(c => c.Id == id);
            if (characterToDelete is null)
                throw new Exception($"Character with ID '{id}'not found");
            var result = Characters.Remove(characterToDelete);
            rtr.Data = result;
        }
        catch (Exception e) {
            rtr.Success = false;
            rtr.Message = e.Message;
        }
        return rtr;
    }
}