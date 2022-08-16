namespace ColorConsole
{
    public struct Point2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point2D(int x, int y)
        {
          X = x; 
          Y = y;  
        }

        public Point2D((int x, int y) tuple)
        {
          X = tuple.x; 
          Y = tuple.y;  
        }

        public static implicit operator Point2D((int x, int y) tuple)
            => new(tuple.x, tuple.y);

        public static bool operator ==(Point2D left, Point2D right)
            => left.X == right.X && left.Y == right.Y;

        public static bool operator !=(Point2D left, Point2D right)
            => left.X != right.X && left.Y != right.Y;

        public static Point2D operator ++(Point2D p)
            => new(p.X + 1, p.Y + 1);
        
        public static Point2D operator +(Point2D left, Point2D right)
        {
            int x = left.X + right.X;
            int y = left.Y + right.Y;
            return new(x, y);
        }

        public static Point2D operator +(Point2D left, (int X, int Y) right)
        {
            int x = left.X + right.X;
            int y = left.Y + right.Y;
            return new(x, y);
        }

        public static Point2D operator --(Point2D p)
            => new(p.X - 1, p.Y - 1);
        
        public static Point2D operator -(Point2D left, Point2D right)
        {
            int x = left.X - right.X;
            int y = left.Y - right.Y;
            return new(x, y);
        }

        public static Point2D operator -(Point2D left, (int X, int Y) right)
        {
            int x = left.X - right.X;
            int y = left.Y - right.Y;
            return new(x, y);
        }

        public override bool Equals(object? obj)
        {
            return obj is Point2D d &&
                   X == d.X &&
                   Y == d.Y;
        }

        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}