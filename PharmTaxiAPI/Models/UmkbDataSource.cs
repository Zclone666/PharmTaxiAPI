using Core.Models;
using Core.Operations;
using MySql.Data.MySqlClient;
using OL.Connections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;

namespace Core.Data
{
	public static class UmkbDataSource
	{
		private static string GetKey(ConnectionMode mode, string name)
		{
			return(String.Format("{0}_{1}", ((mode == ConnectionMode.Test) ? "Test" : "Work"), name));
		}

		private static DateTimeOffset GetOffset()
		{
			return(DateTimeOffset.UtcNow.AddHours(2));
		}

		private static T GetValue<T>(ConnectionMode mode, string name, Func<T> creator)
			where T: class
		{
			lock (MemoryCache.Default)
			{
				string key = GetKey(mode, name);
				T value = MemoryCache.Default[key] as T;

				if (value == null)
				{
					value = creator();
					MemoryCache.Default.Add(key, value, GetOffset());
				}
				return (value);
			}
		}

		private static void ResetValue(ConnectionMode mode, string name)
		{
			lock (MemoryCache.Default)
			{
				MemoryCache.Default.Remove(GetKey(mode, name));
			}
		}

		public static string GetConnectionStringName(ConnectionMode mode)
		{
			switch(mode)
			{
				case ConnectionMode.Test:
					return ("UmkbTestConnectionString");
				case ConnectionMode.Work:
					return ("UmkbConnectionString");
				default:
					return (null);
			}
		}

		public static ConnectionPool<MySqlConnection> GetPool(ConnectionMode mode)
		{
		//	@"Data Source=91.239.27.158;Port=2288;Initial Catalog=dccs;User ID=dssc_user;Password=o5BqvY3Til14xdXY"
			return (GetValue<ConnectionPool<MySqlConnection>>(mode, "Pool", () => new ConnectionPool<MySqlConnection>("server=umkb.socmedica.com;port=3306;database=umkb;user id=umkb_o;password='HG;ls;kg8934=12f';allowuservariables=True;CharSet=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True")));//ConfigurationManager.ConnectionStrings[GetConnectionStringName(mode)].ConnectionString)));
		}

		public static DataAdapters GetAdapters(ConnectionMode mode)
		{
			return (GetValue<DataAdapters>(mode, "Adapters", () =>
			{
				ConnectionPool<MySqlConnection> pl = GetPool(mode);
				return (new DataAdapters(pl, pl, mode));
			}));
		}

		public static PharmUnitConvertor GetPharmUnitConvertor(ConnectionMode mode)
		{
			return (GetValue<PharmUnitConvertor>(mode, "PharmUnitConvertor", () =>
			{
				return (new PharmUnitConvertor(mode));
			}));

		}

		#region Instruments

		public static Dictionary<uint, Instrument> GetInstruments(ConnectionMode mode)
		{
			return (GetValue<Dictionary<uint, Instrument>>(mode, "Instruments", () => GetAdapters(mode).InstrumentAdapter.Load().ToDictionary(i => i.instrument_id)));
		}

		public static Instrument GetTn2DvInstrument(ConnectionMode mode)
		{
			return (GetInstruments(mode)[966]);
		}

		public static Instrument GetFp2TnInstrument(ConnectionMode mode)
		{
			return (GetInstruments(mode)[1094]);
		}

		public static Instrument GetFp2LfInstrument(ConnectionMode mode)
		{
			return (GetInstruments(mode)[1114]);
		}

		public static Instrument GetFp2PackInstrument(ConnectionMode mode)
		{
			return (GetInstruments(mode)[1574]);
		}

		public static Instrument GetGroupInstrument(ConnectionMode mode)
		{
			return (GetInstruments(mode)[1720]);
		}

		public static Instrument GetHolonimMeronimInstrument(ConnectionMode mode)
		{
			return (GetInstruments(mode)[1578]);
		}

		public static Instrument GetFp2DrugstoreInstrument(ConnectionMode mode)
		{
			return (UmkbDataSource.GetInstruments(mode)[1722]);
		}

		public static Instrument GetFp2CountryInstrument(ConnectionMode mode)
		{
			return (UmkbDataSource.GetInstruments(mode)[1526]);
		}

		public static Instrument GetFp2ManufacturerInstrument(ConnectionMode mode)
		{
			return (UmkbDataSource.GetInstruments(mode)[1717]);
		}

		public static Instrument GetUnit2ChildUnitInstrument(ConnectionMode mode)
		{
			return (UmkbDataSource.GetInstruments(mode)[1632]);
		}

		public static Instrument GetUnit2BaseUnitInstrument(ConnectionMode mode)
		{
			return (UmkbDataSource.GetInstruments(mode)[1636]);
		}

		#endregion

		public static ILookup<uint, Concept> GetLinksFp2Up(ConnectionMode mode)
		{
			return (GetValue<ILookup<uint, Concept>>(mode, "LinksFp2Up", () => GetAdapters(mode).ConceptAdapter.Load(GetInstruments(mode)[1576], withValues: true).ToLookup(c => (c.lib_a == 67 && c.id_a != null) ? c.id_a.Value : ((c.lib_b == 67 && c.id_b != null) ? c.id_b.Value : 0))));
		}

		public static string CreateTemporaryAuth(uint userId, ConnectionMode mode)
		{
			return (GetAdapters(mode).Auth(userId));
		}

		public static void FinalizeTemporaryAuth(string key, ConnectionMode mode)
		{
			GetAdapters(mode).UnAuth(key);
		}
	}
}
