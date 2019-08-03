using System.ComponentModel;

#pragma warning disable 1591
namespace Azuria.Enums.Info
{
    public enum AdaptionType
    {
        [Description("entry")] Entry,

        [Description("vn")] VisualNovel,

        [Description("ln")] LightNovel,

        [Description("original")] Original,

        [Description("games")] Games,

        [Description("misc")] Misc
    }
}