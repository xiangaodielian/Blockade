using UnityEngine;

namespace ApplicationManagement {
    public static class PrefsManager {

        #region Pref Keys

        private const string ControlWithMouse = "control_with_mouse";
        private const string LatestCheckpoint = "latest_checkpoint";
        private const string LevelUnlocked = "level_unlocked";
        private const string CurrentLevel = "current_level";
        private const string BallColorRed = "ball_color_red";
        private const string BallColorGreen = "ball_color_green";
        private const string BallColorBlue = "ball_color_blue";
        private const string HighScoreName1 = "high_score_name_1";
        private const string HighScore1 = "high_score_1";
        private const string HighScoreName2 = "high_score_name_2";
        private const string HighScore2 = "high_score_2";
        private const string HighScoreName3 = "high_score_name_3";
        private const string HighScore3 = "high_score_3";
        private const string HighScoreName4 = "high_score_name_4";
        private const string HighScore4 = "high_score_4";
        private const string HighScoreName5 = "high_score_name_5";
        private const string HighScore5 = "high_score_5";
        private const string TextureResolution = "texture_resolution";
        private const string MasterVolume = "master_volume";
        private const string MasterSfxVolume = "master_sfx_volume";
        private const string Difficulty = "difficulty";

        #endregion

        #region Volume Functions

        public static void SetMasterMusicVolume(float volume) {
            if(volume >= 0f && volume <= 1f) {
                PlayerPrefs.SetFloat(MasterVolume, volume);
            } else {
                Debug.LogError("Master Volume Out of Range");
            }
        }

        public static float GetMasterMusicVolume() {
            return PlayerPrefs.GetFloat(MasterVolume, 1f);
        }

        public static void SetMasterSFXVolume(float volume) {
            if(volume >= 0f && volume <= 1f) {
                PlayerPrefs.SetFloat(MasterSfxVolume, volume);
            } else {
                Debug.LogError("SFX Volume Out of Range");
            }
        }

        public static float GetMasterSFXVolume() {
            return PlayerPrefs.GetFloat(MasterSfxVolume, 1f);
        }

        #endregion

        #region Level Functions

        public static void SetLatestCheckpoint(int levelNum) {
            PlayerPrefs.SetInt(LatestCheckpoint, levelNum);
        }

        public static int GetLatestCheckpoint() {
            return PlayerPrefs.GetInt(LatestCheckpoint, 1);
        }

        public static void SetLevelUnlocked(int level) {
            PlayerPrefs.SetInt(LevelUnlocked, level);
        }

        public static int GetLevelUnlocked() {
            return PlayerPrefs.GetInt(LevelUnlocked, 1);
        }

        public static void SetCurrentLevel(int level) {
            PlayerPrefs.SetInt(CurrentLevel, level);
        }

        public static int GetCurrentLevel() {
            return PlayerPrefs.GetInt(CurrentLevel, 1);
        }

        #endregion

        #region Gameplay Pref Functions

        public static void SetMouseControl(bool useCursor) {
            if(useCursor)
                PlayerPrefs.SetInt(ControlWithMouse, 1);
            else
                PlayerPrefs.SetInt(ControlWithMouse, 0);
        }

        public static bool GetMouseControl() {
            int useCursor = PlayerPrefs.GetInt(ControlWithMouse, 0);

            if(useCursor != 0)
                return true;

            return false;
        }

        public static int GetDifficulty() {
            return PlayerPrefs.GetInt(Difficulty, 1);
        }

        public static void SetDifficulty(int value) {
            PlayerPrefs.SetInt(Difficulty, value);
        }

        public static Color GetBallColor() {
            float red = PlayerPrefs.GetFloat(BallColorRed, 1f);
            float green = PlayerPrefs.GetFloat(BallColorGreen, 0f);
            float blue = PlayerPrefs.GetFloat(BallColorBlue, 0f);

            return new Color(red, green, blue, 1f);
        }

        public static void SetBallColor(float red, float green, float blue) {
            PlayerPrefs.SetFloat(BallColorRed, red);
            PlayerPrefs.SetFloat(BallColorGreen, green);
            PlayerPrefs.SetFloat(BallColorBlue, blue);
        }

        #endregion

        #region Video Pref Functions

        public static void SetTextureRes(int res) {
            PlayerPrefs.SetInt(TextureResolution, res);
        }

        public static int GetTextureRes() {
#if UNITY_WEBGL
		int res = PlayerPrefs.GetInt(TEXTURE_RESOLUTION, 0);
		#else
            int res = PlayerPrefs.GetInt(TextureResolution, 1);
#endif

            return res;
        }

        #endregion

        #region High Score Functions

        public static int GetHighScore(int rank) {
            int highScore = 0;
            switch(rank) {
                case 1:
                    highScore = PlayerPrefs.GetInt(HighScore1, 50000);
                    break;
                case 2:
                    highScore = PlayerPrefs.GetInt(HighScore2, 30000);
                    break;
                case 3:
                    highScore = PlayerPrefs.GetInt(HighScore3, 20000);
                    break;
                case 4:
                    highScore = PlayerPrefs.GetInt(HighScore4, 10000);
                    break;
                case 5:
                    highScore = PlayerPrefs.GetInt(HighScore5, 5000);
                    break;
                default:
                    Debug.LogError("High Score Rank out of range!");
                    break;
            }

            return highScore;
        }

        public static void SetHighScore(int rank, int score) {
            int replacedScore = 0;
            switch(rank) {
                case 1:
                    replacedScore = GetHighScore(1);
                    PlayerPrefs.SetInt(HighScore1, score);
                    SetHighScore(2, replacedScore);
                    break;
                case 2:
                    replacedScore = GetHighScore(2);
                    PlayerPrefs.SetInt(HighScore2, score);
                    SetHighScore(3, replacedScore);
                    break;
                case 3:
                    replacedScore = GetHighScore(3);
                    PlayerPrefs.SetInt(HighScore3, score);
                    SetHighScore(4, replacedScore);
                    break;
                case 4:
                    replacedScore = GetHighScore(4);
                    PlayerPrefs.SetInt(HighScore4, score);
                    SetHighScore(5, replacedScore);
                    break;
                case 5:
                    PlayerPrefs.SetInt(HighScore5, score);
                    break;
                default:
                    Debug.LogError("High Score Rank out of range!");
                    break;
            }
        }

        public static string GetHighScoreName(int rank) {
            string highScorename = "null";
            switch(rank) {
                case 1:
                    highScorename = PlayerPrefs.GetString(HighScoreName1, "JEA");
                    break;
                case 2:
                    highScorename = PlayerPrefs.GetString(HighScoreName2, "JEA");
                    break;
                case 3:
                    highScorename = PlayerPrefs.GetString(HighScoreName3, "JEA");
                    break;
                case 4:
                    highScorename = PlayerPrefs.GetString(HighScoreName4, "JEA");
                    break;
                case 5:
                    highScorename = PlayerPrefs.GetString(HighScoreName5, "JEA");
                    break;
                default:
                    Debug.LogError("High Score Rank out of range!");
                    break;
            }

            return highScorename;
        }

        public static void SetHighScoreName(int rank, string name) {
            string replacedName = "null";
            switch(rank) {
                case 1:
                    replacedName = GetHighScoreName(1);
                    PlayerPrefs.SetString(HighScoreName1, name);
                    SetHighScoreName(2, replacedName);
                    break;
                case 2:
                    replacedName = GetHighScoreName(2);
                    PlayerPrefs.SetString(HighScoreName2, name);
                    SetHighScoreName(3, replacedName);
                    break;
                case 3:
                    replacedName = GetHighScoreName(3);
                    PlayerPrefs.SetString(HighScoreName3, name);
                    SetHighScoreName(4, replacedName);
                    break;
                case 4:
                    replacedName = GetHighScoreName(4);
                    PlayerPrefs.SetString(HighScoreName4, name);
                    SetHighScoreName(5, replacedName);
                    break;
                case 5:
                    PlayerPrefs.SetString(HighScoreName5, name);
                    break;
                default:
                    Debug.LogError("High Score Rank out of range!");
                    break;
            }
        }

        #endregion
    }
}