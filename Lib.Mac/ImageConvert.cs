using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using Foundation;
using ImageIO;
using MobileCoreServices;

namespace Lib.Mac
{
	public class ImageConvert
	{
		private readonly NSObject owner;

		private ImageConvert()
		{
		}

		public ImageConvert(NSObject owner)
		{
			this.owner = owner;
		}

		public enum ImageType : int
		{
			JPG,
			PNG,
			BMP,
			TIF,
			PDF
		}


		public async Task<bool> Convert(List<string> src, ImageType type)
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

			var cgimages = new List<CGImage>();
			foreach (var s in src)
			{
				if (Path.GetExtension(s).ToUpper().Contains(ImageType.PDF.ToString()))
				{
					cgimages.AddRange(await GetCGImagesFromPDF(s));
				}
				else
				{
					cgimages.AddRange(await GetCGImages(s));
				}
			}
			if (!multipageSupport)
			{
				if (cgimages.Count != 1)
				{
					throw new ArgumentException();
				}
			}
			string dist = src.FirstOrDefault() + "." + type.ToString();

			using (var url = NSUrl.FromFilename(dist))
			using (var dstImg = CGImageDestination.Create(url, uttype, cgimages.Count))
			{
				foreach (var ci in cgimages)
				{
					dstImg?.AddImage(ci);
				}

				return dstImg?.Close() ?? false;
			}

		}

		private static async Task<List<CGImage>> GetCGImages(string src)
		{
			var cgimages = new List<CGImage>();
			await Task.Run(() =>
			{
				using (var cis = CGImageSource.FromUrl(NSUrl.FromFilename(src)))
				{
					for (int i = 0; i < cis.ImageCount; i++)
					{
						cgimages.Add(cis.CreateImage(i, new CGImageOptions()));
					}
				}
			});
			return cgimages;
		}

		private CGImage NSImage2CGImage(NSImage srcImg)
		{
			var rect = new CGRect(0f, 0f, srcImg.Size.Width, srcImg.Size.Height);
			return srcImg?.AsCGImage(ref rect, null, null) ?? null; 
		}

		private async Task<List<CGImage>> GetCGImagesFromPDF(string src)
		{

			var imageList = new List<CGImage>();
			await Task.Run(() =>
			{

				var doc = new PdfKit.PdfDocument(NSUrl.FromFilename(src));
				for (int n = 0; n < doc.PageCount; n++)
				{
					var page = doc.GetPage(n);
					var nsData = page.DataRepresentation;
					this.owner.InvokeOnMainThread(() =>
					{
						// NSPdfImageRepのConstructorはUI Threadで動作させる必要がある
						var pdfImageRep = new NSPdfImageRep(nsData);
						// HACK 画質の検証
						var nsImage = new NSImage(page.DataRepresentation);
						nsImage.Draw(new CGRect(0, 0, pdfImageRep.Size.Width, pdfImageRep.Size.Height));
						imageList.Add(NSImage2CGImage(nsImage));
					});
				}
			});
			return imageList;
		}

	}

}
