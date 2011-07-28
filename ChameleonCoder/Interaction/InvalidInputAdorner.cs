using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ChameleonCoder.Interaction
{
    public class InvalidInputAdorner : Adorner
    {
        protected InvalidInputAdorner(UIElement element) : base(element) { }

        protected GeometryGroup ad;

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            ad = new GeometryGroup();
            ad.Children.Add(new RectangleGeometry(new Rect(AdornedElement.RenderSize)) { });
            drawingContext.DrawGeometry(null, new Pen(Brushes.Red, 1), ad);
            (AdornedElement as FrameworkElement).ToolTip = ToolTip;
        }

        public static InvalidInputAdorner Adorn(FrameworkElement element, string message)
        {
            var layer = AdornerLayer.GetAdornerLayer(element);
            InvalidInputAdorner adorner = new InvalidInputAdorner(element) { ToolTip = message };
            layer.Add(adorner);
            return adorner;
        }

        public void Remove()
        {
            AdornerLayer.GetAdornerLayer(AdornedElement).Remove(this);
        }
    }
}
