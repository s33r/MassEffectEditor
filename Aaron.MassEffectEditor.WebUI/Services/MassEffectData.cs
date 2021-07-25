using massEffect = Aaron.MassEffectEditor.Coalesced;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.WebUI.Services
{
    public class MassEffectData
    {
        public const long MAX_FILE_SIZE = 8388608;
        public IBrowserFile CurrentFile { get; private set; }

        public massEffect.Container CurrentContainer { get; private set; }

        public async Task<massEffect.Container> LoadFile(IBrowserFile file)
        {

            CurrentFile = file;

            Stream fileStream = file.OpenReadStream(MAX_FILE_SIZE);
            MemoryStream memoryStream = new();

            await fileStream.CopyToAsync(memoryStream);

            byte[] data = memoryStream.ToArray();
            massEffect.Container container = massEffect.CoalescedFile.Load(Core.Games.Me3, data);

            Console.WriteLine(container.Files[0].FriendlyName);

            Console.WriteLine(data.Length);

            CurrentContainer = container;
            return CurrentContainer;
        }

        public void UnloadFile()
        {
            CurrentContainer = null;
            CurrentFile = null;
        }


    }
}
