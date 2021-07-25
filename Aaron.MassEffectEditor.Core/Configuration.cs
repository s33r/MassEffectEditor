using System;
using System.Collections.Generic;
using System.IO;

namespace Aaron.MassEffectEditor.Core
{
    public sealed class Configuration
    {

        #region Singleton
        private static readonly Configuration _instance = new();

        static Configuration() { }

        private Configuration() { }

        // ReSharper disable once ConvertToAutoProperty
        public static Configuration Instance => _instance;

        #endregion

        public string GameBaseLocation { get; set; }
        public string WorkingLocation { get; set; }

        public Dictionary<Games, GameConfiguration> Game { get; } = new();

        public void Initialize()
        {
            if(!BitConverter.IsLittleEndian)
            {
                Console.WriteLine("The processor is not little endian... things are going to break!"); // TODO: Handle big endian
            }


            GameBaseLocation = DefaultPaths.SteamLegendaryEdition;
            WorkingLocation = DefaultPaths.WorkingDirectory;


            Game.Add(Games.Me1, new GameConfiguration
            {
                Name = "Mass Effect 1",
                CoalescedConfigurationLocation = Path.Join(GameBaseLocation, @"ME1\BioGame\CookedPCConsole", "Coalesced_INT.bin")
            });

            Game.Add(Games.Me2, new GameConfiguration
            {
                Name = "Mass Effect 2",
                CoalescedConfigurationLocation = Path.Join(GameBaseLocation, @"ME2\BioGame\CookedPCConsole", "Coalesced_INT.bin")
            });

            Game.Add(Games.Me3, new GameConfiguration
            {
                Name = "Mass Effect 3",
                CoalescedConfigurationLocation = Path.Join(GameBaseLocation, @"ME3\BioGame\CookedPCConsole", "Coalesced.bin")
            });


            Directory.CreateDirectory(WorkingLocation);
        }

   
    }
}
