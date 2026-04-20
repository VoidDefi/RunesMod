using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using Terraria;

namespace RunesMod.UI.Elements
{
    public struct ConcentrationBarGradient
    {
        public Color Highlight { get; private set; }

        public Color HalfShadow { get; private set; }

        public Color Shadow { get; private set; }

        public Color Border { get; private set; }

        public Color this[int index]
        {
            get
            {
                index = (index % 4 + 4) % 4;

                return index switch
                {
                    0 => Highlight,
                    1 => HalfShadow,
                    2 => Shadow,
                    3 => Border,
                };
            }
        }
        public ConcentrationBarGradient(Color highlight, Color halfShadow, Color shadow, Color border)
        {
            Highlight = highlight;
            HalfShadow = halfShadow;
            Shadow = shadow;
            Border = border;
        }

        public ConcentrationBarGradient(Color color, float step)
        {
            step = Math.Clamp(step, 0, 0.5f);

            Vector3 hsl = Main.rgbToHsl(color);

            hsl.Z += step;
            hsl.Z = Math.Clamp(hsl.Z, 0, 1);
            Highlight = Main.hslToRgb(hsl);

            hsl.Z -= step;
            hsl.Z = Math.Clamp(hsl.Z, 0, 1);
            HalfShadow = Main.hslToRgb(hsl);

            hsl.Z -= step;
            hsl.Z = Math.Clamp(hsl.Z, 0, 1);
            Shadow = Main.hslToRgb(hsl);

            hsl.Z -= step;
            hsl.Z = Math.Clamp(hsl.Z, 0, 1);
            Border = Main.hslToRgb(hsl);
        }
    }
}
