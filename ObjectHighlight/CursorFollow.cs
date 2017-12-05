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
    public class CursorFollow : StoryboardObjectGenerator
    {
        [Configurable]
        public string SpritePath = "sb/h.png";

        [Configurable]
        public int StartTime = 0;

        [Configurable]
        public int EndTime = 10000;

        [Configurable]
        public int BeatDivisor = 16;

        [Configurable]
        public int FadeTime = 200;

        [Configurable]
        public double Fade = 0.8;

        [Configurable]
        public double Scale = 1;

        [Configurable]
        public bool Additive = true;

        [Configurable]
        public Color4 Color = Color4.White;

        public override void Generate()
        {
		    var CursorLayer = GetLayer("");
            var Cursor = CursorLayer.CreateSprite(SpritePath,OsbOrigin.Centre,new Vector2(320,240));
            Cursor.Fade(OsbEasing.None, StartTime + FadeTime, StartTime, 0, Fade);
            Cursor.Fade(OsbEasing.None, EndTime, EndTime + FadeTime, Fade, 0);
            Cursor.Scale(StartTime, Scale);
            Cursor.Color(StartTime, Color);
            var beat = Beatmap.GetTimingPointAt(StartTime);
            var BeatDived = beat.Bpm / BeatDivisor;
            OsuHitObject previousHitobject = null;
            foreach (var currentHitobject in Beatmap.HitObjects)
            {
                if(currentHitobject.StartTime >= StartTime && currentHitobject.StartTime <= EndTime)
                {
                  if(previousHitobject == null)
                 {
                     var I = currentHitobject.StartTime;
                     var beforeI = currentHitobject.StartTime;
                     Cursor.Move(OsbEasing.None,beforeI,I,currentHitobject.Position,currentHitobject.Position);
                    if(currentHitobject is OsuSlider)
                    {
                    var timestep = Beatmap.GetTimingPointAt((int)currentHitobject.StartTime).BeatDuration / BeatDivisor;
                    var startTime = currentHitobject.StartTime;
                    for(var i = currentHitobject.StartTime; i < currentHitobject.EndTime; i += timestep)
                    {
                     var endTime = i + timestep;
                     var startPosition = Cursor.PositionAt(i);
                     Cursor.Move(i, endTime, startPosition, currentHitobject.PositionAtTime(endTime));
                    }

                    if (Additive)
                    {
                    Cursor.Additive(StartTime, EndTime);
                    }
                 }
                 }
                 if (previousHitobject != null)
                {
                    var I = currentHitobject.StartTime;
                    if(previousHitobject is OsuSlider){
                    var beforeI = previousHitobject.EndTime;
                    Cursor.Move(OsbEasing.None,beforeI,I,previousHitobject.EndPosition,currentHitobject.Position);

                }else if(previousHitobject is OsuCircle){
                        var beforeI = previousHitobject.StartTime;
                        Cursor.Move(OsbEasing.None,beforeI,I,previousHitobject.Position,currentHitobject.Position);
                    }

                    if (currentHitobject is OsuSlider)
                    {
                    var timestep = Beatmap.GetTimingPointAt((int)currentHitobject.StartTime).BeatDuration / BeatDivisor;
                    var startTime = currentHitobject.StartTime;
                    for(var i = currentHitobject.StartTime; i < currentHitobject.EndTime; i += timestep)
                    {
                     var endTime = i + timestep;
                     var startPosition = Cursor.PositionAt(i);
                     Cursor.Move(i, endTime, startPosition, currentHitobject.PositionAtTime(endTime));
                    }
                }

                 }

                 previousHitobject = currentHitobject;
                }
            }
        }
    }
}




