using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
//using Newtonsoft.Json;
using System.Collections.Specialized;

//using Authentication;
using Database.Models;
using Logging;

namespace Database.Access
{
    public class DatabaseAccess
    {

        ///// <summary>Gets all the rows of the 'Stations' table.</summary>
        ///// <param name="connection">The SqlConnection object used to access the database.</param>
        ///// <returns>Returns all the rows of the 'Stations' table</returns>
        public static StationInfo[] GetAllStations(SqlConnection connection)
        {
            List<StationInfo> stationList = new List<StationInfo>();
            if (connection.State == ConnectionState.Open)
            {
                string query = String.Format("SELECT * FROM Stations");
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    StationInfo s = new StationInfo();

                    s.Id = Convert.ToInt32(reader["Id"]);
                    s.Name = Convert.ToString(reader["Name"]);
                    s.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    s.Genre = Convert.ToString(reader["Genre"]);
                    s.NumCurrentClips = Convert.ToInt32(reader["NumCurrentClips"]);
                    s.BPM = Convert.ToInt32(reader["BPM"]);
                    s.Available = Convert.ToBoolean(reader["Available"]);
                    s.SongFilepath = Convert.ToString(reader["SongFilepath"]);
                    s.MaxNumClips = Convert.ToInt32(reader["MaxNumClips"]);
                    s.TimeSignature = Convert.ToInt32(reader["TimeSignature"]);

                    stationList.Add(s);
                }
                reader.Close();

            }else
            {
                Logger.Log("GetAllStations() failed.  ConnectionState not opened");
            }
            return stationList.ToArray();
        }

        ///// <summary>Gets all the rows of the 'Stations' table.</summary>
        ///// <param name="connection">The SqlConnection object used to access the database.</param>
        ///// <param name="stationName">The name of the station that contains the sound clips you want.</param>
        ///// <returns>Returns the 'SoundClipInfo' objects of the sound clips that are associated with the sepcified station.</returns>
        public static SoundClipInfo[] GetStationSoundClips(SqlConnection connection, string stationName)
        {
            SoundClipInfo[] clips = null;
            if (connection.State == ConnectionState.Open)
            {
                String query = "SELECT * FROM SoundClips WHERE Id IN"
                            + "    (SELECT SoundClip_Id FROM StationSoundJunction WHERE Station_Id ="
                            + "        (SELECT Id FROM Stations WHERE Name=@StationName"
                            + "        )"
                            + "    );";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@StationName", SqlDbType.VarChar).Value = stationName;

                StringCollection id = new StringCollection();
                StringCollection name = new StringCollection();
                StringCollection createdBy = new StringCollection();
                StringCollection location = new StringCollection();
                StringCollection category = new StringCollection();
                StringCollection filepath = new StringCollection();
                StringCollection length = new StringCollection();
                StringCollection uploadDate = new StringCollection();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id.Add(Convert.ToString(reader["Id"]));
                    name.Add(Convert.ToString(reader["Name"]));
                    createdBy.Add(Convert.ToString(reader["CreatedById"]));
                    location.Add(Convert.ToString(reader["Location"]));
                    category.Add(Convert.ToString(reader["Category"]));
                    length.Add(Convert.ToString(reader["Length"]));
                    filepath.Add(Convert.ToString(reader["Filepath"]));
                    uploadDate.Add(Convert.ToString(reader["UploadDate"]));
                }
                reader.Close();

                clips = new SoundClipInfo[id.Count];
                for (int i = 0; i < id.Count; i++)
                {
                    clips[i] = new SoundClipInfo();

                    clips[i].Id = Convert.ToInt32(id[i]);
                    clips[i].Name = name[i];
                    clips[i].CreatedById = Convert.ToInt32(createdBy[i]);
                    clips[i].Location = location[i];
                    clips[i].Category = category[i];
                    clips[i].Filepath = filepath[i];
                    clips[i].Length = Convert.ToDecimal(length[i]);
                    clips[i].UploadDate = Convert.ToDateTime(uploadDate[i]);
                    clips[i].Error = String.Empty;
                }
            }else
            {
                Logger.Log("GetStationSoundClips() failed. Connection not open.");
            }
            return clips;
        }

        public static double[] GetBeatPattern(SqlConnection connection, StationInfo station)
        {
            string pattern = String.Empty;
            if (connection.State == ConnectionState.Open)
            {
                string query = "select BeatPattern From Stations Where Id=" + station.Id;
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    pattern = (string)reader["BeatPattern"];
                }
            }else
            {
                Logger.Log("GetBeatPattern() failed. Connection not open");
            }
            return ParseBeatPatternFromDB(pattern, station.BPM);
        }

        private static double[] ParseBeatPatternFromDB(string pattern, int bpm)
        {
            string[] tokens = pattern.Split(new char[] { ',' });
            List<double> beatPattern = new List<double>();

            beatPattern.Add(Convert.ToDouble(tokens[0]));
            beatPattern.Add(bpm);

            for (int i = 1; i < tokens.Length; i++)
            {
                beatPattern.Add(Convert.ToDouble(tokens[i]));
            }
            return beatPattern.ToArray();
        }

        public static StationInfo GetStation(SqlConnection connection, int stationId)
        {
            StationInfo station = new StationInfo();
            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    string query = "select * from Stations where Id=@StationId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@StationId", SqlDbType.Int).Value = stationId;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        station.Id = Convert.ToInt32(reader["Id"]);
                        station.Name = Convert.ToString(reader["Name"]);
                        station.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                        station.Genre = Convert.ToString(reader["Genre"]);
                        station.NumCurrentClips = Convert.ToInt32(reader["NumCurrentClips"]);
                        station.BPM = Convert.ToInt32(reader["BPM"]);
                        station.Available = Convert.ToBoolean(reader["Available"]);
                        station.SongFilepath = Convert.ToString(reader["SongFilepath"]);
                    }
                }
                catch(SqlException e)
                {
                    Debug.WriteLine(e.Message);
                    Logger.Log(e.Message);
                }
            }
            return station;
        }

        public static void UpdateSongAvailability(int stationId, bool availability)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand("update Stations set Available=@Available where Id=@Id", sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Available", SqlDbType.Bit).Value = availability;
                    sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = stationId;
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public static void RemoveSoundClipFromStation(int stationId, int soundClipId)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            try
            {
                connection.Open();
                string query = "DELETE FROM StationSoundJunction WHERE Station_Id=@StationId AND SoundClip_Id=@SoundClipId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@StationId", SqlDbType.Int).Value = stationId;
                command.Parameters.Add("@SoundClipId", SqlDbType.Int).Value = soundClipId;
                command.ExecuteNonQuery();

                DecrementStationNumClip(connection, stationId);

            }
            catch (SqlException e)
            {
                using (StreamWriter stream = File.AppendText("/musicgroup/log.txt"))
                {
                    stream.WriteLine(DateTime.Now.ToString("mm/dd/yyyy") + ": " + e.Message);
                }
            }
        }

        public static void DecrementStationNumClip(SqlConnection connection, int stationId)
        {
            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    string query = "UPDATE Stations SET NumCurrentClips=NumCurrentClips-1 WHERE Id=@Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = stationId;
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Debug.WriteLine(e.Message);
                }

            }
        }

        static void Main() { }
    }

}

	
