using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Core.Exceptions
{
    public class GameNotSupportedException : Exception
    {
        public Games Game { get; set; }

        public GameNotSupportedException(Games game)
            :base(string.Format("This method does not support the game: {0}", game))
        {
            Game = game;
        }

        public GameNotSupportedException(Games game, string message) 
            : base(message)
        {
            Game = game;
        }

        public GameNotSupportedException(Games game, string message, Exception innerException) 
            : base(message, innerException)

        {
            Game = game;
        }

        protected GameNotSupportedException(Games game, SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            Game = game;
        }
    }
}
