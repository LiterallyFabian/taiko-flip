using System.Collections.Generic;

namespace TaikoFlip
{
    public class TaikoNote : TaikoObject
    {
        public NoteType Type { get; } = NoteType.Don;
        public bool IsBig { get; } 
        public List<HitSound> HitSounds { get; } 
        
        public TaikoNote(string line)
        {
            string[] parts = line.Split(',');
            Time = int.Parse(parts[2]);
            
            // Parse sounds
            HitSounds = new List<HitSound>();
            int sound = int.Parse(parts[4]);

            if (sound == 8 || sound == 10 || sound == 12 || sound == 14)
                HitSounds.Add(HitSound.Clap);
            if (sound == 4 || sound == 6 || sound == 12 || sound == 14)
                HitSounds.Add(HitSound.Finish);
            if (sound == 2 || sound == 6 || sound == 10 || sound == 14)
                HitSounds.Add(HitSound.Whistle);
            if (HitSounds.Count == 0)
                HitSounds.Add(HitSound.Normal);

            if (HitSounds.Contains(HitSound.Whistle) || HitSounds.Contains(HitSound.Clap))
            {
                Type = NoteType.Ka;
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
        Ka,
    }

    public enum HitSound
    {
        Normal = 0,
        Whistle = 1, 
        Finish = 2,
        Clap = 3,
    }
}