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
    public class DeleteBackground : StoryboardObjectGenerator
    {
        [Configurable]
                public string Bg = "Bg.jpg";

        public override void Generate()
        {
		    var Sprite = GetLayer("Background").CreateSprite(Bg);
            Sprite.Fade(0, 0);
        }
    }
}
