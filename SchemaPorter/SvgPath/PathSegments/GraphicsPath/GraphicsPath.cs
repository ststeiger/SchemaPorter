
namespace Svg.Drawing.Primitives 
{


    public class GraphicsPath
        : System.IDisposable
    {
        protected System.Collections.Generic.List<PointF> m_pathPoints;

        public GraphicsPath()
        {
            this.m_pathPoints = new System.Collections.Generic.List<PointF>();
        }
        
        public void AddLine(PointF start, PointF end)
        {
            this.m_pathPoints.Add(start);
            this.m_pathPoints.Add(end);
        }

        //public void AddBeziers(params Point[] points) { }
        public void AddBezier(params PointF[] points)
        {
            foreach (PointF thisPoint in points)
            {
                this.m_pathPoints.Add(thisPoint);
            }

        }

        public void AddBezier(params float[] points)
        {
            if (points.Length % 2 != 0)
                throw new System.ArgumentException("Points must be an even number of points (coordinate pairs).");

            for (int i = 0; i < points.Length; i += 2)
            {
                this.m_pathPoints.Add(new PointF(points[i], points[i + 1]));
            }

        }


        public void StartFigure()
        { }


        public void CloseFigure()
        { }


        public PathData PathData
        {
            get
            {
                PathData pd = new PathData();
                pd.Points = this.m_pathPoints.ToArray();

                return pd;
            }
        }


        public void Dispose()
        {
            if (this.m_pathPoints == null)
                return;

            this.m_pathPoints.Clear();
            this.m_pathPoints = null;
        }


    }


}
