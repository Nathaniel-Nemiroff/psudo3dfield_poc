using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tank
{
    class entity
    {
        Vector2 pos;
        float scale;
        Vector2 drawpos;
        bool wasdrawn;
        float griddisplacement;//used for horizon calculations

        Texture2D texture;
        Vector2 hitbox;//use this for collision and drawing?

        public entity(Texture2D t)
        {
            pos = new Vector2(200, 200);
            griddisplacement = 0;
            texture = t;
            wasdrawn = false;
        }
        public entity(Texture2D t, Vector2 p)
        {
            pos = p;
            griddisplacement = p.Y % 50;// add in a parameter to replace 50 with something relevant
            texture = t;
            wasdrawn = false;
        }
        public Vector2 getPos() { return pos; }
        public Vector2 getBnd() { return new Vector2(texture.Width/2, texture.Height/2); }//bounds of sprite

        public void update(Vector2 pup, Vector2 up)
        {
            pos.Y += up.Y + pup.Y;

            float prevscale = scale;
            scale = TileFactory.getScale(pos);
            if (prevscale > 0 && scale > 0)
                pos.X = (((pos.X-400) / prevscale) * scale +400);
            pos.X += up.X * scale + pup.X * scale;
            drawpos = new Vector2(pos.X, TileFactory.getDrawY(pos.Y));
        }
        public void resetdraw() { wasdrawn = false; }
        public void draw(SpriteBatch sb)
        {
            if(!wasdrawn)
                sb.Draw(texture, drawpos, null, new Color(255, 255, 255, 255), 0, 
                    new Vector2(texture.Width/2, texture.Height), 
                    scale/**TileFactory.getGlobalScale()*/, 0, 0);
            wasdrawn = true;
        }
    }
}
