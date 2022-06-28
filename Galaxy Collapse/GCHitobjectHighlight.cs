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
    public class GCHitobjectHighlight : StoryboardObjectGenerator
    {
        [Configurable]
        public int StartTime = 0;

        [Configurable]
        public int EndTime = 0;

        [Configurable]
        public int BeatDivisor = 8;

        [Configurable]
        public int FadeDuration = 200;

        public override void Generate()
        {
            var hitobjectLayer = GetLayer("Highlight");
            var hitobjects = Beatmap.HitObjects.ToList();
            for (int i=0; i<hitobjects.Count; i++)
            {
                var hitobject = hitobjects[i];
                if ((StartTime != 0 || EndTime != 0) && 
                    (hitobject.StartTime < StartTime - 5 || EndTime - 5 <= hitobject.StartTime))
                    continue;

                var hSprite = hitobjectLayer.CreateSprite("sb/p.png", OsbOrigin.TopCentre, new Vector2(hitobject.Position.X, 0));
                hSprite.ScaleVec(hitobject.StartTime, 48, 540);
                hSprite.Fade(OsbEasing.In, hitobject.StartTime, hitobject.EndTime + FadeDuration, 1, 0);
                hSprite.Additive(hitobject.StartTime, hitobject.EndTime + FadeDuration);
                hSprite.Color(hitobject.StartTime, Color4.Red);

                if (hitobject is OsuSlider)
                {
                    var timestep = Beatmap.GetTimingPointAt((int)hitobject.StartTime).BeatDuration / BeatDivisor;
                    var startTime = hitobject.StartTime;
                    while (true)
                    {
                        var endTime = startTime + timestep;

                        var complete = hitobject.EndTime - endTime < 5;
                        if (complete) endTime = hitobject.EndTime;

                        var startPosition = hSprite.PositionAt(startTime);
                        hSprite.MoveX(startTime, endTime, startPosition.X, hitobject.PositionAtTime(endTime).X);

                        if (complete) break;
                        startTime += timestep;
                    }
                }
            }
        }
    }
}
