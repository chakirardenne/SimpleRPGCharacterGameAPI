namespace Tutorial_DotNet.Services.CharacterService;

public class CharacterService : ICharacterService {
    private readonly IMapper _mapper;
    private readonly DatabaseContext _context;
    public CharacterService(IMapper mapper, DatabaseContext context) {
        _mapper = mapper;
        _context = context;
    }
    
    public async Task<ServiceResponse<List<GetCharacterResponseDto>>> GetAllCharacters() {
        var rtr = await _context.Characters.Select(x => _mapper.Map<GetCharacterResponseDto>(x)).ToListAsync();
        return new ServiceResponse<List<GetCharacterResponseDto>> { Data = rtr};
    }

    public async Task<ServiceResponse<GetCharacterResponseDto>> GetCharacterById(int id) {
        var rtr = await _context.Characters.FirstOrDefaultAsync(character => character.Id == id);
        if (rtr is null)
            throw new Exception("Character not found");
        return new ServiceResponse<GetCharacterResponseDto> { Data = _mapper.Map<GetCharacterResponseDto>(rtr) };
    }

    public async Task<ServiceResponse<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto character) {
        var characterToAdd = _mapper.Map<Character>(character);
        _context.Characters.Add(characterToAdd);
        await _context.SaveChangesAsync();
        var rtr = _context.Characters.Select(x => _mapper.Map<GetCharacterResponseDto>(x)).ToList();
        return  new ServiceResponse<List<GetCharacterResponseDto>> { Data = rtr };
    }

    public async Task<ServiceResponse<GetCharacterResponseDto>> UpdateCharacter(UpdateCharacterDtoRequest characterDto) {
        var serviceResponse = new ServiceResponse<GetCharacterResponseDto>();
        try {
            var characterToUpdate = await _context.Characters.FirstOrDefaultAsync(character => character.Id == characterDto.Id);
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
            var characterToDelete = _context.Characters.FirstOrDefault(c => c.Id == id);
            if (characterToDelete is null)
                throw new Exception($"Character with ID '{id}'not found");
            _context.Characters.Remove(characterToDelete);
            await _context.SaveChangesAsync();
            rtr.Data = true;
        }
        catch (Exception e) {
            rtr.Success = false;
            rtr.Message = e.Message;
        }
        return rtr;
    }
}