﻿using CBS.Core;
using CBS.Models;

namespace CBS.UI
{
    public class BattlePassTasksScroller : PreloadScroller<CBSProfileTask>
    {
        protected override float DeltaToPreload => 0.7f;
    }
}