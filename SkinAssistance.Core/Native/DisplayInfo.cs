using System.Windows;

namespace SkinAssistance.Core.Native
{
	public class DisplayInfo
	{
		public double DPIX { get; private set; }
		public double DPIY { get; private set; }

		public void UpdateDisplayInfo(DependencyObject @object)
		{
			var source = PresentationSource.FromDependencyObject(@object).CompositionTarget.TransformToDevice;
			DPIX = source.M11;
			DPIY = source.M22;
		}



		public double FullScreenWidth
		{
			get { return SystemParameters.FullPrimaryScreenWidth; }
		}

		public double FullScreenHeight
		{
			get
			{
				return SystemParameters.FullPrimaryScreenHeight;
			}
		}
	}
}