
using System.Collections.Generic;
using UnityEngine;

#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

[CreateAssetMenu(fileName = "MetagameHook.Asset", menuName = "TOJam 2025/Metagame Hook")]
public class Metagame : ScriptableObject {
    [System.Flags]
    public enum Count {
        ThisGame = 1,
        OtherGames = 2,
        AllTOJam2025 = ThisGame | OtherGames
    }

    [System.Serializable]
    public class GameInfo {
        public string name;
        [HideInInspector]
        public string key;
        public string[] credits;
        public int launchCount, playCount, winCount, lossCount;
        public float topScore;

        public GameInfo(string name, string[] credits) {
            this.name = name;
            this.credits = credits;
        }

        public GameInfo Clone() {
            var clonedCredits = new string[credits.Length];
            System.Array.Copy(credits, clonedCredits, credits.Length);
            var clone = new GameInfo(name, clonedCredits);
            clone.launchCount = launchCount;
            clone.playCount = playCount;
            clone.winCount = winCount;
            clone.lossCount = lossCount;
            clone.topScore = topScore;
            return clone;
        }
    }

    [System.Serializable]
    public struct GameInfos {
        public List<GameInfo> games;
    }


    [SerializeField, Tooltip("Title of your game as it should appear in other games / in the index")]
    string _displayName;

    [SerializeField, HideInInspector]
    string _key;

    [SerializeField, Tooltip("Add one entry per team member who consents to have their name/handle appear in other games - can be left empty")]
    string[] _credits;

    static bool _hasLaunched;
    static bool _playInProgress;

    #pragma warning disable CS0414 // Used in WebGL path only - please don't warn on PC/editor!
    bool _canSave;
    #pragma warning restore CS0414

    GameInfos _saveData;
    GameInfo _thisGameData;

    [Tooltip("Dummy data for testing, and for easy reference for how the data will look")]
    public List<GameInfo> otherGames;

    public void StartPlay() {
        _thisGameData.playCount++;
        _playInProgress = true;
        TrySave();
    }

    public void RecordWin(float score = 0) {
        if (!_playInProgress) return;
        _playInProgress = false;
        _thisGameData.winCount++;
        _thisGameData.topScore = Mathf.Max(score, _thisGameData.topScore);
        TrySave();
    }

    public void RecordLoss(float score = 0) {
        if (!_playInProgress) return;
        _playInProgress = false;
        _thisGameData.lossCount++;
        _thisGameData.topScore = Mathf.Max(score, _thisGameData.topScore);
        TrySave();
    }

    public int CountGames(Count mode = Count.OtherGames) {
        int count = 0;
        if (mode.HasFlag(Count.ThisGame)) {
            count = 1;
        }
        if (mode.HasFlag(Count.OtherGames)) {
            count += otherGames.Count;
        }
        return count;
    }

    public int CountGamesPlayed(Count mode = Count.OtherGames) {
        int count = 0;
        if (mode.HasFlag(Count.ThisGame) && _thisGameData.playCount > 0) {
            count = 1;
        }
        if (mode.HasFlag(Count.OtherGames)) {
            foreach(var game in otherGames)
                if (game.playCount > 0) count++;
        }
        return count;
    }

    public int CountGamesWon(Count mode = Count.OtherGames) {
        int count = 0;
        if (mode.HasFlag(Count.ThisGame) && _thisGameData.winCount > 0) {
            count = 1;
        }
        if (mode.HasFlag(Count.OtherGames)) {
            foreach(var game in otherGames)
                if (game.winCount > 0) count++;
        }
        return count;
    }

    public int CountGamesLost(Count mode = Count.OtherGames) {
        int count = 0;
        if (mode.HasFlag(Count.ThisGame) && _thisGameData.lossCount > 0) {
            count = 1;
        }
        if (mode.HasFlag(Count.OtherGames)) {
            foreach(var game in otherGames)
                if (game.lossCount > 0) count++;
        }
        return count;
    }

    public int CountTotalLaunches(Count mode = Count.OtherGames) {
        int count = 0;
        if (mode.HasFlag(Count.ThisGame)) {
            count = _thisGameData.launchCount;
        }
        if (mode.HasFlag(Count.OtherGames)) {
            foreach(var game in otherGames)
                count += game.launchCount;
        }
        return count;
    }

    public int CountTotalPlays(Count mode = Count.OtherGames) {
        int count = 0;
        if (mode.HasFlag(Count.ThisGame)) {
            count = _thisGameData.playCount;
        }
        if (mode.HasFlag(Count.OtherGames)) {
            foreach(var game in otherGames)
                count += game.playCount;
        }
        return count;
    }

    public int CountTotalWins(Count mode = Count.OtherGames) {
        int count = 0;
        if (mode.HasFlag(Count.ThisGame)) {
            count = _thisGameData.winCount;
        }
        if (mode.HasFlag(Count.OtherGames)) {
            foreach(var game in otherGames)
                count += game.winCount;
        }
        return count;
    }

    public int CountTotalLosses(Count mode = Count.OtherGames) {
        int count = 0;
        if (mode.HasFlag(Count.ThisGame)) {
            count = _thisGameData.lossCount;
        }
        if (mode.HasFlag(Count.OtherGames)) {
            foreach(var game in otherGames)
                count += game.lossCount;
        }
        return count;
    }

    public float GetTopScore(Count mode = Count.OtherGames) {
        float top = 0;        
        if (mode.HasFlag(Count.ThisGame)) {
            top = _thisGameData.topScore;
        }
        if (mode.HasFlag(Count.OtherGames)) {
            foreach(var game in otherGames)
                top = Mathf.Max(top, game.topScore);
        }
        return top;
    }

    [ContextMenu("Reset Cached Data")]
    void ResetCache() {
        _thisGameData = new(_displayName, _credits);
    }

    void OnEnable() {
        const string defaultJson = "{\"games\":[]}";
        string json = GetGameData();

        _canSave = true;
        if (json == "failed") {
            _canSave = false;
            Debug.LogError("Denied access to use metagame JSON");
            json = defaultJson;
        } else if (string.IsNullOrWhiteSpace(json)) {
            Debug.LogError("Read empty / no metagame JSON");
            json = defaultJson;
        }
        
        try {
            _saveData = JsonUtility.FromJson<GameInfos>(json);
        } catch (System.Exception e) {
            Debug.LogError($"Failed to parse metagame JSON: {e.Message}");
            _saveData = new() {
                games = new()
            };
        }
        
        if (otherGames == null) {
            otherGames = new();
        } else { 
            otherGames.Clear();
        }

        foreach(var game in _saveData.games) {            
            //Debug.Log($"Found game {game.name} ({game.key}, launched {game.launchCount} / played {game.playCount})");
            if (game.key == _key) {
                _thisGameData = game;
                game.name = _displayName;
                game.credits = _credits;
                if (!_hasLaunched)
                    game.launchCount++;
            } else {
                otherGames.Add(game.Clone());
            }
        }

        if (_thisGameData == null) {          
            _thisGameData = new GameInfo(_displayName, _credits);
            _thisGameData.key = _key;
            _thisGameData.launchCount = 1;
            _saveData.games.Add(_thisGameData);
        }

        if(!_hasLaunched) {
            TrySave();
            _hasLaunched = true;
        }
    }
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern string LoadTOJamMetaData();

    [DllImport("__Internal")]
    private static extern void SaveTOJamMetaData(string jsonText);

    string GetGameData() {
        string result = LoadTOJamMetaData();
        Debug.Log("Loaded from JSON: " + result);
        return result;
    }

    void TrySave() {
        if (_canSave)
            SaveTOJamMetaData(JsonUtility.ToJson(_saveData));
    }
#else
    #if UNITY_EDITOR
    void OnValidate() {
        if (!string.IsNullOrWhiteSpace(_key)) return;

        UnityEditor.Undo.RecordObject(this, "Assign Unique Key");
        UnityEditor.EditorUtility.SetDirty(this);
        _key = UnityEngine.GUID.Generate().ToString();
    }
    #endif

    string GetGameData() {
        if (Application.isEditor) {
            //Debug.Log("Editor mode: substituting test JSON");
            return JsonUtility.ToJson(otherGames);
        } else {
            //Debug.Log("Non-web build: disabling JSON load/save");
            return "failed";
        }
    }

    void TrySave() {}
#endif
}
