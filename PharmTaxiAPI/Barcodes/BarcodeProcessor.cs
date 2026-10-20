using System.Drawing;
using ZXing;

namespace Core.Barcodes
{
	public class BarcodeProcessor
	{
		private BarcodeReader _reader;

		public BarcodeProcessor()
		{
			_reader = new BarcodeReader();


			//_reader.Options.CharacterSet = ;
			//_reader.Options.ReturnCodabarStartEnd = false;
			//_reader.Options.UseCode39ExtendedMode = ;
			//_reader.Options.UseCode39RelaxedExtendedMode = ;

			_reader.Options.Hints.Add(DecodeHintType.PURE_BARCODE, false);
			_reader.Options.Hints.Add(DecodeHintType.TRY_HARDER, true);
			_reader.Options.Hints.Add(DecodeHintType.TRY_HARDER_WITHOUT_ROTATION, true);
			_reader.Options.PureBarcode = false;
			_reader.Options.PossibleFormats = BarcodeInfo.SupportedFormats;
			_reader.Options.TryHarder = true;
			_reader.AutoRotate = true;
			_reader.TryInverted = true;
		}

		/// <summary>
		/// Чтение штрих-кода из файла
		/// </summary>
		/// <param name="fileName">Имя файла</param>
		/// <returns>Данные штрих-кода</returns>
		//public BarcodeInfo Read(string fileName)
		//{
		//	using (Bitmap barcodeBitmap = new Bitmap(fileName))
		//	{
		//		Result r = _reader.Decode(barcodeBitmap);
		//		return ((r == null) ? null : new BarcodeInfo(r));
		//	}
		//}
	}
}
