﻿using CBS.Core;
using CBS.Models;

namespace CBS.UI
{
    public class CalendarScroller : PreloadScroller<CalendarPosition>
    {
        protected override float DeltaToPreload => 0.7f;
    }
}