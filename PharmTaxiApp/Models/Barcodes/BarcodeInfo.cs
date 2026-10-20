//using Core.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ZXing;

namespace PharmTaxiApp.Models.Barcodes
{
	public class BarcodeInfo
	{
		public const char FNC1 = '\x1D';
		private static readonly char[] Brackets = { '(', ')', '[', ']' };

		private static readonly Dictionary<string, AI> _aiByStarterWithBrackets = AI.All
			.Select(i => new KeyValuePair<string, AI>(String.Format("({0})", i.Starter), i))
			.Concat(AI.All.Select(i => new KeyValuePair<string, AI>(String.Format("[{0}]", i.Starter), i)))
			.ToDictionary(p => p.Key, p => p.Value);

		private bool _wasReaded = false;
		private string _readedFormat = null;
		private Dictionary<AIType, string> _values = new Dictionary<AIType, string>();

		internal static readonly ZXing.BarcodeFormat[] SupportedFormats =
		{
			ZXing.BarcodeFormat.DATA_MATRIX,
			ZXing.BarcodeFormat.QR_CODE,
			ZXing.BarcodeFormat.CODE_128,
			ZXing.BarcodeFormat.EAN_13,
			ZXing.BarcodeFormat.EAN_8
		};

		/// <summary>
		/// Текст, содержащийся в штрих-коде
		/// </summary>
		public string PlainText { get; private set; }

		/// <summary>
		/// Форматы, которым соответствует штрих-код
		/// </summary>
		public string[] Formats { get; private set; }

		/// <summary>
		/// Байтовое наполнение штрих-кода
		/// </summary>
		public byte[] RawBytes { get; private set; }

		public long Timestamp { get; private set; }

		/// <summary>
		/// Определяет, было ли разобрано по полям содержимое штрих-кода
		/// </summary>
		public bool WasReaded { get { return (_wasReaded); } }

		/// <summary>
		/// Возвращает опознанный формат штрих-кода
		/// </summary>
		public string ReadedFormat { get { return (_readedFormat); } }

		/// <summary>
		/// Словарь полей и их значений, обнаруженных в штрих-коде
		/// </summary>
		public Dictionary<AIType, string> Values { get { return (_values); } }

		private string GetValueByCode(AIType type)
		{
			string val;
			return ((_values.TryGetValue(type, out val)) ? val : null);
		}

		/// <summary>
		/// Значение GTIN
		/// </summary>
		public string GTIN
		{
			get
			{
				return(GetValueByCode(AIType.GTIN));
			}
		}

		public string GTINTruncated
		{
			get
			{
				string gtin = GetValueByCode(AIType.GTIN);

				if (gtin != null && gtin.Length > 13)
				{
					gtin = gtin.Substring(gtin.Length - 13, 13);
					//int pos = 0;

					//while ((pos < gtin.Length) && (gtin[pos] == '0'))
					//	pos++;
					
					//gtin = gtin.Substring(pos);
				}

				return (gtin);
			}
		}
		
		/// <summary>
		/// Срок годности
		/// </summary>
		public DateTime? UseBy
		{
			get
			{
				DateTime vl;
				string exp = GetValueByCode(AIType.USE_BY_OR_EXPIRY);
				return (((exp != null) && DateTime.TryParse(exp, out vl)) ? (DateTime?)vl : (DateTime?)null);
			}
		}

		/// <summary>
		/// Номер партии
		/// </summary>
		public string Seria
		{ get { return (GetValueByCode(AIType.BATCH_LOT)); } }

		/// <summary>
		/// Уникальный номер упаковки
		/// </summary>
		public string Number
		{ get { return (GetValueByCode(AIType.SERIAL)); } }

		/// <summary>
		/// Проверка контрольной суммы GTIN
		/// </summary>
		/// <returns></returns>
		public bool? CheckGTINControlSum()
		{
			string gtin = GTIN;
			return ((gtin == null) ? (bool?)null : (bool?)EAN.CheckControlSum(gtin));
		}

		/// <summary>
		/// Проверка срока годности
		/// </summary>
		/// <returns></returns>
		public bool? CheckUseBy()
		{
			DateTime? useBy = UseBy;
			return ((useBy == null) ? (bool?)null : (bool?)(useBy.Value >= DateTime.Now.Date));
		}

		public bool? CheckGTINAsInternal()
		{
			string gtin = GTIN;
			return (String.IsNullOrEmpty(gtin) ? (bool?)null : (bool?)(!(gtin.StartsWith("2") || gtin.StartsWith("02"))));
		}

		#region Private reading methods

		private void ReadAiWithFNC1(string value)
		{
			for (int pos = 0; pos < value.Length; )
			{
				string starter = null;

				for (int starterLen = 4; starterLen >= 2; starterLen--)
				{
					starter = value.Substring(pos, starterLen);

					if (AI.ByStarters.ContainsKey(starter))
						break;
					else
						starter = null;
				}

				if (starter == null)
				{
					pos++;
					continue;
				}

				AI ai = AI.ByStarters[starter];
				pos += starter.Length;
				string strVal = null;

				if (ai.Length != null)
				{
					strVal = value.Substring(pos, ai.Length.Value);
					pos += ai.Length.Value;
				}
				else
				{
					int toEnd = value.Length - pos;
					int maxLen = Math.Min(ai.MaxLength ?? toEnd, toEnd);
					int ps = value.IndexOf(FNC1, pos, maxLen);

					if (ps >= 0)
					{
						strVal = value.Substring(pos, ps - pos);
						pos = ps + 1;
					}
					else
					{
						strVal = value.Substring(pos);
						pos = value.Length; // Чтобы выйти на этом витке
					}
				}

				if (ai.DecimalPlaces != null)
					strVal = AI.ApplyDecimalPlaces(strVal, ai.DecimalPlaces.Value);
				else if (ai.AsDateTime)
					strVal = AI.NormalizeDatetimeFormat(strVal);

				_values.Add(ai.Type, strVal);
			}
		}

		private void ReadAiWithBrackets(string value)
		{
			for (int pos = 0; pos < value.Length; )
			{
				string starter = null;

				for (int starterLen = 6; starterLen >= 4; starterLen--)
				{
					starter = value.Substring(pos, starterLen);

					if (_aiByStarterWithBrackets.ContainsKey(starter))
						break;
					else
						starter = null;
				}

				if (starter == null)
				{
					pos++;
					continue;
				}

				AI ai = _aiByStarterWithBrackets[starter];
				pos += starter.Length;
				string strVal = null;

				if (ai.Length != null)
				{
					strVal = value.Substring(pos, ai.Length.Value);
					pos += ai.Length.Value;
				}
				else
				{
					int toEnd = value.Length - pos;
					int maxLen = Math.Min(ai.MaxLength ?? toEnd, toEnd);
					int ps = value.IndexOfAny(Brackets, pos, maxLen);

					if (ps >= 0)
					{
						strVal = value.Substring(pos, ps - pos);
						pos = ps;
					}
					else
					{
						strVal = value.Substring(pos);
						pos = value.Length; // Чтобы выйти на этом витке
					}
				}

				if (ai.DecimalPlaces != null)
					strVal = AI.ApplyDecimalPlaces(strVal, ai.DecimalPlaces.Value);
				else if (ai.AsDateTime)
					strVal = AI.NormalizeDatetimeFormat(strVal);

				_values.Add(ai.Type, strVal);
			}
		}

		private void ReadAiWithoutSeparators(string value)
		{
			for (int pos = 0; pos < value.Length; )
			{
				string starter = null;

				for (int starterLen = 4; starterLen >= 2; starterLen--)
				{
					starter = value.Substring(pos, starterLen);

					if (AI.ByStarters.ContainsKey(starter))
						break;
					else
						starter = null;
				}

				if (starter == null)
				{
					pos++;
					continue;
				}

				AI ai = AI.ByStarters[starter];
				pos += starter.Length;
				string strVal = null;

				if (ai.Length != null)
				{
					try
					{
						strVal = value.Substring(pos, ai.Length.Value);
					}
					catch(IndexOutOfRangeException)
					{
						// Нифига не AI (длины кода не хватает, чтобы прочитать всё поле)
						_values.Clear();
						return;
					}
					pos += ai.Length.Value;
				}
				else // Это - последнее поле, читаем всё до конца
				{
					strVal = value.Substring(pos);
					pos = value.Length; // Чтобы выйти на этом витке

					if(((ai.MinLength != null) && (strVal.Length < ai.MinLength.Value))
						|| ((ai.MaxLength != null) && (strVal.Length > ai.MaxLength.Value)))
					{
						// Нифига не AI (длина поля не укладывается в ограничения для этого AI)
						_values.Clear();
						return;
					}
				}

				if (ai.DecimalPlaces != null)
					strVal = AI.ApplyDecimalPlaces(strVal, ai.DecimalPlaces.Value);
				else if (ai.AsDateTime)
					strVal = AI.NormalizeDatetimeFormat(strVal);

				_values.Add(ai.Type, strVal);
			}


			// Парсить пока не попадётся поле с переменной длиной.
			// Если такое попалось - считаем его последним и читаем его до конца кода
			// Для полей переменной длины:
			//	если длина получившегося значения не помещается в ограничения для данного AI
			//		- значит это нифига не данные AI
			// Для полей постоянной длины:
			//	если при чтении такого поля значение оборвалось на середине (неожиданный EOF, длина значения не совпадает с длиной AI)
			//		- значит это нифига не данные AI
		}

		private bool ReadAI(string value, bool maybeeNotAi)
		{
			if (value == String.Empty)
				return (false);
			else if (value[0] == FNC1)
			{
				if (value.IndexOfAny(Brackets, 1, Math.Min(6, value.Length - 1)) >= 0)
					ReadAiWithBrackets(value);
				else
					ReadAiWithFNC1(value);

				return (true);
			}
			else if (value.IndexOfAny(Brackets, 1, Math.Min(6, value.Length - 1)) >= 0)
			{
				ReadAiWithBrackets(value);
				return (_values.Count > 0);
			}
			else if (!maybeeNotAi && value[0] >= '0' && value[0] <= '9')
			{
				ReadAiWithoutSeparators(value);
				return (_values.Count > 0);
			}

			return (false);
		}

		private bool ReadFromChina128(string value)
		{
			if (value == null)
				throw new ArgumentNullException("value");
			if (value.Length != 20)
				return (false);

			string ean8 = value.Substring(0, 7);

			// Добавляем стандартную контрольную сумму к EAN-8
			byte cs = EAN.CalculateControlSum(ean8, 8);

			_values.Add(AIType.GTIN, String.Concat(ean8, cs));
			_values.Add(AIType.SERIAL, value.Substring(7));
			return (true);
		}

		private bool ReadFromEan13(string value)
		{
			if (value == null)
				throw new ArgumentNullException("value");
			
			if (value.Length != 13)
				return (false);

			_values.Add(AIType.GTIN, value);
			return (true);
		}

		private bool ReadFromEan8(string value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			if (value.Length != 8)
				return (false);

			_values.Add(AIType.GTIN, value);
			return (true);
		}

		#endregion

		public BarcodeInfo(string code, string format)
		{
			PlainText = code;
			_readedFormat = format;
			RawBytes = (code == null) ? null : Encoding.UTF8.GetBytes(code);
			Timestamp = 0;
			bool maybeeNotAi = (format != "DATA_MATRIX") && (format != "QR_CODE") && (format != "CODE_128");
			_wasReaded = !String.IsNullOrEmpty(code) && (ReadAI(code, maybeeNotAi) || ReadFromEan13(code) || ReadFromEan8(code));
		}

		internal BarcodeInfo(Result src)
		{
			PlainText = src.Text;
			RawBytes = src.RawBytes;
			Timestamp = src.Timestamp;

			List<string> fmts = new List<string>();

			if ((src.BarcodeFormat & ZXing.BarcodeFormat.DATA_MATRIX) != 0)
			{
				char[] sr = src.Text.ToCharArray();
				_readedFormat = "DATA_MATRIX";
				fmts.Add(ReadedFormat);
				_wasReaded = ReadAI(src.Text, false);
			}
			else if ((src.BarcodeFormat & ZXing.BarcodeFormat.QR_CODE) != 0)
			{
				_readedFormat = "QR_CODE";
				fmts.Add(ReadedFormat);
				_wasReaded = ReadAI(src.Text, false);
			}

			else if ((src.BarcodeFormat & ZXing.BarcodeFormat.CODE_128) != 0)
			{
				_readedFormat = "CODE_128";
				fmts.Add(ReadedFormat);
				_wasReaded = ReadAI(src.Text, false) || ReadFromChina128(src.Text);
			}
			else if ((src.BarcodeFormat & ZXing.BarcodeFormat.EAN_13) != 0)
			{
				_readedFormat = "EAN_13";
				fmts.Add("EAN_13");
				_wasReaded = ReadFromEan13(src.Text);
			}
			else if ((src.BarcodeFormat & ZXing.BarcodeFormat.EAN_8) != 0)
			{
				_readedFormat = "EAN_8";
				fmts.Add("EAN_8");
				_wasReaded = ReadFromEan8(src.Text);
			}

			if ((src.BarcodeFormat & ZXing.BarcodeFormat.AZTEC) != 0)
				fmts.Add("AZTEC");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.CODABAR) != 0)
				fmts.Add("CODABAR");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.CODE_39) != 0)
				fmts.Add("CODE_39");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.CODE_93) != 0)
				fmts.Add("CODE_93");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.ITF) != 0)
				fmts.Add("ITF");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.MAXICODE) != 0)
				fmts.Add("MAXICODE");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.PDF_417) != 0)
				fmts.Add("PDF_417");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.RSS_14) != 0)
				fmts.Add("RSS_14");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.RSS_EXPANDED) != 0)
				fmts.Add("RSS_EXPANDED");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.UPC_A) != 0)
				fmts.Add("UPC_A");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.UPC_E) != 0)
				fmts.Add("UPC_E");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.All_1D) != 0)
				fmts.Add("All_1D");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.UPC_EAN_EXTENSION) != 0)
				fmts.Add("UPC_EAN_EXTENSION");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.MSI) != 0)
				fmts.Add("MSI");
			if ((src.BarcodeFormat & ZXing.BarcodeFormat.PLESSEY) != 0)
				fmts.Add("PLESSEY");

			Formats = fmts.ToArray();
		}

		public override string ToString()
		{
			return (String.Format("GTIN: {0}, BATCH_LOT: {1}, SERIAL: {2}, USE_BY_OR_EXPIRY: {3:dd.MM.yyyy}", GTIN, Seria, Number, UseBy));
		}
	}
}
