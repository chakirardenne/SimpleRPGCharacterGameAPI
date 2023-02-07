namespace Tutorial_DotNet.Dto.Character;

public class AddCharacterRequestDto {
    public string Name { get; set; }
    public int HitPoints { get; set; }
    public int Strength { get; set; }
    public int Defense{ get; set; }
    public int Intelligence{ get; set; }
    public RpgClass Class { get; set; }
}