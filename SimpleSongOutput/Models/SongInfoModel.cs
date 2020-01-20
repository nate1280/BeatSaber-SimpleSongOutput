using SimpleSongOutput.Misc;

namespace SimpleSongOutput.Models
{
    public class SongInfoModel
    {
        public string SongName { get; set; }
        public string SongSubName { get; set; }
        public string SongAuthorName { get; set; }
        public string LevelAuthorName { get; set; }
        public BeatmapDifficulty Difficulty { get; set; }
        public string Base64Thumbnail { get; set; }
        public float SongBPM { get; set; }
        public float NoteJumpSpeed { get; set; }
        public int NotesCount { get; set; }
        public int BombsCount { get; set; }
        public int ObstaclesCount { get; set; }
    }
}
