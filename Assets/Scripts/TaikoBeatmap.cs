using System;
using System.Collections.Generic;
using UnityEngine;

namespace TaikoFlip
{
    public class TaikoBeatmap
    {
        public AudioClip Music { get; private set; }
        
        public Sprite Background { get; private set; }
        
        public List<TaikoTimingPoint> TimingPoints { get; private set; }

        public List<TaikoObject> Objects { get; private set; }
        
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

        public TaikoBeatmap(TextAsset beatmap)
        {
            string[] lines = beatmap.text.Split('\n');
            BeatmapSection currentSection = BeatmapSection.None;
            foreach (string line in lines)
            {
                switch (line)
                {
                    case "[General]":
                        currentSection = BeatmapSection.General;
                        continue;
                    case "[Metadata]":
                        currentSection = BeatmapSection.Metadata;
                        continue;
                    case "[TimingPoints]":
                        currentSection = BeatmapSection.TimingPoints;
                        continue;
                    case "[HitObjects]":
                        currentSection = BeatmapSection.HitObjects;
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
                            float.Parse(line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1).Trim());
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
                        TimingPoints.Add(new TaikoTimingPoint(line));
                        break;
                    case BeatmapSection.HitObjects:
                    {
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
        HitObjects,
    }
}