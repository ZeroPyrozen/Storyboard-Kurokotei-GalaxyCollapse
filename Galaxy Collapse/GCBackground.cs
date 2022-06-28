using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class GCBackground : StoryboardObjectGenerator
    {
        public OsbSprite DefaultSprite { get; set; }
        public OsbSprite BlueSprite { get; set; }
        public OsbSprite RectangleSprite { get; set; }
        public OsbSprite NegativeSprite { get; set; }
        public override void Generate()
        {
            DefaultSprite = GetLayer("DefaultBackground").CreateSprite("bg.jpg");
            RectangleSprite = GetLayer("RectangleBackground").CreateSprite("sb/p.png");
            BlueSprite = GetLayer("BlueBackground").CreateSprite("sb/blue.jpg");
            NegativeSprite = GetLayer("PulseForeground").CreateSprite("sb/red-negative.jpg");

            FirstBuildup();
            FirstKiai();
            SecondBuildup();
            SecondKiai();
            ThirdBuildup();
            ThirdKiai();
            FourthBuildup();
            FourthKiai();
            Outro();
        }

        void Flash(int startTime, bool isBlue = false)
        {
            double beatDuration = (int)Beatmap.GetTimingPointAt(startTime).BeatDuration;
            if(!isBlue)
            {
                DefaultSprite.Fade(OsbEasing.In, startTime, startTime + beatDuration, 1, 0.5);
            }
            else
            {
                BlueSprite.Fade(OsbEasing.Out, startTime, startTime + beatDuration, 1, 0.5);
            }
            
        }

        void RedFlash(int startTime, int endTime)
        {
            double flashTimeStamp = startTime;
            while(flashTimeStamp<endTime)
            {
                double beatDuration = (int)Beatmap.GetTimingPointAt(startTime).BeatDuration;
                DefaultSprite.Color(flashTimeStamp, flashTimeStamp + beatDuration*4, Color4.Red, Color4.White);
                flashTimeStamp = flashTimeStamp + beatDuration*8;
            }
        }

        void AlternateRedFlash(int startTime, int endTime, int mode = 1)
        {
            double flashTimeStamp = startTime;
            while(flashTimeStamp<endTime)
            {
                double beatDuration = (int)Beatmap.GetTimingPointAt(startTime).BeatDuration;
                if(mode == 1)
                {
                    DefaultSprite.Color(flashTimeStamp, flashTimeStamp + beatDuration, Color4.White, Color4.Red);
                    DefaultSprite.Color(flashTimeStamp + beatDuration, flashTimeStamp + beatDuration*2, Color4.Red, Color4.White);
                    flashTimeStamp = flashTimeStamp + beatDuration*2;
                }
                else
                {
                    DefaultSprite.Color(flashTimeStamp, flashTimeStamp + beatDuration*0.5, Color4.Black, Color4.Red);
                    DefaultSprite.Color(flashTimeStamp + beatDuration*0.5, flashTimeStamp + beatDuration, Color4.Red, Color4.Black);
                    flashTimeStamp = flashTimeStamp + beatDuration;
                }
            }
        }

        void FirstBuildup()
        {
            DefaultSprite.Scale(0, Math.Max((float)854/(float)1366, (float)477/(float)768));
            DefaultSprite.Color(0, Color4.White);
            RectangleSprite.ScaleVec(0, 1024, 480);
            RectangleSprite.Color(0, Color4.Black);

            DefaultSprite.Fade(0, 28621, 0, 1);
            RectangleSprite.Fade(0, 28621, 1, 0);
            DefaultSprite.Fade(28621, 28732, 1, 0);
            RectangleSprite.Fade(28621, 1);
        }

        void SecondBuildup()
        {
            Flash(67843);
            Flash(74954);
            Flash(82065);

            DefaultSprite.Fade(85399, 0.75);
            DefaultSprite.Fade(88010, 88287, 0.75, 0);

            RectangleSprite.Fade(88287, 1);
            RectangleSprite.Fade(89176, 0);
        }

        void ThirdBuildup()
        {
            BlueSprite.Scale(117676, Math.Max((float)854/(float)1366, (float)477/(float)768));

            BlueSprite.Fade(117676, 117885, 0, 0.5);
            BlueSprite.Fade(176528, 0);
            RectangleSprite.Fade(176528, 1);
            RectangleSprite.Fade(179428, 0);

            Flash(130065, true);
            Flash(142865, true);
        }

        void FourthBuildup()
        {
            RectangleSprite.Fade(230677, 1);
            RectangleSprite.Fade(244190, 244405, 1, 0);
            DefaultSprite.Color(244190, Color4.White);
            DefaultSprite.Fade(244190, 244405, 0, 0.5);

            Flash(251516);
            Flash(258627);
            Flash(265738);

            DefaultSprite.Fade(272627, 0);
            BlueSprite.Fade(272627, 0.5);
            BlueSprite.Fade(286821, 287071, 0.5, 0.75);
            BlueSprite.Fade(301293, 0);
            
            RectangleSprite.Fade(301293, 1);
            RectangleSprite.Fade(304092, 0);
        }

        void FirstKiai()
        {
            DefaultSprite.Fade(32287,1);
            RectangleSprite.Fade(32287, 0);
            DefaultSprite.Fade(60454, 60565, 1, 0.5);
        }

        void SecondKiai()
        {
            DefaultSprite.Fade(89176, 1);
            DefaultSprite.Fade(117621, 0);
        }

        void ThirdKiai()
        {
            DefaultSprite.Fade(179428, 1);
            RedFlash(179727, 190928);
            DefaultSprite.Fade(190928, 191428, 1, 0);
            RectangleSprite.Fade(190928, 1);
            RectangleSprite.Fade(192528, 0);
            DefaultSprite.Fade(192528, 1);
            RedFlash(192528, 202828);
            DefaultSprite.Fade(203528, 203728, 1, 0);
            RectangleSprite.Fade(203528, 1);
            RectangleSprite.Fade(205378, 0);
            DefaultSprite.Fade(205378, 1);
            DefaultSprite.Fade(217078, 217328, 1, 0);
            RectangleSprite.Fade(217078, 1);
            RectangleSprite.Fade(218103, 0);
            DefaultSprite.Fade(218103, 1);
            AlternateRedFlash(227628, 229128);
            AlternateRedFlash(229328, 230928, 2);
            DefaultSprite.Fade(230928, 0);
        }

        void FourthKiai()
        {
            DefaultSprite.Fade(304093, 304493, 0, 1);
            RedFlash(304493, 315593);
            DefaultSprite.Fade(315618, 315693, 1, 0);
            RectangleSprite.Fade(315618, 1);
            RectangleSprite.Fade(317268, 0);
            DefaultSprite.Fade(317268, 1);
            RedFlash(317293, 328243);
            DefaultSprite.Fade(328293, 328393, 1, 0);
            RectangleSprite.Fade(328293, 1);
            RectangleSprite.Fade(330093, 0);
            NegativeSprite.Scale(330093, Math.Max((float)854/(float)1366, (float)477/(float)768));
            NegativeSprite.Fade(330093, 1);
            NegativeSprite.Fade(341202, 341319, 1, 0);
            RectangleSprite.Fade(341202, 1);
            RectangleSprite.Fade(342093, 0);
            NegativeSprite.Fade(342093, 1);
            NegativeSprite.Fade(351093, 0);
            RectangleSprite.Fade(351093, 1);
            RectangleSprite.Fade(354093, 0);
        }

        void Outro()
        {
            BlueSprite.Fade(353905, 353999, 0, 1);
            BlueSprite.Fade(377999, 378093, 1, 0.5);
            BlueSprite.Fade(401905, 405891, 0.5, 0);
        }
    }
}
