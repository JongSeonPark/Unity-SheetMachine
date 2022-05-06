using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ChickenGames.SheetMachine
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        static T _instance = null;
        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                    if (!_instance)
                    {
                        string filter = "t:" + typeof(T);
                        var guids = AssetDatabase.FindAssets(filter);
                        if (guids.Length > 0)
                        {
                            if (guids.Length > 1)
                                Debug.LogWarningFormat("Multiple {0} assets are found.", typeof(T));

                            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                            Debug.LogFormat("Using {0}.", assetPath);
                            _instance = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                        }
                        // no found any setting file.
                        else
                        {
                            Debug.LogWarning("No instance of " + typeof(T).Name + " is loaded. Please create a " + typeof(T).Name + " asset file.");
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
