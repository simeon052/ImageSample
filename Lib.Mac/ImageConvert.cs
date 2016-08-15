using System;
using AppKit;
using CoreGraphics;
using PCLStorage;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using ImageIO;
using MobileCoreServices;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Mac
{
	public class ImageConvert
	{
		public ImageConvert()
		{
		}
		public enum ImageType : int
		{
			JPG,
			PNG,
			BMP,
			TIF,
			PDF
		}


		public static async Task<bool> Convert(List<string> src, ImageType type)
		{
			string uttype;
			bool multipageSupport;
			switch (type)
			{
				case ImageType.JPG:
					uttype = UTType.JPEG;
					multipageSupport = false;
					break;
				case ImageType.BMP:
					uttype = UTType.BMP;
					multipageSupport = false;
					break;
				case ImageType.PNG:
					uttype = UTType.PNG;
					multipageSupport = false;
					break;
				case ImageType.TIF:
					uttype = UTType.TIFF;
					multipageSupport = true;
					break;
				case ImageType.PDF:
					uttype = UTType.PDF;
					multipageSupport = true;
					break;
				default:
					throw new ArgumentException();
			}
			if (!multipageSupport)
			{
				if (src.Count != 1)
				{
					throw new ArgumentException();
				}
			}


			string dist = src.FirstOrDefault() + "." + type.ToString();

			using (var url = NSUrl.FromFilename(dist))
			using (var dstImg = CGImageDestination.Create(url, uttype, src.Count))
			{
				foreach (var s in src)
				{
					var cgimage = await GetCGImage(s);
					dstImg?.AddImage(cgimage);
				}
				return dstImg?.Close() ?? false;
			}
		}

		private static async Task<CGImage> GetCGImage(string src)
		{
			IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(Path.GetDirectoryName(src));
			IFile file = await folder.GetFileAsync(Path.GetFileName(src));
			NSImage srcImg = NSImage.FromStream(await file.OpenAsync(PCLStorage.FileAccess.Read));
			var rect = new CGRect(0f, 0f, srcImg.Size.Width, srcImg.Size.Height);
			CGImage cgImage = srcImg.AsCGImage(ref rect, null, null);
			return cgImage;
		}
	
	}
	
}
