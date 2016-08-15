using System;

using AppKit;
using Foundation;
using Lib.Mac;
using System.Threading.Tasks;

namespace ImageSample
{
	public partial class ViewController : NSViewController
	{
		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Do any additional setup after loading the view.
		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}

		async partial void  ConvertPressed(NSObject sender)
		{
			await ImageConvert.Convert(@"/Users/matsussa/Documents/cat2.jpg", "/Users/matsussa/Documents/cat2.jpg.png").ConfigureAwait(false);
		}

		partial void BrowsePressed(NSObject sender)
		{

		}
	}
}
