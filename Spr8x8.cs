using Microsoft.Xna.Framework;

namespace LD52
{
    public class Spr8x8 : Sprite80
    {
        public int col { get; private set; }
        public int row { get; private set; }
        int colDest;
        int rowDest;
        public float speed = 0.5f;
        public bool moving { get; private set; }
        public bool justArrived { get; private set; }
        private float _distanceX;
        private float _distanceY;

        public Spr8x8(int pSpr, int pRow, int pCol) : base(pSpr, new Vector2(pCol * 8, pRow * 8))
        {
            row = pRow;
            rowDest = pRow;
            col = pCol;
            colDest = pCol;
        }

        public void SetMapPosition(int pRow, int pCol)
        {
            Position = new Vector2(pCol * 8, pRow * 8);
            moving = false;
            row = pRow;
            col = pCol;
            rowDest = pRow;
            colDest = pCol;

        }

        public void GoTo(int pRow, int pCol)
        {
            if (!moving)
            {
                rowDest = pRow;
                colDest = pCol;
                justArrived = false;
            }
        }

        public string getDirection()
        {
            if (col < colDest)
            {
                return "right";
            }
            if (col > colDest)
            {
                return "left";
            }
            if (row < rowDest)
            {
                return "down";
            }
            if (row > rowDest)
            {
                return "up";
            }
            return "";
        }

        public override void Update()
        {
            base.Update();

            Vector2 currentPos = Position;
            if (col < colDest)
            {
                Move(speed, 0);
                _distanceX += speed;
            }
            if (col > colDest)
            {
                Move(-speed, 0);
                _distanceX += speed;
            }
            if (row < rowDest)
            {
                Move(0, speed);
                _distanceY += speed;
            }
            if (row > rowDest)
            {
                Move(0, -speed);
                _distanceY += speed;
            }
            if (currentPos != Position)
            {
                moving = true;
                justArrived = false;
            }
            if (moving)
            {
                if (_distanceX >= 8 || _distanceY >= 8)
                {
                    moving = false;
                    col = colDest;
                    row = rowDest;
                    _distanceX = 0;
                    _distanceY = 0;
                    justArrived = true;
                }
            }
        }
    }
}
