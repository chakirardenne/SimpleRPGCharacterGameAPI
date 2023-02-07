using Tutorial_DotNet.Dto;

namespace Tutorial_DotNet;

public class AutoMapperProfiles : Profile{
    public AutoMapperProfiles() { 
        CreateMap<Character, GetCharacterResponseDto>();
        CreateMap<AddCharacterRequestDto, Character>();
        CreateMap<UpdateCharacterDtoRequest, Character>();
    }
}