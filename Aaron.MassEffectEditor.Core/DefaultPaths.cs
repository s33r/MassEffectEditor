using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Core
{
    internal static class DefaultPaths
    {
        public static string SteamLegendaryEdition { get; } = @"C:\Program Files (x86)\Steam\steamapps\common\Mass Effect Legendary Edition\Game\";
        public static string WorkingDirectory { get; } = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "aaron");
    }
}
