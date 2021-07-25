using System;
using System.Runtime.Serialization;

namespace Aaron.MassEffectEditor.Core.Exceptions
{
    public class GameNotSupportedException : Exception
    {
        public Games Game { get; set; }

        public GameNotSupportedException(Games game)
            : base($"This method does not support the game: {game}")
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