using UnityEngine;

namespace DESIGN
{
    public static class DESIGN_VALUES
    {
        public const int InitialHeroHP = 15;

        public const int MAX_MANA = 10;
        public const int BOT_PlayerInitialMana = 1;
        public const int TOP_PlayerInitialMana = 2;

        public const int ExtraManaPerTurn = 1;

        //--------------DEBUG---------------
        public const bool SwitchCameraPosition = true;

        //------------WAIT TIMES------------
        //The time that a card waits after any encounter against an enemy
        public const float timeAfterEncounter = 0.5f;
        //The time that a card waits after doing all its attack sequence (it does it even when it does not attack anyone)
        public const float timeAfterSequence = 0.3f;
        //The time that cards wait to appear after the enemy turn is finished
        public const float timeAfterTurn = 1f;

        //------------ANIMATIONS------------
        //The time a card takes to move from one position to another every time it encounters (from inital position to the 1st card, from 1st to 2nd...)
        public const float timeOnGoing = 0.08f;
        //The time a card takes to return to its initial position after all attack sequence
        public const float timeOnReturning = 0.05f;

        //The time the camera takes to switch positions
        public const float timeOnCameraSwitching = 1.2f;

        //Card selecting UP & DOWN animation
        public const float CardSelectingAnimationUpDistance = 0.25f;
        public const float timeOnCardSelectingAnimation = 0.1f;

        //--------------OTHERS--------------
        public static readonly Color cardHighlightColor = new Color(0.98f, 1.0f, 0.80f, 1.0f);

        //---------------SIZES--------------
        public const float OnBoardCardSize = 4.5f;
        public const float CardSelectingDistanceToCamera = 3f;
        public const float CardZoomDistanceToCamera = 8f;

    }
}