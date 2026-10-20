using Core.Data;
using Core.Models;
using MySql.Data.MySqlClient;
using OL.Connections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace PharmService
{
	public class ApiBase
	{
		private const string _logFileName = @"C:\inetpub\wcfroot\PharmService\log.txt";

		private Dictionary<uint, Concept> _pharmCache = new Dictionary<uint, Concept>();
		private Dictionary<uint, Concept> _pharmCacheTest = new Dictionary<uint, Concept>();

		#region Фармтаксишная база

		private const string PharmConnectionPoolName = "ApiBase.PharmConnectionPool";
		private const string PharmAdaptersName = "ApiBase.PharmAdapters";

		private const string PharmConnectionStringName = "PharmConnectionString";

		// Для базы аптечного ассортимента
		private ConnectionPool<MySqlConnection> _pharmConnectionPool = null;
		private PharmDataAdapters _pharmAdapters = null;

		#endregion

		private static CultureInfo _ruCulture = CultureInfo.CreateSpecificCulture("ru-RU");

		protected Dictionary<uint, Concept> GetPharmCache(ConnectionMode mode)
		{
			switch (mode)
			{
				case ConnectionMode.Test:
					return (_pharmCacheTest);
				case ConnectionMode.Work:
					return (_pharmCache);
				default:
					return (null);
			}
		}

		protected async Task Log(string message)
		{
			string str = String.Format(_ruCulture,
						"============================================={3}MESSAGE{3}At:      {0:G}{3}From:    {1}{3}{2}{3}{3}",
						DateTime.Now,
						GetType().Name,
						message,
						Environment.NewLine);

			try
			{
				using (StreamWriter sw = new StreamWriter(_logFileName, true, Encoding.UTF8))
				{
					await sw.WriteLineAsync(str);
				}
			}
			catch
			{
			}
		}

		protected void Log(Exception err)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(_logFileName, true, Encoding.UTF8))
				{
					sw.WriteLine(String.Format(_ruCulture,
						"============================================={2}ERROR{2}At:      {0:G}{2}From:    {1}{2}",
						DateTime.Now,
						GetType().Name,
						Environment.NewLine));

					while (err != null)
					{
						sw.WriteLine(String.Format(_ruCulture,
							"---{3}Class:   {0}{3}Message: {1}{3}{2}{3}",
							err.GetType().Name,
							err.Message,
							err.StackTrace,
							Environment.NewLine));

						err = err.InnerException;
					}
				}
			}
			catch
			{

			}
		}
	}
}