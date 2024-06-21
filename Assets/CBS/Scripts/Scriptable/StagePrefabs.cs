using UnityEngine;

namespace CBS.Scriptable
{
    [CreateAssetMenu(fileName = "StagePrefabs", menuName = "CBS/Add new Stage Prefabs")]
    public class StagePrefabs : CBSScriptable
    {
        public override string ResourcePath => "Scriptable/StagePrefabs";

        public GameObject StageWindow;
    }
}