using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using System.Configuration;
using Database.Access;
using System.Data.SqlClient;

namespace SongConstructionService
{


    // This class listens for changes from the StationSoundJunction
    // table in the db and delegates out method calls to stations.
    class StationManager
    {
        public static Dictionary<int,Station> Stations;
        public static Dictionary<int,StationSoundJunction> clipsToStationsMapping;

        public int MaxStationId;
        public int MaxStationSoundJunctionID;

        public StationManager()
        {
            MaxStationId = 0;
            MaxStationSoundJunctionID = 0;
            Stations = new Dictionary<int, Station>();
            clipsToStationsMapping = new Dictionary<int,StationSoundJunction>();

            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand("", sqlConnection))
                {

                    sqlCommand.CommandText = "select * from StationSoundJunction";
                    using (var dataReader = sqlCommand.ExecuteReader())
                    {

                        while (dataReader.Read())
                        {
                            var row = new StationSoundJunction();
                            row.ID = Convert.ToInt32(dataReader["ID"]);
                            row.Station_Id = Convert.ToInt32(dataReader["Station_Id"]);
                            row.SoundClip_Id = Convert.ToInt32(dataReader["SoundClip_Id"]);

                            clipsToStationsMapping[row.ID] = row;
                            if (row.ID > MaxStationSoundJunctionID) { MaxStationSoundJunctionID = row.ID; }
                        }
                    }

                    sqlCommand.CommandText = "select * from Stations";
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

                            Stations[row.Id] = new Station(row);
                            if (row.Id > MaxStationId) { MaxStationId = row.Id; }
                        }
                    } 

                }
            }
        }

        // Triggered when the StationSoundJunction table changes
        public void OnStationSoundJunctionTableChanged(object state)
        {
            var row = (StationSoundJunction)state;

            clipsToStationsMapping[row.ID] = row;
            
            Stations[row.Station_Id].OnSoundAdded(row.SoundClip_Id);

            if (row.ID > MaxStationSoundJunctionID)
            {
                MaxStationSoundJunctionID = row.ID;
            }
        }

        // Triggered when Stations table changes
        public void OnStationsTableChanged(object state)
        {
            var row = (StationInfo)state;
            Stations[row.Id] = new Station(row);

            if (row.Id > MaxStationId)
            {
                MaxStationId = row.Id;
            }
        }

        public static List<SoundClipInfo> GetStationClips(int stationId)
        {
            var clips = new List<SoundClipInfo>();
            foreach (KeyValuePair<int,StationSoundJunction> clipMapping in clipsToStationsMapping)
            {
                var Station_Id = clipMapping.Value.Station_Id;
                var SoundClip_Id = clipMapping.Value.SoundClip_Id;
                if (Station_Id == stationId)
                {
                    clips.Add(SoundClipManager.SoundClips[SoundClip_Id]);
                }
            }
            return clips;
        }

    }


}
