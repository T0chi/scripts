using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Animations;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class SpectrumFake : StoryboardObjectGenerator
    {
        public enum Style { Break, Break2, Intro, Intro2, Hook, NormalVerse, BeforeKiai, Kiai, Kiai2, Kiai3, Kiai4, AfterKiai, Outro, Outro2 }

        [Configurable]
        public OsbOrigin BarOrigin = OsbOrigin.Centre;

        [Configurable]
        public string Path = "sb/bar.png";

        [Configurable]
        public Style Section = Style.Kiai2;

        [Configurable]
        public int DrumHit_BeatDuration = 1;

        [Configurable]
        public int StandbyHeight_BeatDuration = 2;

        [Configurable]
        public int BarCount = 36;

        [Configurable]
        public int WaveIntensity = 8;

        [Configurable]
        public float PosX = 320;

        [Configurable]
        public float PosY = 240;

        [Configurable]
        public float Width = 930;

        [Configurable]
        public float StandbyHeight = 0.5f;

        [Configurable]
        public bool BarFlip = true;

        [Configurable]
        public bool VibrationTweening = false;

        [Configurable]
        public float VibrationAmount = 0.05f;

        [Configurable]
        public float EarlyScaleX = 0.3f;
        
        [Configurable]
        public float EarlyScaleY = 0.5f;

        [Configurable]
        public int EarlyScaleYDelay = 250;

        [Configurable]
        public float ScaleX = 0.5f;

        [Configurable]
        public float MinScaleY = 1;

        [Configurable]
        public float MaxScaleY = 3;

        [Configurable]
        public float Fade = 0.8f;

        [Configurable]
        public int FadeTime = 200;

        [Configurable]
        public bool Additive = true;

        [Configurable]
        public bool UseHSLColor = false;

        [Configurable]
        public Vector2 HSLColorWheel = new Vector2(0, 360);

        [Configurable]
        public double Saturation = 0.5f;

        [Configurable]
        public double Brightness = 1f;

        [Configurable]
        public Color4 barColor = Color4.White;

        // The "public override void Generate()" is used to store the timings for the individual sections in the map,
        // and also for each DrumHits (DrumHits are the reactions the spectrum makes on the drums).
        
        // Each "if" is described as a "section" with the help of the enum at the top of this script.


        // This is the database for each sections
        public override void Generate()
        {
            // Delete the "/*" and "*/" and create/add a new Section by inserting the Enum name in the "InsertEnumHere"
            
            /*

            if (Section == Style.InsertEnumHere)
            {

            }
            
            */
            
            if (Section == Style.Kiai)
            {
                var KiaiTimeStart = 89225;
                var KiaiTimeEnd = 116812;
                var RealKiaiEnding = 205087;
                var Start = KiaiTimeStart;
                var End = KiaiTimeEnd;

                var Beat = Beatmap.GetTimingPointAt((int)Start).BeatDuration;
                var DrumHitDuration = Beat * DrumHit_BeatDuration;
                var StandbyHeightDuration = Beat * StandbyHeight_BeatDuration;

                // RepeatPattern is for repeating the desired pattern when you reach half of the kiai time.
                var RepeatPattern = ((float)RealKiaiEnding - (float)KiaiTimeStart) / 2;

                var DrumHits = new float[]{
                    // First kiai half DrumHit pattern
                    89225, 90605, 91984,
                    93363, 94053, 94743,
                    95432, 96122, 96812,
                    97501, 98881,
                    
                    // Second kiai half DrumHit pattern
                    89225 + RepeatPattern, 90605 + RepeatPattern, 91984 + RepeatPattern,
                    93363 + RepeatPattern, 94053 + RepeatPattern, 94743 + RepeatPattern,
                    95432 + RepeatPattern, 96122 + RepeatPattern, 96812 + RepeatPattern,
                    97501 + RepeatPattern, 98881 + RepeatPattern };

                var StandbyHeightTimes = new float[]{
                    // First kiai half StandbyHeight pattern
                    89225 + (int)StandbyHeightDuration, 90605 + (int)StandbyHeightDuration, 91984 + (int)StandbyHeightDuration,
                    93363 + (int)StandbyHeightDuration, 94053 + (int)StandbyHeightDuration, 94743 + (int)StandbyHeightDuration,
                    95432 + (int)StandbyHeightDuration, 96122 + (int)StandbyHeightDuration, 96812 + (int)StandbyHeightDuration,
                    97501 + (int)StandbyHeightDuration, 98881 + (int)StandbyHeightDuration,
                    
                    // Second kiai half StandbyHeight pattern
                    89225 + (int)StandbyHeightDuration + RepeatPattern, 90605 + (int)StandbyHeightDuration + RepeatPattern, 91984 + (int)StandbyHeightDuration + RepeatPattern,
                    93363 + (int)StandbyHeightDuration + RepeatPattern, 94053 + (int)StandbyHeightDuration + RepeatPattern, 94743 + (int)StandbyHeightDuration + RepeatPattern,
                    95432 + (int)StandbyHeightDuration + RepeatPattern, 96122 + (int)StandbyHeightDuration + RepeatPattern, 96812 + (int)StandbyHeightDuration + RepeatPattern,
                    97501 + (int)StandbyHeightDuration + RepeatPattern, 98881 + (int)StandbyHeightDuration + RepeatPattern,
                    
                    // The part after kiai, and before the next section (you could call it "kiaiEnding") 
                    116122 };

                // If you ever want to enable the vibration effect at some places, then you can use this array to add them
                var CustomVibrationTimes = new int[]{
                    115432 };

                DrumHitsTimings(Start, End, DrumHitDuration, StandbyHeightDuration,
                            DrumHits, StandbyHeightTimes, CustomVibrationTimes);
            }

/*---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- */

            if (Section == Style.Kiai2)
            {
                var KiaiTimeStart = 183018;
                var KiaiTimeEnd = 210605;
                var RealKiaiEnding = 205087;
                var Start = KiaiTimeStart;
                var End = KiaiTimeEnd;

                var Beat = Beatmap.GetTimingPointAt((int)Start).BeatDuration;
                var DrumHitDuration = Beat * DrumHit_BeatDuration;
                var StandbyHeightDuration = (float)Beat * (float)StandbyHeight_BeatDuration;

                // RepeatPattern is for repeating the desired pattern when you reach half of the kiai time.
                var RepeatPattern = ((float)RealKiaiEnding - (float)KiaiTimeStart) / 2;

                var DrumHits = new float[]{
                    // First kiai half DrumHit pattern
                    183018, 184398, 185777,
                    187156, 187846, 188536,
                    189225, 189915, 190605,
                    191294, 192674,

                    // Second kiai half DrumHit pattern
                    183018 + RepeatPattern, 184398 + RepeatPattern, 185777 + RepeatPattern,
                    187156 + RepeatPattern, 187846 + RepeatPattern, 188536 + RepeatPattern,
                    189225 + RepeatPattern, 189915 + RepeatPattern, 190605 + RepeatPattern,
                    191294 + RepeatPattern, 192674 + RepeatPattern,
                    204398,
                    
                    // The part after kiai, and before the next section (you could call it "kiaiEnding") 
                    205087, 205777, 206467,
                    207156, 207846, 208536 };

                var StandbyHeightTimes = new float[]{
                    // First kiai half StandbyHeight pattern
                    183018 + StandbyHeightDuration, 184398 + StandbyHeightDuration, 185777 + StandbyHeightDuration,
                    191294 + StandbyHeightDuration, 192674 + StandbyHeightDuration,

                    // Second kiai half StandbyHeight pattern
                    183018 + StandbyHeightDuration + RepeatPattern, 184398 + StandbyHeightDuration + RepeatPattern, 185777 + StandbyHeightDuration + RepeatPattern,
                    191294 + StandbyHeightDuration + RepeatPattern,
                    
                    // The part after kiai, and before the next section (you could call it "kiaiEnding") 
                    209915 };

                // If you ever want to enable the vibration effect at some places, then you can use this array to add them
                var CustomVibrationTimes = new int[]{
                    209225 };

                DrumHitsTimings(Start, End, DrumHitDuration, StandbyHeightDuration,
                                DrumHits, StandbyHeightTimes, CustomVibrationTimes);
            }

/*---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- */

            if (Section == Style.Kiai3)
            {
                var KiaiTimeStart = 287846;
                var KiaiTimeEnd = 304398;
                var RealKiaiEnding = 205087;
                var Start = KiaiTimeStart;
                var End = KiaiTimeEnd;

                var Beat = Beatmap.GetTimingPointAt((int)Start).BeatDuration;
                var DrumHitDuration = Beat * DrumHit_BeatDuration;
                var StandbyHeightDuration = Beat * StandbyHeight_BeatDuration;

                // RepeatPattern is for repeating the desired pattern when you reach half of the kiai time.
                var RepeatPattern = ((float)RealKiaiEnding - (float)KiaiTimeStart) / 2;

                var DrumHits = new float[]{
                    // First kiai half DrumHit pattern
                    183018, 184398, 185777,
                    187156, 187846, 188536,
                    189225, 189915, 190605,
                    191294, 192674,

                    // Second kiai half DrumHit pattern
                    183018 + RepeatPattern, 184398 + RepeatPattern, 185777 + RepeatPattern,
                    187156 + RepeatPattern, 187846 + RepeatPattern, 188536 + RepeatPattern,
                    189225 + RepeatPattern, 189915 + RepeatPattern, 190605 + RepeatPattern,
                    191294 + RepeatPattern, 192674 + RepeatPattern,
                    
                    // The part after kiai, and before the next section (you could call it "kiaiEnding") 
                    205087, 205777, 206467,
                    207156, 207846, 208536 };

                var StandbyHeightTimes = new float[]{
                    // First kiai half StandbyHeight pattern
                    183018 + (int)StandbyHeightDuration, 184398 + (int)StandbyHeightDuration, 185777 + (int)StandbyHeightDuration,
                    187156 + (int)StandbyHeightDuration, 187846 + (int)StandbyHeightDuration, 188536 + (int)StandbyHeightDuration,
                    189225 + (int)StandbyHeightDuration, 189915 + (int)StandbyHeightDuration, 190605 + (int)StandbyHeightDuration,
                    191294 + (int)StandbyHeightDuration, 192674 + (int)StandbyHeightDuration,

                    // Second kiai half StandbyHeight pattern
                    183018 + (int)StandbyHeightDuration + RepeatPattern, 184398 + (int)StandbyHeightDuration + RepeatPattern, 185777 + (int)StandbyHeightDuration + RepeatPattern,
                    187156 + (int)StandbyHeightDuration + RepeatPattern, 187846 + (int)StandbyHeightDuration + RepeatPattern, 188536 + (int)StandbyHeightDuration + RepeatPattern,
                    189225 + (int)StandbyHeightDuration + RepeatPattern, 189915 + (int)StandbyHeightDuration + RepeatPattern, 190605 + (int)StandbyHeightDuration + RepeatPattern,
                    191294 + (int)StandbyHeightDuration + RepeatPattern, 192674 + (int)StandbyHeightDuration + RepeatPattern,
                    
                    // The part after kiai, and before the next section (you could call it "kiaiEnding") 
                    303708 };

                // If you ever want to enable the vibration effect at some places, then you can use this array to add them
                var CustomVibrationTimes = new int[]{
                    303018 };

                DrumHitsTimings(Start, End, DrumHitDuration, StandbyHeightDuration,
                            DrumHits, StandbyHeightTimes, CustomVibrationTimes);
            }

            /*

            if (Section == Style.InsertEnumHere)
            {
                
            }

            */
        }

/*---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- */

        // This is the database for the spectrum effects
        public void DrumHitsTimings(int Start, int End, double DrumHitDuration, double StandbyHeightDuration,
                                    float[] DrumHits, float[] StandbyHeightTimes,  int[] CustomVibrationTimes)
        {
            var Beat = Beatmap.GetTimingPointAt((int)Start).BeatDuration;
            float xPosition = -107 + PosX - 320;

            double FadeStart = Start;
            double FadeEnd = End;
            double Scale = Start;
            double WaveDelay = 0;
            double StartTime;

            for (int i = 0; i < BarCount; i++)
            {
                var bar = GetLayer("").CreateSprite(Path, BarOrigin);
                var BarWidth = Width / BarCount;

                bar.Color(Start, Color4.Gray);
                bar.Fade(FadeStart, FadeStart + FadeTime, 0, Fade);
                bar.Fade(FadeStart + FadeTime, FadeEnd - FadeTime, Fade, Fade);
                bar.Fade(FadeEnd - FadeTime, FadeEnd, Fade, 0);

                if (Additive)
                {
                    bar.Additive(Start, End + FadeTime);
                }

                if (UseHSLColor)
                {
                    bar.ColorHsb(Start, (i * (HSLColorWheel.Y - HSLColorWheel.X) / BarCount) + HSLColorWheel.X, Saturation, Brightness);
                }

                else
                {
                    bar.Color(Start, barColor);
                }


                var timeStep = Beat * 4;
                var shakeDelay = timeStep / 20f * 2;
                var xScale = ScaleX;

                // Database for the StandbyHeight reactions
                for (var h = 0; h < StandbyHeightTimes.Length; h++)
                {
                    var hitTime = StandbyHeightTimes[h];
                    bar.ScaleVec(hitTime, hitTime + StandbyHeightDuration, xScale, StandbyHeight, xScale, StandbyHeight);
                }

                // Database for the controlled random scale reactions
                if (BarFlip)
                {
                    foreach (var hitTime in DrumHits)
                    {
                        var RandomMinScaleY = Random(-MinScaleY, MinScaleY);

                        float RandomY = (float)Random(RandomMinScaleY, MaxScaleY);
                        float EarlyRandomY2 = (float)Random(RandomMinScaleY + EarlyScaleY, MaxScaleY + EarlyScaleY);

                        bar.ScaleVec(hitTime, hitTime + EarlyScaleYDelay, xScale + EarlyScaleX, EarlyRandomY2, xScale, RandomY);
                        bar.ScaleVec(OsbEasing.OutBack, hitTime + EarlyScaleYDelay, hitTime + EarlyScaleYDelay, xScale, RandomY, xScale, RandomY);

                        // VibrationEffect
                        for (var t = hitTime + EarlyScaleYDelay; t < hitTime + Beat * 2; t += (float)shakeDelay)
                        {
                            if (!VibrationTweening)
                            {
                                bar.ScaleVec(t, xScale + Random(0, VibrationAmount), RandomY + Random(0, VibrationAmount));
                                bar.ScaleVec(t + shakeDelay / 2f, xScale, RandomY);
                            }
                            else
                            {
                                bar.ScaleVec(t, t + shakeDelay / 2f, bar.ScaleAt(t).X, bar.ScaleAt(t).Y, xScale + Random(0, VibrationAmount), RandomY + Random(0, VibrationAmount));
                                bar.ScaleVec(t + shakeDelay / 2f, t + shakeDelay, bar.ScaleAt(t).X, bar.ScaleAt(t).Y, xScale, RandomY);
                            }
                        }
                    }
                }

                else
                {
                    foreach (var hitTime in DrumHits)
                    {
                        float RandomY = (float)Random(MinScaleY, MaxScaleY);
                        float EarlyRandomY2 = (float)Random(MinScaleY + EarlyScaleY, MaxScaleY + EarlyScaleY);

                        bar.ScaleVec(hitTime, hitTime + EarlyScaleYDelay, xScale + EarlyScaleX, EarlyRandomY2, xScale, RandomY);
                        bar.ScaleVec(OsbEasing.OutBack, hitTime + EarlyScaleYDelay, hitTime + EarlyScaleYDelay, xScale, RandomY, xScale, RandomY);

                        // VibrationEffect
                        for (var t = hitTime + EarlyScaleYDelay; t < hitTime + timeStep; t += (float)shakeDelay)
                        {
                            if (!VibrationTweening)
                            {
                                bar.ScaleVec(t, xScale + Random(0, VibrationAmount), RandomY + Random(0, VibrationAmount));
                                bar.ScaleVec(t + shakeDelay / 2f, xScale, RandomY);
                            }
                            else
                            {
                                bar.ScaleVec(t, t + shakeDelay / 2f, bar.ScaleAt(t).X, bar.ScaleAt(t).Y, xScale + Random(0, VibrationAmount), RandomY + Random(0, VibrationAmount));
                                bar.ScaleVec(t + shakeDelay / 2f, t + shakeDelay, bar.ScaleAt(t).X, bar.ScaleAt(t).Y, xScale, RandomY);
                            }
                        }
                    }
                }

                // The wave effect
                for (double i3 = Start; i3 < End; i3 += Beat * 8)
                {
                    StartTime = i3 + WaveDelay - 500;
                    bar.MoveX(OsbEasing.InOutSine, StartTime, StartTime + Beat * 4, xPosition + 20, xPosition - 20);
                    bar.MoveX(OsbEasing.InOutSine, StartTime + Beat * 4, StartTime + Beat * 8, xPosition - 20, xPosition + 20);
                    bar.MoveY(OsbEasing.InOutSine, StartTime + Beat * 2, StartTime + Beat * 6, PosY + 20, PosY - 20);
                    bar.MoveY(OsbEasing.InOutSine, StartTime + Beat * 6, StartTime + Beat * 10, PosY - 20, PosY + 20);
                }

                Scale += Beat / 32;
                xPosition += BarWidth;
                FadeStart += Beat / 32;
                WaveDelay += Beat / WaveIntensity;
            }
        }
    }
}
