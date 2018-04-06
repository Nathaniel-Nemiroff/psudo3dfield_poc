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
    class TileFactory
    {
        LinkedList<Tile> Ground;

        List<entity> entities;// might replace later, especially if I keep these in here

        static Vector2 Upperbound;
        static Vector2 Lowerbound;
        static float Horizon;
        static float GlobalScale;
                           
        public TileFactory(Texture2D t, Texture2D e, int height, int width)
        {
            Ground = new LinkedList<Tile>();
            entities = new List<entity>();

            Upperbound = new Vector2(0,   -50);
            Lowerbound = new Vector2(width, height+50);
            Horizon = 50;
            GlobalScale = 1;

            for (int i = -1; i < (height / 25) + 3; i++)
                Ground.AddLast(new Tile(t, new Vector2(width/2, 25 * i)));

            entities.Add(new entity(e, new Vector2(100, 200)));
            entities.Add(new entity(e, new Vector2(200, 190)));
            entities.Add(new entity(e, new Vector2(300, 180)));
            entities.Add(new entity(e, new Vector2(400, 170)));
            entities.Add(new entity(e, new Vector2(500, 160)));
            entities.Add(new entity(e, new Vector2(600, 150)));
        }
        //up=globalposition update, hup=horizon update, sup=scale update, eup=entityposition update
        public void Update(Vector2 up, int hup, float sup, Vector2 eup)
        {
            Horizon += hup;
            if (Horizon > 350) Horizon = 350;
            if (Horizon < 50) Horizon = 50;
            GlobalScale += sup;
            if (GlobalScale < .5f) GlobalScale = .5f;
            if (GlobalScale > 2f) GlobalScale = 2f;

            int requeueIndex = 0;

            foreach (Tile t in Ground)
            {
                requeueIndex += t.incpos(up);
            }

            if (requeueIndex > 0)
            {
                Tile t = Ground.First();
                Ground.RemoveFirst();
                Ground.AddLast(t);
            }
            if (requeueIndex < 0)
            {
                Tile t = Ground.Last();
                Ground.RemoveLast();
                Ground.AddFirst(t);
            }

            foreach (Tile t in Ground)
                t.update();

            foreach (entity e in entities)
                e.update(eup, up);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (entity e in entities)
                e.resetdraw();

            int skipt = 0;
            //int skipe = 0;
            foreach (entity e in entities)
                if (e.getPos().Y < Horizon)
                    e.draw(sb);

            foreach (Tile t in Ground)
            {
                if (t.getpos().Y > Horizon)
                    break;
                t.draw(sb);
                ++skipt;
            }
            
            foreach (entity e in entities)
            {
                if (e.getPos().Y > Horizon)
                    break;
                e.draw(sb);
            }
            
            foreach (Tile t in Ground.Skip<Tile>(skipt))
                t.draw(sb);
            
            foreach (entity e in entities)
                    e.draw(sb);
                    
        }



        public static float checkVerticalBounds(float pos)
        {
            if (pos > Lowerbound.Y)
                return Upperbound.Y;
            if (pos < Upperbound.Y)
                return Lowerbound.Y;
            return pos;
        }
        public static float checkHorizontalBounds(float pos)
        {
            if (pos > Lowerbound.X)
                return Upperbound.X;
            if (pos < Upperbound.X)
                return Lowerbound.X;

            return pos;
        }
        public static float getScale(Vector2 pos)
        {
            float retval = GlobalScale*(.5f * (pos.Y - Horizon) / (Lowerbound.Y - Horizon) + .5f);
            if (retval > 0) return retval;
            return 0;
        }
        public static float getGlobalScale() { return GlobalScale; }

        public static float getDrawY(float pos)
        {
            float calcval = pos;
            if (calcval < Horizon && calcval > Horizon - 50)//50 is the grid gap between tiles, not actual drawposition
                calcval = Horizon;
            else if (calcval < Horizon - 49)
                calcval = Math.Abs((calcval + 50) - Horizon) + Horizon;
            else calcval = Math.Abs(calcval - Horizon) + Horizon;
            return calcval *
             ((Lowerbound.Y - Horizon) / (Lowerbound.Y - Upperbound.Y)) + Horizon;
        }
    }
    class Tile
    {
        Vector2 pos;
        Vector2 drawpos;
        float scale;
        
        Texture2D texture;

        public Tile(Texture2D t)
        {
            texture = t;
            pos = new Vector2(0, 100);
        }
        public Tile(Texture2D t, Vector2 POS)
        {
            texture = t;
            pos = POS;
        }
        public void setpos(Vector2 POS) {pos = POS;}
        public Vector2 getpos() { return pos; }
        public Vector2 getDrawPos() { return drawpos; }
        
        public void update()
        {
            drawpos = pos;
            drawpos.Y = TileFactory.getDrawY(pos.Y);
            scale = TileFactory.getScale(pos);

            float posX = TileFactory.checkHorizontalBounds(pos.X);
            if (posX < pos.X) pos.X -= texture.Width;
            if (posX > pos.X) pos.X += texture.Width;
        }
        public int incpos(Vector2 newPOS)
        {
            pos += newPOS;

            newPOS.Y = TileFactory.checkVerticalBounds(pos.Y);

            int ret = 0;
            if (newPOS.Y > pos.Y) ret =  1;
            if (newPOS.Y < pos.Y) ret = -1;

            pos.Y = newPOS.Y;
            return ret;
        }
        public void draw(SpriteBatch sb)
        {//drawpos.X + i*(texture.Width * scale)
            for (int i = -4; i < 3; i++)
                sb.Draw(texture, new Vector2(400, drawpos.Y)
                    , null, new Color(255, 255, 255, 255), 0, new Vector2(-(drawpos.X+i * (texture.Width)), 0),
                    scale/**TileFactory.getGlobalScale()*/, 0, 0);
        }
    }
}
