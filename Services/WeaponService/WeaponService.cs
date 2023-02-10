using System.Security.Claims;

namespace Tutorial_DotNet.Services;

public class WeaponService : IWeaponService {
    private readonly IMapper _mapper;
    private readonly DatabaseContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public WeaponService(IMapper mapper, DatabaseContext context, IHttpContextAccessor httpContextAccessor) {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<GetCharacterResponseDto>> AddWeapon(AddWeaponRequestDto addWeaponRequestDto) {
        var response = new ServiceResponse<GetCharacterResponseDto>();
        try {
            var characterToEquipWeapon = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == addWeaponRequestDto.CharacterId
                                          && c.User!.Id == int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (characterToEquipWeapon is null) {
                response.Success = false;
                response.Message = "Character not found.";
            }
            var weapon = _mapper.Map<Weapon>(addWeaponRequestDto);
            await _context.Weapons.AddAsync(weapon);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetCharacterResponseDto>(characterToEquipWeapon);
        }
        catch (Exception e) {
            response.Success = false;
            response.Message = e.Message;
        }
        return response;
    }
}