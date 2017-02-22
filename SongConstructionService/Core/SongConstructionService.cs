using System.ServiceProcess;
using System.Threading;
using Logging;
using System.Data.SqlClient;
using System.Configuration;
using Database.Models;
using System;
using System.Data;

namespace SongConstructionService
{
    public partial class SongConstructionService : ServiceBase
    {
        private ManualResetEvent shutdownEvent = new ManualResetEvent(false);
        private Thread parentThread;

        public SongConstructionService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.Log("OnStart()");
            parentThread = new Thread(InitStations);
            parentThread.Start();
        }


        private void InitStations()
        {
            Logger.Log("InitStations()");
            var soundClipManager = new SoundClipManager();
            var stationManager = new StationManager();

            //while (!shutdownEvent.WaitOne(0))
            while(true)
            {
                try
                {
                    LoadCollectionData(ref stationManager, ref soundClipManager);
                    Thread.Sleep(1000);
                }catch(Exception e)
                {
                    Logger.Log(e.Message);
                }
            }
        }


        private void LoadCollectionData(ref StationManager stationManager, ref SoundClipManager soundClipManager)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand("", sqlConnection))
                {
                    // Get Stations
                    sqlCommand.CommandText = "select * from Stations where Id > @StationId";
                    sqlCommand.Parameters.Add("@StationId", SqlDbType.Int).Value = stationManager.MaxStationId;
                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var row = new StationInfo();
                            row.Id = Convert.ToInt32(dataReader["Id"]);
                            row.Name = Convert.ToString(dataReader["Name"]);
                            row.CreatedBy = Convert.ToString(dataReader["CreatedBy"]);
                            row.Genre = Convert.ToString(dataReader["Genre"]);
                            row.NumCurrentClips = Convert.ToInt32(dataReader["NumCurrentClips"]);
                            row.MaxNumClips = Convert.ToInt32(dataReader["MaxNumClips"]);
                            row.BPM = Convert.ToInt32(dataReader["BPM"]);
                            row.Available = Convert.ToBoolean(dataReader["Available"]);
                            row.TimeSignature = Convert.ToInt32(dataReader["TimeSignature"]);
                            row.SongFilepath = Convert.ToString(dataReader["SongFilepath"]);

                            stationManager.OnStationsTableChanged(row);
                        }
                    }

                    // Get SoundClips
                    sqlCommand.CommandText = "select * from SoundClips where Id > @SoundClipId";
                    sqlCommand.Parameters.Add("@SoundClipId", SqlDbType.Int).Value = soundClipManager.MaxSoundClipId;
                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var row = new SoundClipInfo();
                            row.Id = Convert.ToInt32(dataReader["Id"]);
                            row.Name = Convert.ToString(dataReader["Name"]);
                            row.CreatedById = Convert.ToInt32(dataReader["CreatedById"]);
                            row.Location = Convert.ToString(dataReader["Location"]);
                            row.Category = Convert.ToString(dataReader["Category"]);
                            row.Filepath = Convert.ToString(dataReader["Filepath"]);
                            row.Length = Convert.ToDecimal(dataReader["Length"]);
                            row.UploadDate = Convert.ToDateTime(dataReader["UploadDate"]);
                            row.Error = string.Empty;

                            soundClipManager.OnSoundClipsTableChanged(row);
                        }
                    }

                    // Get StationSoundJunction
                    sqlCommand.CommandText = "select * from StationSoundJunction where ID > @JunctionID";
                    sqlCommand.Parameters.Add("@JunctionID", SqlDbType.Int).Value = stationManager.MaxStationSoundJunctionID;
                    using (var dataReader = sqlCommand.ExecuteReader())
                    {

                        while (dataReader.Read())
                        {
                            var row = new StationSoundJunction();
                            row.ID = Convert.ToInt32(dataReader["ID"]);
                            row.Station_Id = Convert.ToInt32(dataReader["Station_Id"]);
                            row.SoundClip_Id = Convert.ToInt32(dataReader["SoundClip_Id"]);

                            stationManager.OnStationSoundJunctionTableChanged(row);
                        }
                    }
                }
            }
        }

        protected override void OnStop()
        {
            Logger.Log("OnStop()");
            //shutdownEvent.Set();
            //if (!parentThread.Join(3000))
            //{
            //    parentThread.Abort();
            //}
        }
    }
}
