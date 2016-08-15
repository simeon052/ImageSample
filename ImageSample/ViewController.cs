using System;

using AppKit;
using Foundation;
using static Lib.Mac.ImageConvert;
using System.Threading.Tasks;
using System.Collections.Generic;

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
			List<string> srcList = new List<string> { @"/Users/matsussa/Documents/cat1.png", @"/Users/matsussa/Documents/cat2.jpg" };
			await Convert(srcList, ImageType.JPG).ConfigureAwait(false);
		}

		partial void BrowsePressed(NSObject sender)
		{

		}
	}
}
