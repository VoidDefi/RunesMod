using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunesMod.Graphics
{
    public interface IUnitedDustDrawer
    {
        public virtual void PostDrawUnitedDusts(SpriteBatch spriteBatch, RenderTarget2D target)
        {

        }
    }
}
