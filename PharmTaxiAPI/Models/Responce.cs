using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Core.Pharm
{
	/// <summary>
	/// Ответ сервиса
	/// </summary>
	[DataContract]
	[Serializable]
	public class Response
	{
		/// <summary>
		/// Результат выполнения операции (выполнено - не выполнено)
		/// </summary>
		[DataMember]
		public bool Successed { get; set; }

		/// <summary>
		/// Текстовое сообщение ("Операция выполнена" либо текст сообщения об ошибке)
		/// </summary>
		[DataMember]
		public string Message { get; set; }

		/// <summary>
		/// Предупреждалки и ошибки по номеру строки (если со строкой что-то прошло не так)
		/// </summary>
		[DataMember]
		public RowWarning[] RowWarnings { get; set; }

		public Response()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="successed">Результат выполнения операции (выполнено - не выполнено)</param>
		/// <param name="message">Текстовое сообщение ("Операция выполнена либо текст сообщения об ошибке")</param>
		public Response(bool successed, string message)
		{
			Successed = successed;
			Message = message;
			RowWarnings = null;
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="successed">Результат выполнения операции (выполнено - не выполнено)</param>
		/// <param name="message">Текстовое сообщение ("Операция выполнена либо текст сообщения об ошибке")</param>
		public Response(bool successed, string message, RowWarning[] rowWarnings)
		{
			Successed = successed;
			Message = message;
			RowWarnings = rowWarnings;
		}
		public Response(bool successed, string message, Dictionary<int, string> rowWarnings)
		{
			Successed = successed;
			Message = message;
			RowWarnings = rowWarnings
				.Select(p => new RowWarning(p.Key, p.Value))
				.ToArray();
		}
	}

	[DataContract]
	[Serializable]
	public class RowWarning
	{
		[DataMember]
		public int RowNumber { get; set; }

		[DataMember]
		public string Message { get; set; }

		public RowWarning()
		{
		}

		public RowWarning(int rowNumber, string message)
		{
			RowNumber = rowNumber;
			Message = message;
		}
	}

	public static class ResponseExtensions
	{
		public static Response JoinWithTitle(this IEnumerable<Response> src, string title)
		{
			bool ok = false;
			bool wr = false;
			int rownum = 1;
			Dictionary<int, string> rowWarnings = new Dictionary<int,string>();

			foreach(Response r in src)
			{
				if(r.Successed)
					ok = true;
				else
				{
					wr = true;
					rowWarnings.Add(rownum, r.Message);
				}

				rownum++;
			}

			return (new Response(ok, String.Format("{0}: {1}", title, ok ? (wr ? "Выполнено с ошибками" : "Выполнено") : "Не выполнено"), rowWarnings));
		}

		public static Response JoinWithTitle2(IEnumerable<Response> src, string title)
		{
			bool ok = false;
			bool wr = false;
			int rownum = 1;
			Dictionary<int, string> rowWarnings = new Dictionary<int, string>();

			foreach (Response r in src)
			{
				if (r.Successed)
					ok = true;
				else
				{
					wr = true;
					rowWarnings.Add(rownum, r.Message);
				}

				rownum++;
			}

			return (new Response(ok, String.Format("{0}: {1}", title, ok ? (wr ? "Выполнено с ошибками" : "Выполнено") : "Не выполнено"), rowWarnings));
		}
	}
}