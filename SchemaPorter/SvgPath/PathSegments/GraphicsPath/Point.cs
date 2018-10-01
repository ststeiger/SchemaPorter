// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;


namespace Svg.Drawing.Primitives 
{
    /// <summary>
    ///    Represents an ordered pair of x and y coordinates that
    ///    define a point in a two-dimensional plane.
    /// </summary>
    [System.Serializable]
    public struct Point 
        : System.IEquatable<Point>
    {
        /// <summary>
        ///    Creates a new instance of the <see cref='System.Drawing.Point'/> class
        ///    with member data left uninitialized.
        /// </summary>
        public static readonly Point Empty = new Point();

        private int x; // Do not rename (binary serialization)
        private int y; // Do not rename (binary serialization)

        /// <summary>
        ///    Initializes a new instance of the <see cref='System.Drawing.Point'/> class
        ///    with the specified coordinates.
        /// </summary>
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        /// <summary>
        ///    Initializes a new instance of the Point class using
        ///    coordinates specified by an integer value.
        /// </summary>
        public Point(int dw)
        {
            x = LowInt16(dw);
            y = HighInt16(dw);
        }

        /// <summary>
        ///    <para>
        ///       Gets a value indicating whether this <see cref='System.Drawing.Point'/> is empty.
        ///    </para>
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty => x == 0 && y == 0;

        /// <summary>
        ///    Gets the x-coordinate of this <see cref='System.Drawing.Point'/>.
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        ///    <para>
        ///       Gets the y-coordinate of this <see cref='System.Drawing.Point'/>.
        ///    </para>
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        ///    <para>
        ///       Creates a <see cref='System.Drawing.PointF'/> with the coordinates of the specified
        ///    <see cref='System.Drawing.Point'/> 
        /// </para>
        /// </summary>
        public static implicit operator PointF(Point p) => new PointF(p.X, p.Y);


        /// <summary>
        ///    <para>
        ///       Compares two <see cref='System.Drawing.Point'/> objects. The result specifies
        ///       whether the values of the <see cref='System.Drawing.Point.X'/> and <see cref='System.Drawing.Point.Y'/> properties of the two <see cref='System.Drawing.Point'/>
        ///       objects are equal.
        ///    </para>
        /// </summary>
        public static bool operator ==(Point left, Point right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        ///    <para>
        ///       Compares two <see cref='System.Drawing.Point'/> objects. The result specifies whether the values
        ///       of the <see cref='System.Drawing.Point.X'/> or <see cref='System.Drawing.Point.Y'/> properties of the two
        ///    <see cref='System.Drawing.Point'/> 
        ///    objects are unequal.
        /// </para>
        /// </summary>
        public static bool operator !=(Point left, Point right) => !(left == right);

        /// <summary>
        ///   Converts a PointF to a Point by performing a ceiling operation on
        ///   all the coordinates.
        /// </summary>
        public static Point Ceiling(PointF value) => new Point(unchecked((int)System.Math.Ceiling(value.X)), unchecked((int)System.Math.Ceiling(value.Y)));

        /// <summary>
        ///   Converts a PointF to a Point by performing a truncate operation on
        ///   all the coordinates.
        /// </summary>
        public static Point Truncate(PointF value) => new Point(unchecked((int)value.X), unchecked((int)value.Y));

        /// <summary>
        ///   Converts a PointF to a Point by performing a round operation on
        ///   all the coordinates.
        /// </summary>
        public static Point Round(PointF value) => new Point(unchecked((int)System.Math.Round(value.X)), unchecked((int)System.Math.Round(value.Y)));

        /// <summary>
        ///    <para>
        ///       Specifies whether this <see cref='System.Drawing.Point'/> contains
        ///       the same coordinates as the specified <see cref='System.Object'/>.
        ///    </para>
        /// </summary>
        public override bool Equals(object obj) => obj is Point && Equals((Point)obj);

        public bool Equals(Point other) => this == other;


        private static int Combine(int h1, int h2)
        {
            unchecked
            {
                // RyuJIT optimizes this to use the ROL instruction
                // Related GitHub pull request: dotnet/coreclr#1830
                uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
                return ((int)rol5 + h1) ^ h2;
            }
        }

        /// <summary>
        ///    <para>
        ///       Returns a hash code.
        ///    </para>
        /// </summary>
        public override int GetHashCode() => Combine(X, Y);

        /// <summary>
        ///    Translates this <see cref='System.Drawing.Point'/> by the specified amount.
        /// </summary>
        public void Offset(int dx, int dy)
        {
            unchecked
            {
                X += dx;
                Y += dy;
            }
        }

        /// <summary>
        ///    Translates this <see cref='System.Drawing.Point'/> by the specified amount.
        /// </summary>
        public void Offset(Point p) => Offset(p.X, p.Y);

        /// <summary>
        ///    <para>
        ///       Converts this <see cref='System.Drawing.Point'/>
        ///       to a human readable
        ///       string.
        ///    </para>
        /// </summary>
        public override string ToString() => "{X=" + X.ToString() + ",Y=" + Y.ToString() + "}";

        private static short HighInt16(int n) => unchecked((short)((n >> 16) & 0xffff));

        private static short LowInt16(int n) => unchecked((short)(n & 0xffff));
    }
}
