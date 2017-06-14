using System;
using BeMyEyesApp.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Button), typeof(WordWrapButtonRenderer))]
namespace BeMyEyesApp.iOS
{
	public class WordWrapButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);
            if (Control != null)
            {
                Control.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
            }
		}
	}
}
