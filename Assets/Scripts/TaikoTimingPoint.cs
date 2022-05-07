namespace TaikoFlip
{
    public class TaikoTimingPoint
    {
        public int Time { get; }
        
        public float BeatLength { get; }
        
        public int Meter { get; }
        
        public SampleSet SampleSet { get; }
        
        public int Volume { get; }
        
        public bool Uninherited { get; }
        
        public bool Kiai { get; }

        public TaikoTimingPoint(string line)
        {
            string[] parts = line.Split(',');
            Time = int.Parse(parts[0]);
            BeatLength = float.Parse(parts[1]);
            Meter = int.Parse(parts[2]);
            SampleSet = (SampleSet)int.Parse(parts[3]);
            Volume = int.Parse(parts[4]);
            Uninherited = parts[5] == "1";
            Kiai = parts[6] == "1";
        }
    }
}