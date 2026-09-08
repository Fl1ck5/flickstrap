namespace Flickstrap.Enums.FlagPresets
{
    public enum MSAAMode
    {
        [EnumName(FromTranslation = "Common.Automatic")]
        Default,
        [EnumName(StaticName = "Off (0x)")]
        x0,
        [EnumName(StaticName = "1x")]
        x1,
        [EnumName(StaticName = "2x")]
        x2,
        [EnumName(StaticName = "4x")]
        x4
    }
}
