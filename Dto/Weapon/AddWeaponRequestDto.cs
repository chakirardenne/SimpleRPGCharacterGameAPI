namespace Tutorial_DotNet.Dto.Weapon;

public class AddWeaponRequestDto
{
    public string Name { get; set; }
    public int Damage { get; set; }
    public int CharacterId { get; set; }
}