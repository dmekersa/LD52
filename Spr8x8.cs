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
        private string _lastDirection = "";
        private float _fadeInSpeed;
        public string state;
        public double stateTime;
        public bool stateDone { get; private set; }

        public Spr8x8(int pSpr, int pRow, int pCol) : base(pSpr, new Vector2(pCol * 8, pRow * 8))
        {
            SetMapPosition(pRow, pCol);
        }

        public void FadeIn(float pSpeed)
        {
            Alpha = 0;
            _fadeInSpeed = pSpeed;
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
            return _lastDirection;
        }

        public bool AtDestination()
        {
            return col == colDest && row == rowDest;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (stateTime > 0)
            {
                stateTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (stateTime <= 0)
                    stateDone = true;
            }
            else
                stateDone = false;

            if (_fadeInSpeed > 0 && Alpha < 1)
            {
                Alpha += _fadeInSpeed;
            }

            Vector2 currentPos = Position;
            if (col < colDest)
            {
                Move(speed, 0);
                _distanceX += speed;
                _lastDirection = "right";
                FlipX = false;
            }
            if (col > colDest)
            {
                Move(-speed, 0);
                _distanceX += speed;
                _lastDirection = "left";
                FlipX = true;
            }
            if (row < rowDest)
            {
                Move(0, speed);
                _distanceY += speed;
                _lastDirection = "down";
            }
            if (row > rowDest)
            {
                Move(0, -speed);
                _distanceY += speed;
                _lastDirection = "up";
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
                    SetMapPosition(rowDest, colDest);
                    _distanceX = 0;
                    _distanceY = 0;
                    justArrived = true;
                }
            }
        }
    }
}
