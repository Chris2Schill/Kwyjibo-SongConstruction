using System;
using System.Collections.Generic;
using Database.Access;
using Database.Models;
using System.Threading;
using System.Data.SqlClient;
using BeatGeneration;
using SongConstruction;
using Logging;

namespace SongConstructionService
{
    public class Station
    {
        private Queue<SoundClipInfo> ClipQueue { get; set; }
        public ModuloArray<SoundClipInfo> CurrentClips { get; set; }
        public StationInfo Info { get; set; }

        private BeatGenerator beatGenerator { get; set; }
        private double[] beatPattern { get; set; }

        private Station() { } // hidden so we are garunteed StationInfo 

        public Station(StationInfo info)
        {
            Info = info;
            CurrentClips = new ModuloArray<SoundClipInfo>(info.MaxNumClips);
            ClipQueue = new Queue<SoundClipInfo>();
            

            // Put all the initial clips into the currentSounds Array and the rest into the queue 
            List<SoundClipInfo> clips = StationManager.GetStationClips(Info.Id);
            foreach (SoundClipInfo clip in clips)
            {
                if (CurrentClips.Count < Info.MaxNumClips)
                {
                    CurrentClips.Add(clip);
                } else
                {
                    ClipQueue.Enqueue(clip);
                }
            }

            beatGenerator = new BeatGenerator();
            beatPattern = beatGenerator.GenerateBeat(this);

            if (CurrentClips.Count > 0)
            {
                SoxUtils.Construct(beatPattern, CurrentClips.Items, Info.SongFilepath);
            }
            Logger.Log("");
        }

        public void OnSoundAdded(int soundClipId)
        {
            Logger.Log("OnSoundAdded(): stationId: " + Info.Id + ", soundClipId: " + soundClipId);
            if (CurrentClips[CurrentClips.Index] != null)
            {
                var oldClip = (SoundClipInfo)CurrentClips[CurrentClips.Index];
                DatabaseAccess.RemoveSoundClipFromStation(Info.Id, oldClip.Id);
            }

            CurrentClips.Add(SoundClipManager.SoundClips[soundClipId]);
            ThreadPool.QueueUserWorkItem(UpdateStation, Info);
        }

        private void UpdateStation(object state)
        {
            StationInfo station = (StationInfo)state;
            while (!station.Available)
            {
                Thread.Sleep(1000);
            }

            DatabaseAccess.UpdateSongAvailability(station.Id, false);
            beatPattern = beatGenerator.GenerateBeat(this);
            SoxUtils.Construct(beatPattern, CurrentClips.Items, station.SongFilepath);
            DatabaseAccess.UpdateSongAvailability(station.Id, true);
        }

        public void OnSoundDeleted(object state)
        {
             
        }


    }
}
