using BS_Utils.Utilities;
using SimpleSongOutput.Misc;
using System.Collections;
using UnityEngine;

namespace SimpleSongOutput
{
    public class SimpleSongOutput : MonoBehaviour
    {
        public bool initialized = false;

        private static SimpleSongOutput _instance = null;
        public static SimpleSongOutput Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject("SimpleSongOutput").AddComponent<SimpleSongOutput>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        internal void OnLoad()
        {
            initialized = false;

            // remove potential for duplicate event calls
            BSEvents.gameSceneLoaded -= SongStarted;
            BSEvents.menuSceneLoaded -= MenuSceneLoaded;

            // attach to level selected event if we are enabled
            if (Plugin.cfg.Enabled)
            {
                BSEvents.gameSceneLoaded += SongStarted;
                BSEvents.menuSceneLoaded += MenuSceneLoaded;
            }

            // clear the file
            Utilities.ClearOutputFile();
        }

        private void MenuSceneLoaded()
        {
            StartCoroutine(MenuSceneLoadedCoroutine());
        }

        public IEnumerator MenuSceneLoadedCoroutine()
        {
            // write empty string to file to clear it
            Utilities.ClearOutputFile();

            yield return null;

            // do udp broadcast
            if (!string.IsNullOrEmpty(Plugin.cfg.UdpStopAction))
            {
                Utilities.UdpBroadcastStop();
            }

            // return
            yield return null;
        }

        public void SongStarted()
        {
            StartCoroutine(SongStartedCoroutine());
        }

        public IEnumerator SongStartedCoroutine()
        {
            if (!BS_Utils.Plugin.LevelData.IsSet)
            {
                Plugin.Log.Debug("BS_Utils Level Data not set, aborting");

                // write unknown song to file
                Utilities.WriteToOutputFile("Unknown Song");

                // break out, no song data
                yield break;
            }

            // get song information
            var difficultyBeatmap = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap;

            // write out song information
            Utilities.WriteToOutputFile(difficultyBeatmap.level, difficultyBeatmap);

            yield return null;

            // do udp broadcast
            if (!string.IsNullOrEmpty(Plugin.cfg.UdpStartAction))
            {
                Utilities.UdpBroadcastStart(difficultyBeatmap.level, difficultyBeatmap);
            }

            yield return null;

            // check if we need to save the thumbnail
            if (Plugin.cfg.SaveThumbnail)
            {
                // save thumbnail
                Utilities.WriteThumbnail(difficultyBeatmap.level);
            }

            yield return null;
        }
    }
}
