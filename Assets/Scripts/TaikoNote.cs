using System.Collections.Generic;

namespace TaikoFlip
{
    public class TaikoNote : TaikoObject
    {
        public NoteType Type { get; } = NoteType.Don;
        private bool IsBig { get; } 
        public List<HitSound> HitSounds { get; } 
        
        public TaikoNote(string line)
        {
            string[] parts = line.Split(',');
            Time = int.Parse(parts[0]);
            
            // Parse sounds
            HitSounds = new List<HitSound>();
            int sound = int.Parse(parts[4]);

            if ((sound & 1) == 1)
            {
                HitSounds.Add(HitSound.Whistle);
            }
            if ((sound & 2) == 2)
            {
                HitSounds.Add(HitSound.Finish);
            }
            if ((sound & 3) == 3)
            {
                HitSounds.Add(HitSound.Clap);
            }
            if (HitSounds.Count == 0)
            {
                HitSounds.Add(HitSound.Normal);
            }
            
            if (HitSounds.Contains(HitSound.Whistle) || HitSounds.Contains(HitSound.Clap))
            {
                Type = NoteType.Kat;
            }

            IsBig = HitSounds.Contains(HitSound.Finish);
        }
    }

    public enum NoteType
    {
        /// <summary>
        /// "Red", for us non-taiko players
        /// </summary>
        Don,

        /// <summary>
        /// A.k.a. "Blue"
        /// </summary>
        Kat,
    }

    public enum HitSound
    {
        Normal = 0,
        Whistle = 1, 
        Finish = 2,
        Clap = 3,
    }
}