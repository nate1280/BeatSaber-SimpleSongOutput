using SimpleSongOutput.Extensions;
using SimpleSongOutput.Models;
using System;
using System.IO;
using UnityEngine;

namespace SimpleSongOutput.Misc
{
    public class Utilities
    {
        /// <summary>
        /// Convert Difficulty enum to string
        /// </summary>
        /// <param name="difficulty">Difficulty to convert to string</param>
        /// <returns></returns>
        public static string DifficultyToString(BeatmapDifficulty difficulty)
        {
            if (difficulty == BeatmapDifficulty.ExpertPlus) return "Expert+";
            return difficulty.ToString();
        }

        /// <summary>
        /// Writes out an empty string to text file, and optionally writes 0 bytes to thumbnail image
        /// </summary>
        public static void ClearOutputFile()
        {
            if (string.IsNullOrEmpty(Plugin.cfg.SongTemplateEmpty))
            {
                // write empty string
                WriteToOutputFile(string.Empty);
            }
            else
            {
                // build template full path
                var templateEmptyPath = Path.Combine(Plugin.DataPath, Plugin.cfg.SongTemplateEmpty);

                // read template file
                var songTemplateEmpty = File.ReadAllText(templateEmptyPath);

                // write out song information using template file
                WriteToOutputFile(songTemplateEmpty);
            }

            // check if we need to save the thumbnail
            if (Plugin.cfg.SaveThumbnail)
            {
                // write out 0 bytes
                File.WriteAllBytes(Plugin.FullThumbnailFilename, new byte[0]);
            }
        }

        /// <summary>
        /// Write the given level/difficulty out to file
        /// </summary>
        /// <param name="level">Beat Saber level data</param>
        /// <param name="difficulty">Difficulty of song</param>
        public static async void WriteToOutputFile(IBeatmapLevel level, IDifficultyBeatmap difficultyBeatmap)
        {
            // get song cover texture
            var tex = await level.GetCoverImageTexture2DAsync(System.Threading.CancellationToken.None);

            // resize to thumbnail size
            tex = tex.ResizeTexture(Plugin.cfg.ThumbnailSize, Plugin.cfg.ThumbnailSize);

            // encode to base64
            var base64Image = $"data:image/jpg;base64,{Convert.ToBase64String(tex.EncodeToJPG())}";
            
            // build song info model
            var songInfo = new SongInfoModel
            {
                SongName = level.songName,
                SongSubName = level.songSubName,
                SongAuthorName = level.songAuthorName,
                LevelAuthorName = level.levelAuthorName,
                Difficulty = difficultyBeatmap.difficulty,
                Base64Thumbnail = base64Image,
                SongBPM = level.beatsPerMinute,
                NoteJumpSpeed = difficultyBeatmap.noteJumpMovementSpeed,
				NotesCount = difficultyBeatmap.beatmapData.notesCount,
				BombsCount = difficultyBeatmap.beatmapData.bombsCount,
				ObstaclesCount = difficultyBeatmap.beatmapData.obstaclesCount
            };

            // check if we are using a template file
            if (string.IsNullOrEmpty(Plugin.cfg.SongTemplate))
            {
                // write out song information using SongFormat template
                WriteToOutputFile(DynamicText.Parse(Plugin.cfg.SongFormat, songInfo));
            }
            else
            {
                // build template full path
                var templatePath = Path.Combine(Plugin.DataPath, Plugin.cfg.SongTemplate);

                // read template file
                var songFormat = File.ReadAllText(templatePath);

                // write out song information using template file
                WriteToOutputFile(DynamicText.Parse(songFormat, songInfo));
            }
        }

        /// <summary>
        /// Write the given output to text file
        /// </summary>
        /// <param name="output">String to output</param>
        public static void WriteToOutputFile(string output)
        {
            // log
            //Plugin.Log.Info($"Writing to {Plugin.cfg.TextFilename} :: {output}");

            // output to file
            File.WriteAllText(Plugin.FullTextFilename, output);
        }

        /// <summary>
        /// Write thumbnail for the passed level to file
        /// </summary>
        /// <param name="level">Level containing the song information</param>
        public static async void WriteThumbnail(IBeatmapLevel level)
        {
            // get song cover texture
            var tex = await level.GetCoverImageTexture2DAsync(System.Threading.CancellationToken.None);

            // resize to thumbnail size
            tex = tex.ResizeTexture(Plugin.cfg.ThumbnailSize, Plugin.cfg.ThumbnailSize);

            // encode to jpg
            var img = tex.EncodeToJPG();

            // output image to file
            File.WriteAllBytes(Plugin.FullThumbnailFilename, img);
        }
    }
}
