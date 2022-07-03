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
    public class GCForeground : StoryboardObjectGenerator
    {
        public OsbSprite FlashSprite { get; set; }
        public OsbSprite ShakeSprite { get; set; }
        public OsbSprite PulseSprite { get; set; }
        public OsbSprite PulseNegativeSprite { get; set; }
        public OsbSprite RectangleSprite { get; set; }
        public OsbSprite TopBlinderSprite { get; set; }
        public OsbSprite BottomBlinderSprite { get; set; }
        public override void Generate()
        {
            PulseSprite = GetLayer("PulseForeground").CreateSprite("bg.jpg");
            PulseNegativeSprite = GetLayer("PulseForeground").CreateSprite("sb/red-negative.jpg");
            FlashSprite = GetLayer("FlashForeground").CreateSprite("bg.jpg");
            ShakeSprite = GetLayer("ShakeForeground").CreateSprite("sb/red-distort.jpg");
            RectangleSprite = GetLayer("RectangleForeground").CreateSprite("sb/p.png");
            TopBlinderSprite = GetLayer("BlinderForeground").CreateSprite("sb/p.png", OsbOrigin.TopCentre, new Vector2(320, 0));
            BottomBlinderSprite = GetLayer("BlinderForeground").CreateSprite("sb/p.png", OsbOrigin.BottomCentre, new Vector2(320, 480));
            
            FirstBuildup();
            FirstKiai();
            SecondBuildup();
            SecondKiai();
            ThirdBuildup();
            ThirdKiai();
            FourthBuildup();
            FourthKiai();
            
        }

        void Blinder(int startTime, int endTime)
        {
            double beatDuration = (int)Beatmap.GetTimingPointAt(startTime).BeatDuration;

            TopBlinderSprite.Color(startTime, Color4.Black);
            BottomBlinderSprite.Color(startTime, Color4.Black);
            
            TopBlinderSprite.ScaleVec(OsbEasing.In, startTime, startTime + beatDuration, 1024, 0, 1024, 240);
            TopBlinderSprite.Fade(startTime, startTime + beatDuration, 0.5, 1);

            TopBlinderSprite.ScaleVec(OsbEasing.In, endTime - beatDuration, endTime, 1024, 240, 1024, 0);
            TopBlinderSprite.Fade(endTime, 0);

            BottomBlinderSprite.ScaleVec(OsbEasing.In, startTime, startTime + beatDuration, 1024, 0, 1024, 240);
            BottomBlinderSprite.Fade(startTime, startTime + beatDuration, 0.5, 1);

            BottomBlinderSprite.ScaleVec(OsbEasing.In, endTime - beatDuration, endTime, 1024, 240, 1024, 0);
            BottomBlinderSprite.Fade(endTime, 0);
        }

        void Pulse(int startTime, bool isNegative = false)
        {
            double beatDuration = (int)Beatmap.GetTimingPointAt(startTime).BeatDuration / 4;
            if(!isNegative)
            {
                PulseSprite.Color(startTime, Color4.Red);
                PulseSprite.Fade(startTime, 1);
                PulseSprite.Scale(startTime, startTime + beatDuration, 0.65, 0.75);
                PulseSprite.Scale(OsbEasing.Out, startTime + beatDuration, startTime + beatDuration * 2, 0.75, 0.65);
                PulseSprite.Fade(startTime + beatDuration, startTime + beatDuration * 3, 1, 0);
            }
            else
            {
                PulseNegativeSprite.Fade(startTime, 1);
                PulseNegativeSprite.Scale(startTime, startTime + beatDuration, 0.65, 0.75);
                PulseNegativeSprite.Scale(OsbEasing.Out, startTime + beatDuration, startTime + beatDuration * 2, 0.75, 0.65);
                PulseNegativeSprite.Fade(startTime + beatDuration, startTime + beatDuration * 3, 1, 0);
            }
        }

        void Flash(int rangeStart, int rangeEnd, bool isNegative = false)
        {
            double beatDuration = rangeStart;
            while(beatDuration<rangeEnd)
            {
                int startTime = (int)beatDuration;
                int endTime = (int)beatDuration + (int)Beatmap.GetTimingPointAt(startTime).BeatDuration;
                if(isNegative)
                {
                    PulseNegativeSprite.Fade(startTime, endTime, 1, 0);
                    PulseNegativeSprite.Scale(startTime, endTime, 0.65, 0.90);
                }
                else
                {
                    FlashSprite.Fade(startTime, endTime, 1, 0);
                    FlashSprite.Scale(startTime, endTime, 0.65, 0.90);
                }
                beatDuration += Beatmap.GetTimingPointAt(startTime).BeatDuration * 2;
            }
        }

        void Shake(int startTime, int shakeTimes)
        {
            double shakeTimeStamp = startTime;
            for(int i=0; i<shakeTimes; i++)
            {
                Log(shakeTimeStamp);
                ShakeSprite.Fade(shakeTimeStamp, shakeTimeStamp + Beatmap.GetTimingPointAt((int)shakeTimeStamp).BeatDuration / 4, 0, 1);
                ShakeSprite.MoveX(shakeTimeStamp, shakeTimeStamp + Beatmap.GetTimingPointAt((int)shakeTimeStamp).BeatDuration / 4, 320, 315);
                shakeTimeStamp = shakeTimeStamp + Beatmap.GetTimingPointAt((int)shakeTimeStamp).BeatDuration / 4;
                ShakeSprite.Fade(shakeTimeStamp, shakeTimeStamp + Beatmap.GetTimingPointAt((int)shakeTimeStamp).BeatDuration / 4, 1, 0);
                ShakeSprite.MoveX(shakeTimeStamp, shakeTimeStamp + Beatmap.GetTimingPointAt((int)shakeTimeStamp).BeatDuration / 4, 315, 325);
                shakeTimeStamp = shakeTimeStamp + Beatmap.GetTimingPointAt((int)shakeTimeStamp).BeatDuration / 4;
            }
        }

        void WhiteFlash(int startTime, double beat)
        {
            double beatDuration = Beatmap.GetTimingPointAt(startTime).BeatDuration * beat;
            RectangleSprite.Fade(startTime, startTime+beatDuration, 0.75, 0);
        }

        void Flicker(int startTime, int endTime, double beat)
        {
            double flickerTimeStamp = startTime;
            while(flickerTimeStamp<endTime)
            {
                PulseNegativeSprite.Fade(flickerTimeStamp, flickerTimeStamp + Beatmap.GetTimingPointAt((int)flickerTimeStamp).BeatDuration * beat, 0, 0.75);
                flickerTimeStamp = flickerTimeStamp + Beatmap.GetTimingPointAt((int)flickerTimeStamp).BeatDuration * beat;
                if(flickerTimeStamp>endTime)
                    break;
                PulseNegativeSprite.Fade(flickerTimeStamp, flickerTimeStamp + Beatmap.GetTimingPointAt((int)flickerTimeStamp).BeatDuration * beat, 0.75, 0);
                flickerTimeStamp = flickerTimeStamp + Beatmap.GetTimingPointAt((int)flickerTimeStamp).BeatDuration * beat;
            }
        }

        void FirstBuildup()
        {
            Pulse(14371);
            Pulse(21621);
        }

        void SecondBuildup()
        {
            RectangleSprite.ScaleVec(86287, 1024, 480);
            RectangleSprite.Color(86287, Color4.White);
            WhiteFlash(85621, 2);
            WhiteFlash(86510, 2);
            WhiteFlash(87399, 2);
            WhiteFlash(87843, 1);
        }

        void ThirdBuildup()
        {
            Pulse(155665, true);
            Pulse(157119, true);
            Pulse(158451, true);
            Pulse(159682, true);
            Pulse(160824, true);
            Pulse(161889, true);
            Pulse(162889, true);
            Pulse(163830, true);
            Pulse(164718, true);
            Pulse(165561, true);
            Pulse(166360, true);
            Pulse(167121, true);
            Pulse(167848, true);
            Pulse(168544, true);
            Pulse(169210, true);
            Pulse(169850, true);
            Pulse(170464, true);
            Pulse(171057, true);
            Pulse(171628, true);
            Pulse(172179, true);
            Pulse(172712, true);
            Pulse(173227, true);
            Pulse(173727, true);
            Pulse(174212, true);
            //174682
            Flicker(174682, 176008, 1);
            Flicker(176081, 176528, 0.5);
        }

        void FourthBuildup()
        {
            WhiteFlash(230944, 0.5);

            WhiteFlash(232272, 0.5);
            WhiteFlash(232479, 0.5);
            WhiteFlash(232583, 0.5);

            WhiteFlash(234238, 0.5);

            WhiteFlash(235583, 0.5);
            WhiteFlash(235790, 0.5);
            WhiteFlash(235893, 0.5);

            WhiteFlash(236721, 0.5);
            WhiteFlash(236824, 0.5);
            WhiteFlash(236928, 0.5);
            WhiteFlash(237031, 0.5);

            WhiteFlash(237134, 2);

            WhiteFlash(238940, 0.5);
            WhiteFlash(239155, 0.5);
            WhiteFlash(239262, 0.5);

            WhiteFlash(240976, 0.5);

            WhiteFlash(242369, 0.5);
            WhiteFlash(242583, 0.5);
            WhiteFlash(242690, 0.5);

            WhiteFlash(243548, 0.5);
            WhiteFlash(243655, 0.5);
            WhiteFlash(243762, 0.5);
            WhiteFlash(243869, 0.5);

            WhiteFlash(243976, 1);
            WhiteFlash(244190, 1);

            RectangleSprite.Color(269293, Color4.White);
            WhiteFlash(269293, 2);
            WhiteFlash(270182, 2);
            WhiteFlash(271071, 2);
            
            WhiteFlash(271516, 1);

            WhiteFlash(271960, 1);
            WhiteFlash(272182, 1);
            Blinder(272405, 272849);
        }

        void FirstKiai()
        {
            ShakeSprite.Scale(0, Math.Max((float)854/(float)1366, (float)477/(float)768));
            FlashSprite.Color(32287, Color4.White);
            Flash(32287, 59767);
            Shake(46065, 4);
            Shake(57843, 4);
            Shake(58732, 4);
            Shake(59176, 4);
            Shake(59621, 11);
        }

        void SecondKiai()
        {
            Flash(89176, 95565);
            Shake(95843, 2);
            Flash(96287, 102843);
            Shake(102957, 4);
            Flash(103399, 110010);
            Shake(110065, 2);
            Flash(110510, 114732);
            Shake(114732, 2);
            Flash(114954, 115565);
            Shake(115621, 2);
            Flash(115843, 116024);
            Shake(116065, 2);
            Flash(116274, 116468);
            Shake(116510, 10);
            Blinder(117399, 118065);
        }

        void ThirdKiai()
        {
            FlashSprite.Color(205444, Color4.Red);
            Flash(205444, 217078);
            Flash(218103, 227628);
            FlashSprite.Color(227628, Color4.White);
        }

        void FourthKiai()
        {
            PulseNegativeSprite.Scale(330093, Math.Max((float)854/(float)1366, (float)477/(float)768));
            PulseNegativeSprite.Color(330093, Color4.White);
            Flash(330093, 341061, true);
            Flash(342093, 350999, true);
            PulseNegativeSprite.Scale(351093, Math.Max((float)854/(float)1366, (float)477/(float)768));
            PulseNegativeSprite.Color(351093, Color4.White);
            Flicker(351093, 352546, 0.5);
            Flicker(352593, 354093, 0.25);
        }
    }
}
