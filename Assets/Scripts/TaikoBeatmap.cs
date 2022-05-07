using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace TaikoFlip
{
    public class TaikoBeatmap
    {
        public static NumberFormatInfo NumberFormatInfo;
        public AudioClip Music { get; }
        
        public Sprite Background { get; private set; }
        
        public List<TaikoTimingPoint> TimingPoints { get; } = new List<TaikoTimingPoint>();

        public List<TaikoObject> Objects { get; } = new List<TaikoObject>();
        
        public SampleSet SampleSet { get; }
        
        /// <summary>
        /// Milliseconds of silence before the audio starts playing
        /// </summary>
        public float AudioLeadIn { get; }
        
        public string Title { get; }
        public string Artist { get; }
        public string Creator { get; }
        public string Version { get; }
        public string Source { get; }
        public string Tags { get; }

        static TaikoBeatmap()
        {
            NumberFormatInfo = new NumberFormatInfo
            {
                NegativeSign = "-",
                NumberDecimalSeparator = ".",
            };
        }

        public TaikoBeatmap(TextAsset beatmap)
        {
            string[] lines = beatmap.text.Split('\n');
            BeatmapSection currentSection = BeatmapSection.None;
            foreach (string line in lines)
            {
                switch (line)
                {
                    case "[General]\r":
                        currentSection = BeatmapSection.General;
                        continue;
                    case "[Metadata]\r":
                        currentSection = BeatmapSection.Metadata;
                        continue;
                    case "[TimingPoints]\r":
                        currentSection = BeatmapSection.TimingPoints;
                        continue;
                    case "[HitObjects]\r":
                        currentSection = BeatmapSection.HitObjects;
                        continue;
                    case "[Colours]\r":
                        currentSection = BeatmapSection.Colours;
                        continue;
                }

                switch (currentSection)
                {
                    case BeatmapSection.General when line.StartsWith("AudioFilename"):
                        Music = Resources.Load<AudioClip>(line
                            .Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim().Replace(".mp3", ""));
                        break;
                    case BeatmapSection.General when line.StartsWith("AudioLeadIn"):
                        AudioLeadIn =
                            float.Parse(line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim(), NumberFormatInfo);
                        break;
                    case BeatmapSection.General:
                    {
                        if (line.StartsWith("SampleSet"))
                        {
                            string set = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();

                            SampleSet = (SampleSet) Enum.Parse(typeof(SampleSet), set);
                        }

                        break;
                    }
                    case BeatmapSection.Metadata when line.StartsWith("Title"):
                        Title = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        break;
                    case BeatmapSection.Metadata when line.StartsWith("Artist"):
                        Artist = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        break;
                    case BeatmapSection.Metadata when line.StartsWith("Creator"):
                        Creator = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        break;
                    case BeatmapSection.Metadata when line.StartsWith("Version"):
                        Version = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        break;
                    case BeatmapSection.Metadata when line.StartsWith("Source"):
                        Source = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        break;
                    case BeatmapSection.Metadata:
                    {
                        if (line.StartsWith("Tags"))
                        {
                            Tags = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        }

                        break;
                    }
                    case BeatmapSection.TimingPoints:
                        if(line.Length >5)
                            TimingPoints.Add(new TaikoTimingPoint(line));
                        break;
                    case BeatmapSection.HitObjects:
                    {
                        if (line.Length == 0)
                            continue;
                        
                        int type = int.Parse(line.Split(',')[3]);

                        switch (type)
                        {
                            case 0:
                                Objects.Add(new TaikoNote(line));
                                break;
                            case 1:
                                //Objects.Add(new TaikoSlider(line));
                                break;
                            case 3:
                                //Objects.Add(new TaikoSpinner(line));
                                break;
                        }

                        break;
                    }
                }
            }
        }
    }

    public enum SampleSet
    {
        Normal,
        Soft,
        Drum,
        None,
    }

    internal enum BeatmapSection
    {
        None,
        General,
        Editor,
        Metadata,
        Difficulty,
        Events,
        TimingPoints,
        Colours,
        HitObjects,
    }
}