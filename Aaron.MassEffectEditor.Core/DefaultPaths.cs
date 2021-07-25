using System;
using System.IO;

namespace Aaron.MassEffectEditor.Core
{
    internal static class DefaultPaths
    {
        // ReSharper disable once StringLiteralTypo
        public static string SteamLegendaryEdition { get; } =
            @"C:\Program Files (x86)\Steam\steamapps\common\Mass Effect Legendary Edition\Game\";

        public static string WorkingDirectory { get; } =
            Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "aaron");
    }
}