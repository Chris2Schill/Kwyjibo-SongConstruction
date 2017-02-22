using System;

namespace Database.Models
{
    // These must be classes (rather than structs) so we can watch it with SqlTableDependency

    public class StationInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string Genre { get; set; }
        public int NumCurrentClips { get; set; }
        public int BPM { get; set ; }
        public int TimeSignature { get; set; }
        public bool Available { get; set; }
        public string SongFilepath { get; set; }
        public int MaxNumClips { get; set; }
    }

    public class SoundClipInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatedById { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Filepath { get; set; }
        public decimal Length { get; set; }
        public DateTime UploadDate { get; set; }
        public string Error { get; set; }
    }

    public class SessionInfo
    {
        public string USER_ID;
        public string USER_NAME;
        public string USER_EMAIL;
        public string AUTH_TOKEN;
        public bool IS_AUTHENTICATED;
        public string ERROR;
    }

    public class StationSoundJunction
    {
        public int ID { get; set; }
        public int Station_Id { get; set; }
        public int SoundClip_Id { get; set; }
    }
}
