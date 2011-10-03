using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ChameleonCoder.Shared
{
    /// <summary>
    /// an adorner to show invalid user input
    /// </summary>
    public sealed class InvalidInputAdorner : Adorner
    {
        /// <summary>
        /// creates a new instance of the adorner, given an element
        /// </summary>
        /// <param name="element"></param>
        private InvalidInputAdorner(FrameworkElement element) : base(element) { }

        /// <summary>
        /// rendres the adorner
        /// </summary>
        /// <param name="drawingContext">the drawing context for this adorner</param>
        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            GeometryGroup ad = new GeometryGroup();
            ad.Children.Add(new RectangleGeometry(new Rect(AdornedElement.RenderSize)) { });
            drawingContext.DrawGeometry(null, new Pen(Brushes.Red, 1), ad);
            (AdornedElement as FrameworkElement).ToolTip = ToolTip;
        }

        /// <summary>
        /// adorns an element with a new InvalidInputAdorner
        /// </summary>
        /// <param name="element">the element to adorn</param>
        /// <param name="message">a Custom tooltip message</param>
        /// <returns>the new instance</returns>
        public static InvalidInputAdorner Adorn(FrameworkElement element, string message)
        {
            var layer = AdornerLayer.GetAdornerLayer(element);
            InvalidInputAdorner adorner = new InvalidInputAdorner(element) { ToolTip = message };
            layer.Add(adorner);
            return adorner;
        }

        /// <summary>
        /// removes the current instance from its element
        /// </summary>
        public void Remove()
        {
            AdornerLayer.GetAdornerLayer(AdornedElement).Remove(this);
        }
    }
}
