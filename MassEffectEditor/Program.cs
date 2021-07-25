using Aaron.MassEffectEditor.Coalesced;
using Aaron.MassEffectEditor.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace MassEffectEditor
{
    class Program
    {
        private static string testInputLocation;
        private static string gibbedLocation;

        static void Main(string[] args)
        {
            Configuration.Instance.Initialize();
            testInputLocation = Path.Join(Configuration.Instance.WorkingLocation, Path.GetFileName(Configuration.Instance.Game[Games.Me3].CoalescedConfigurationLocation));
            gibbedLocation = Path.Join(Configuration.Instance.WorkingLocation, "Coalesced.bin.gib");

            BackupCoalesced();

            string inputLocation = testInputLocation;

            Container container = CoalescedFile.Load(Games.Me3, inputLocation);
            string name = container.Files[0].FriendlyName;
            
            CoalescedFile.Save(Games.Me3, container, inputLocation);


            Compare();

            Container testContainer = CoalescedFile.Load(Games.Me3, inputLocation);

        }



        static void BackupCoalesced()
        {            
            string sourceLocation = Configuration.Instance.Game[Games.Me3].CoalescedConfigurationLocation;
            string destinationLocation = testInputLocation;

            File.Delete(destinationLocation);

            File.Copy(sourceLocation, destinationLocation);
        }


        static void RestoreCoalesced()
        {
            string sourceLocation = testInputLocation;
            string destinationLocation = Configuration.Instance.Game[Games.Me3].CoalescedConfigurationLocation;

            File.Copy(sourceLocation, destinationLocation);
        }

        static void Compare()
        {
            byte[] original = File.ReadAllBytes(Configuration.Instance.Game[Games.Me3].CoalescedConfigurationLocation);
            byte[] mine = File.ReadAllBytes(testInputLocation);
            byte[] gibbed = File.ReadAllBytes(gibbedLocation);

            CoalescedFile.Compare(original, mine, gibbed);
        }
  
    }
}
