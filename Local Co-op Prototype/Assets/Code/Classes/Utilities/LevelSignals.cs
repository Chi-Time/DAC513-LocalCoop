using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Classes.Utilities
{
    public static class LevelSignals
    {
        /// <summary> When a bullet has hit an entity. </summary>
        public static event Action<int, GameObject> OnEntityHit;
        public static void HitEntity (int damage, GameObject entity) { if (OnEntityHit != null) OnEntityHit (damage, entity); }

        /// <summary> When the score has been increased. </summary>
        public static event Action<int> OnScoreIncreased;
        public static void IncreaseScore (int score) { if (OnScoreIncreased != null) OnScoreIncreased (score); }
    }
}
