namespace Flickstrap.Enums
{
    public enum CleanerFrequency
    {
        [EnumName(StaticName = "Never (manual only)")]
        Never,
        [EnumName(StaticName = "Every day")]
        Daily,
        [EnumName(StaticName = "Every week")]
        Weekly,
        [EnumName(StaticName = "Every month")]
        Monthly
    }
}
