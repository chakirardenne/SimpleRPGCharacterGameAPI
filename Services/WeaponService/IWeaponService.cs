
namespace Tutorial_DotNet.Services;

public interface IWeaponService {
    Task<ServiceResponse<GetCharacterResponseDto>> AddWeapon(AddWeaponRequestDto addWeaponRequestDto);
}