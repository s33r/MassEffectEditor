using Aaron.MassEffectEditor.Coalesced;
using Aaron.MassEffectEditor.Core;
using System.IO;

namespace MassEffectEditor
{
    class Program
    {
        private static string _testInputLocation;
        private static readonly Games _game = Games.Me1;

        static void Main(string[] args)
        {
            

            Configuration.Instance.Initialize();
            _testInputLocation = Path.Join(Configuration.Instance.WorkingLocation, 
                Path.GetFileName(Configuration.Instance.Game[_game].CoalescedConfigurationLocation));


            BackupCoalesced();

            string inputLocation = _testInputLocation;

            Container container = CoalescedFile.Load(_game, inputLocation);
            string name = container.Files[0].FriendlyName;
            
            CoalescedFile.Save(_game, container, inputLocation);


            Compare();

            Container testContainer = CoalescedFile.Load(_game, inputLocation);

        }



        static void BackupCoalesced()
        {            
            string sourceLocation = Configuration.Instance.Game[_game].CoalescedConfigurationLocation;
            string destinationLocation = _testInputLocation;

            File.Delete(destinationLocation);

            File.Copy(sourceLocation, destinationLocation);
        }


        static void RestoreCoalesced()
        {
            string sourceLocation = _testInputLocation;
            string destinationLocation = Configuration.Instance.Game[_game].CoalescedConfigurationLocation;

            File.Copy(sourceLocation, destinationLocation);
        }

        static void Compare()
        {
            byte[] original = File.ReadAllBytes(Configuration.Instance.Game[_game].CoalescedConfigurationLocation);
            byte[] mine = File.ReadAllBytes(_testInputLocation);

            CoalescedFile.Compare(_game, original, mine );
        }
  
    }
}
