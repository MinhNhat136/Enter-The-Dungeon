﻿using CBS.Models;
using UnityEngine;

namespace CBS.UI.Matchmaking
{
    public class DuelPreview : MonoBehaviour, IMatchmakingPreview
    {
        [SerializeField]
        private MatchmakingPreviewPlayer[] PlayersPreview;

        public void Draw(CBSStartMatchResult result)
        {
            var firstPlayer = result.Players[0];
            var secondPlayer = result.Players[1];

            PlayersPreview[0].DrawUser(firstPlayer);
            PlayersPreview[1].DrawUser(secondPlayer);
        }

        // button click
        public void StartMatch()
        {
            gameObject.SetActive(false);
        }
    }
}
