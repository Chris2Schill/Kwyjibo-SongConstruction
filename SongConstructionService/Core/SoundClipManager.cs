using System;
using System.Collections.Generic;
using Database.Models;
using System.Configuration;
using System.Data.SqlClient;
using Logging;

namespace SongConstructionService
{
    // This class listens for changes from the StationSoundJunction
    // table in the db and delegates out method calls to stations.
    class SoundClipManager
    {

        public static Dictionary<int, SoundClipInfo> SoundClips;
        public int MaxSoundClipId;

        public SoundClipManager()
        {
            MaxSoundClipId = 0;
            SoundClips = new Dictionary<int, SoundClipInfo>();

            try
            {
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand("select * from SoundClips", sqlConnection))
                    {
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

                                SoundClips[row.Id] = row;
                                if (row.Id > MaxSoundClipId) { MaxSoundClipId = row.Id; }
                            }
                        }
                    }
                }
            }catch(Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        public void OnSoundClipsTableChanged(SoundClipInfo clip)
        {
            SoundClips.Add(clip.Id, clip);
            if (clip.Id > MaxSoundClipId)
            {
                MaxSoundClipId = clip.Id;
            }
        }

    }


}
