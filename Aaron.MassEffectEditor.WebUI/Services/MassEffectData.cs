using massEffect = Aaron.MassEffectEditor.Coalesced;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.IO;
using System.Threading.Tasks;
using Aaron.MassEffectEditor.Coalesced.Records;

namespace Aaron.MassEffectEditor.WebUI.Services
{
    public delegate void CurrentContainerChanged(massEffect.Container newContainer);

    public delegate void CurrentFileRecordChanged(FileRecord newFileRecord);

    public class MassEffectData
    {

        public event CurrentContainerChanged OnCurrentContainerChanged;
        public event CurrentFileRecordChanged OnCurrentFileRecordChanged;

        public const long MAX_FILE_SIZE = 8388608;
        public IBrowserFile CurrentFile { get; private set; }

        public massEffect.Container CurrentContainer { get; private set; }


        private FileRecord _currentFileRecord;
        public FileRecord CurrentFileRecord {
            get => _currentFileRecord;
            set
            {
                _currentFileRecord = value;
                OnCurrentFileRecordChanged?.Invoke(CurrentFileRecord);
            }
        }

        public async Task<massEffect.Container> LoadFile(IBrowserFile file)
        {
            CurrentFileRecord = null;
            CurrentFile = file;

            Stream fileStream = file.OpenReadStream(MAX_FILE_SIZE);
            MemoryStream memoryStream = new();

            await fileStream.CopyToAsync(memoryStream);

            byte[] data = memoryStream.ToArray();
            massEffect.Container container = massEffect.CoalescedFile.Load(Core.Games.Me3, data);

            Console.WriteLine(container.Files[0].FriendlyName);

            Console.WriteLine(data.Length);

            CurrentContainer = container;

            OnCurrentContainerChanged?.Invoke(CurrentContainer);
            OnCurrentFileRecordChanged?.Invoke(CurrentFileRecord);

            return CurrentContainer;
        }

        public void UnloadFile()
        {
            CurrentContainer = null;
            CurrentFile = null;
        }


    }
}
