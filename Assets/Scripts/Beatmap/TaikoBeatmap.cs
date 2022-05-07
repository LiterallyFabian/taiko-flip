using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TaikoFlip
{
    public class TaikoBeatmap
    {
        public static NumberFormatInfo NumberFormatInfo;
        public AudioClip Music { get; private set; }

        public Sprite Background { get; private set; }

        public List<TaikoTimingPoint> TimingPoints { get; } = new List<TaikoTimingPoint>();

        public List<TaikoObject> Objects { get; } = new List<TaikoObject>();

        public SampleSet SampleSet { get; private set; }

        /// <summary>
        /// Milliseconds of silence before the audio starts playing
        /// </summary>
        public float AudioLeadIn { get; private set; }

        public string Title { get; private set; }
        public string Artist { get; private set; }
        public string Creator { get; private set; }
        public string Version { get; private set; }
        public string Source { get; private set; }
        public string Tags { get; private set; }

        static TaikoBeatmap()
        {
            NumberFormatInfo = new NumberFormatInfo
            {
                NegativeSign = "-",
                NumberDecimalSeparator = ".",
            };
        }

        private TaikoBeatmap() {}

        
        public static TaikoBeatmap Parse(string path)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            if (textAsset == null)
                throw new Exception($"Beatmap file not found: {path}");
            
            TaikoBeatmap beatmap = new TaikoBeatmap();
            string[] lines = textAsset.text.Split('\n');
            string dirPath = path.Substring(0, path.LastIndexOf('\\') + 1);
            
            
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
                    case "[Events]\r":
                        currentSection = BeatmapSection.Events;
                        continue;
                }

                switch (currentSection)
                {
                    case BeatmapSection.General when line.StartsWith("AudioFilename"):
                        // file name without extension and \r
                        string fileName = line.Substring(line.IndexOf(':') + 1).Replace(".mp3\r", "").Trim();
                        beatmap.Music = Resources.Load<AudioClip>(dirPath + fileName);
                        break;
                    case BeatmapSection.General when line.StartsWith("AudioLeadIn"):
                        beatmap.AudioLeadIn =
                            float.Parse(line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim(),
                                NumberFormatInfo);
                        break;
                    case BeatmapSection.General:
                    {
                        if (line.StartsWith("SampleSet"))
                        {
                            string set = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();

                            beatmap.SampleSet = (SampleSet) Enum.Parse(typeof(SampleSet), set);
                        }

                        break;
                    }
                    case BeatmapSection.Metadata when line.StartsWith("Title"):
                        beatmap.Title = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        break;
                    case BeatmapSection.Metadata when line.StartsWith("Artist"):
                        beatmap.Artist = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        break;
                    case BeatmapSection.Metadata when line.StartsWith("Creator"):
                        beatmap.Creator = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        break;
                    case BeatmapSection.Metadata when line.StartsWith("Version"):
                        beatmap.Version = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        break;
                    case BeatmapSection.Metadata when line.StartsWith("Source"):
                        beatmap.Source = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        break;
                    case BeatmapSection.Metadata when line.StartsWith("Tags"):
                        beatmap.Tags = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        break;
                    case BeatmapSection.Events:
                        if (line.StartsWith("0,0,"))
                        {
                            string backgroundName = line.Substring(line.IndexOf('"') + 1, line.LastIndexOf('"') - line.IndexOf('"') - 1);
                            backgroundName = backgroundName.Substring(0, backgroundName.LastIndexOf('.'));
                            beatmap.Background = Resources.Load<Sprite>(dirPath + backgroundName);
                        }

                        break;
                    case BeatmapSection.TimingPoints:
                        if (line.Length > 5)
                            beatmap.TimingPoints.Add(new TaikoTimingPoint(line));
                        break;
                    
                    case BeatmapSection.HitObjects:
                    {
                        if (line.Length == 0)
                            continue;

                        int type = int.Parse(line.Split(',')[3]);

                        switch (type)
                        {
                            case 1:
                                beatmap.Objects.Add(new TaikoNote(line));
                                break;
                            case 6:
                                //map.Objects.Add(new TaikoSlider(line));
                                break;
                            case 12:
                                //map.Objects.Add(new TaikoSpinner(line));
                                break;
                        }

                        break;
                    }
                }
            }
            
            return beatmap;
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