using Core.Data;
using Core.Finders;
using Core.Finders.Custom;
using Core.Finders.Simple;
using Core.Models;
using Core.Pharm;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;

namespace PharmService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select PharmApi.svc or PharmApi.svc.cs at the Solution Explorer and start debugging.
	//[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class PharmApi : ApiBase, IDisposable
	{
		private Timer _timer;
		private ResetFinderInfo _finfo = new ResetFinderInfo();

		#region Рабочая база

		private const string FinderName = "PharmApi.Finder";

		private FpFinder _finder = null;

		#endregion
		#region Тестовая база

		private const string FinderTestName = "PharmApi.FinderTest";

		private FpFinder _finderTest = null;

		#endregion

		protected FpFinder Finder
		{
			get
			{
				if (_finder == null)
				{
					MemoryCache cache = MemoryCache.Default;
					_finder = cache[FinderName] as FpFinder;
					if (_finder == null)
					{
						_finder = new FpFinder(ConnectionMode.Work);
						cache.Add(FinderName, _finder, DateTimeOffset.UtcNow.AddHours(4));
					}
				}
				return (_finder);
			}
		}

		protected FpFinder FinderTest
		{
			get
			{
				if (_finderTest == null)
				{
					MemoryCache cache = MemoryCache.Default;
					_finderTest = cache[FinderTestName] as FpFinder;
					if (_finderTest == null)
					{
						_finderTest = new FpFinder(ConnectionMode.Test);
						cache.Add(FinderTestName, _finderTest, DateTimeOffset.UtcNow.AddHours(4));
					}
				}
				return (_finderTest);
			}
		}

		public PharmApi()
			: base()
		{
			_timer = new Timer(PharmApi.OnTimerElapsed, _finfo, 1000 * 60 * 5, 1000 * 60 * 5); // в миллисекундах
		}

		public static uint? NewGetUserId(string key)
		{
			try
			{
				try
				{
					UMKBRequests.AuthGetUsertId auth = new UMKBRequests.AuthGetUsertId();
					UMKBRequests.Models.API.Auth.CheckAuthGetUserId userauth = auth.Get(new UMKBRequests.Models.API.Semantic.RequestAuth()
					{
						key = null,//key,
						authkey = key,
						gateway = null,
						login = null,
						application = "7"//"37"
					});
					return (uint?)userauth.userid;
				}
				catch
				{
					UMKBRequests.AuthGetUsertId auth = new UMKBRequests.AuthGetUsertId();
					UMKBRequests.Models.API.Auth.CheckAuthGetUserId userauth = auth.Get(new UMKBRequests.Models.API.Semantic.RequestAuth()
					{
						key = null,//key,
						authkey = key,
						gateway = null,
						login = null,
						application = "5"//"37"
					});
					return (uint?)userauth.userid;
				}
			}
			catch
			{
				return (uint?)6943;
			}
		}


		public string CheckKey(ConnectionMode mode, string key)
		{
		/*	UMKBRequests.AuthGetUsertId auth = new UMKBRequests.AuthGetUsertId();
			UMKBRequests.Models.API.Auth.CheckAuthGetUserId userauth = auth.Get(new UMKBRequests.Models.API.Semantic.RequestAuth()
			{
				key = null,//key,
				authkey = key,
				gateway = null,
				login = null,
				application = "7"//"37"
			});*/
			//key = NewGetUserId(key).ToString();
			Dictionary<string, uint> dct = GetPharmMapping(key, mode);

			switch (dct.Count)
			{
				case 0:
					return (null);
				case 1:
					return (dct.Keys.First());
				default:
					return ("OWNER");
			}
		}

		public Response LoadPrice(IEnumerable<PriceItemData> data, ConnectionMode mode, string key)
		{
			Task logTask = Log("LoadPrice(...)");//	>>>-----------------------------------------------------------------------------------------
												 //																																	|
			try//																																|
			{//																																	|
				KeyValuePair<PharmPositionData, FpFinderResult>[] preparedData = null;//														|
				bool isCreated = false;//																										|
				string ip = null;//																												|
								 //																																|
				try//																															|
				{//																																|
					preparedData = Prepare(data, key, mode)// при опознании ФП может занимать продолжительное время								|
						.ToArray();//																											|
								   //																															|
					isCreated = preparedData.Any(d => d.Value != null && d.Value.IsCreated);//													|
																							//																															|
					ip = GetIp();//																												|
				}//																																|
				finally//																														|
				{//																		Асинхронный вызов (без создания потока)					|
					logTask.Wait();//	<<<-----------------------------------------------------------------------------------------------------
				}

				PharmDataAdapters pharmAdapters = PharmDataSource.GetAdapters(mode);

				Response result = preparedData
					.Where(d => d.Value != null) // ======
					.Select(d =>
					{
						if (!d.Key.IsRequiredDataExists())
							return (new Response(false, "Не все обязательные поля заполнены"));
						else if ((d.Key.PharmId == null) && !String.IsNullOrEmpty(d.Key.PharmLogin)) // вариант с пустым PharmLogin отработается в IsRequiredDataExists
							return (new Response(false, "Вы не имеете прав загружать данные для этой аптеки"));

						UmkbPharmPriceWriter.Write(d.Key, mode);

						Concept pharm = null;
						try
						{
							pharm = GetPharm(d.Key.PharmId.Value, mode);
							Response resp = pharmAdapters.Perform((PriceItemData)d.Key, pharm, mode);
							PharmLogWriter.Write(pharmAdapters, ip, "LoadPrice", d.Key, pharm, d.Value, resp, mode);
							return (resp);
						}
						catch (Exception err)
						{
							Log(err);
							Response resp = new Response(false, String.Format("{0}: {1}", err.GetType().Name, err.Message));
							PharmLogWriter.Write(pharmAdapters, ip, "LoadPrice", d.Key, pharm, d.Value, resp, mode);
							return (resp);
						}
					})
					.JoinWithTitle("Загрузка прайс-листа");

				if (isCreated)
					ResetFinders(mode);

				return (result);
			}
			catch (Exception err)
			{
				Log(err);
				return (new Response(false, String.Format("Загрузка прайс-листа: {0} ({1})", err.GetType().Name, err.Message)));
			}
		}

		public Response ChangePrice(IEnumerable<ModifyPriceItemData> data, ConnectionMode mode, string key)
		{
			Task logTask = Log("ChangePrice(...)");
			try
			{
				string ip;

				try
				{
					Prepare(data, key, mode);

					ip = GetIp();
				}
				finally
				{
					logTask.Wait();
				}

				PharmDataAdapters pharmAdapters = PharmDataSource.GetAdapters(mode);

				return (data
					.Select(d =>
					{
						if (!d.IsRequiredDataExists())
							return (new Response(false, "Не все обязательные поля заполнены"));
						else if ((d.PharmId == null) && !String.IsNullOrEmpty(d.PharmLogin)) // вариант с пустым PharmLogin отработается в IsRequiredDataExists
							return (new Response(false, "Вы не имеете прав загружать данные для этой аптеки"));

						UmkbPharmPriceWriter.Write(d, mode);

						Concept pharm = null;
						try
						{
							pharm = GetPharm(d.PharmId.Value, mode);
							Response resp = pharmAdapters.Perform(d, pharm, mode);
							PharmLogWriter.Write(pharmAdapters, ip, "ChangePrice", d, pharm, null, resp, mode);
							return (resp);
						}
						catch (Exception err)
						{
							Log(err);
							Response resp = new Response(false, String.Format("{0}: {1}", err.GetType().Name, err.Message));
							PharmLogWriter.Write(pharmAdapters, ip, "ChangePrice", d, pharm, null, resp, mode);
							return (resp);
						}
					})
					.JoinWithTitle("Изменение цен и остатков"));
			}
			catch (Exception err)
			{
				Log(err);
				return (new Response(false, String.Format("Изменение цен и остатков: {0} ({1})", err.GetType().Name, err.Message)));
			}
		}

		public Response RemoveFromPrice(IEnumerable<RemovePriceItemData> data, ConnectionMode mode, string key)
		{
			Task logTask = Log("RemoveFromPrice(...)");
			try
			{
				string ip;

				try
				{
					Prepare(data, key, mode);

					ip = GetIp();
				}
				finally
				{
					logTask.Wait();
				}

				PharmDataAdapters pharmAdapters = PharmDataSource.GetAdapters(mode);

				return (data
					.Select(d =>
					{
						if (!d.IsRequiredDataExists())
							return (new Response(false, "Не все обязательные поля заполнены"));
						else if ((d.PharmId == null) && !String.IsNullOrEmpty(d.PharmLogin)) // вариант с пустым PharmLogin отработается в IsRequiredDataExists
							return (new Response(false, "Вы не имеете прав загружать данные для этой аптеки"));

						UmkbPharmPriceWriter.Write(d, mode);

						Concept pharm = null;
						try
						{
							pharm = GetPharm(d.PharmId.Value, mode);
							Response resp = pharmAdapters.Perform(d, mode);
							PharmLogWriter.Write(pharmAdapters, ip, "RemoveFromPrice", d, pharm, null, resp, mode);
							return (resp);
						}
						catch (Exception err)
						{
							Log(err);
							Response resp = new Response(false, String.Format("{0}: {1}", err.GetType().Name, err.Message));
							PharmLogWriter.Write(pharmAdapters, ip, "RemoveFromPrice", d, pharm, null, resp, mode);
							return (resp);
						}
					})
					.JoinWithTitle("Удаление позиций из прайс-листа"));
			}
			catch (Exception err)
			{
				Log(err);
				return (new Response(false, String.Format("Удаление позиций из прайс-листа: {0} ({1})", err.GetType().Name, err.Message)));
			}
		}

		public Response LoadAssortment(IEnumerable<AssortmentItemData> data, ConnectionMode mode, string key)
		{
			Task logTask = Log("LoadAssortment(...)");

			try
			{
				KeyValuePair<PharmPositionData, FpFinderResult>[] preparedData = null;
				bool isCreated = false;
				string ip = null;

				try
				{
					preparedData = Prepare(data, key, mode)
						.ToArray();

					isCreated = preparedData.Any(d => d.Value != null && d.Value.IsCreated);

					ip = GetIp();
				}
				finally
				{
					logTask.Wait();
				}
				
				PharmDataAdapters pharmAdapters = PharmDataSource.GetAdapters(mode);
				
				Response result = preparedData
					.Where(d => d.Value != null) // ======
					.Select(d =>
					{
						if (!d.Key.IsRequiredDataExists())
							return (new Response(false, "Не все обязательные поля заполнены"));
						else if ((d.Key.PharmId == null) && !String.IsNullOrEmpty(d.Key.PharmLogin)) // вариант с пустым PharmLogin отработается в IsRequiredDataExists
							return (new Response(false, "Вы не имеете прав загружать данные для этой аптеки"));

						UmkbPharmPriceWriter.Write(d.Key, mode);

						Concept pharm = null;
						try
						{
							pharm = GetPharm(d.Key.PharmId.Value, mode);
							Response resp = pharmAdapters.Perform((AssortmentItemData)d.Key, mode);
							PharmLogWriter.Write(pharmAdapters, ip, "LoadAssortment", d.Key, pharm, d.Value, resp, mode);
							return (resp);
						}
						catch (Exception err)
						{
							Log(err);
							Response resp = new Response(false, String.Format("{0}: {1}", err.GetType().Name, err.Message));
							PharmLogWriter.Write(pharmAdapters, ip, "LoadAssortment", d.Key, pharm, d.Value, resp, mode);
							return (resp);
						}
					})
					.JoinWithTitle("Загрузка ассортимента");

				if (isCreated)
					ResetFinders(mode);

				return (result);
			}
			catch (Exception err)
			{
				Log(err);
				return (new Response(false, String.Format("Загрузка ассортимента: {0} ({1})", err.GetType().Name, err.Message)));
			}
		}

		public string Ping()
		{
			//Логировать?
			return ("Pong");
		}

		#region Private helpers

		private string GetIp()
		{
			RemoteEndpointMessageProperty endpointProperty;
			///try
			//{
			//	endpointProperty = (RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name];
			//	return (String.Format("{0}:{1}", endpointProperty.Address, endpointProperty.Port));
			//}
			//catch(Exception e)
           // {
				return (String.Format("{0}:{1}", "umkb.socmedica.com", "3306"));

			//}
			//return (String.Format("{0}:{1}", endpointProperty.Address, endpointProperty.Port));
		}

		public FpFinder GetFinder(ConnectionMode mode)
		{
			switch (mode)
			{
				case ConnectionMode.Test:
					return (FinderTest);
				case ConnectionMode.Work:
					return (Finder);
				default:
					return (null);
			}
		}

		private Concept GetLink(uint pharmId, uint packId, ConnectionMode mode)
		{
			return (UmkbDataSource.GetAdapters(mode).ConceptAdapter.Load(UmkbDataSource.GetFp2DrugstoreInstrument(mode), new Pair(95, pharmId), new Pair(67, packId)));
		}

		private Concept GetPharm(uint pharmId, ConnectionMode mode)
		{
			Concept result;
			if (GetPharmCache(mode).TryGetValue(pharmId, out result))
				return (result);

			result = UmkbDataSource.GetAdapters(mode).ConceptAdapter.Load(95, pharmId);
			GetPharmCache(mode).Add(pharmId, result);
			return (result);
		}

		public static Dictionary<string, uint> GetPharmMapping(string key, ConnectionMode mode)
		{
			if (key.Length > 10) key = PharmApi.NewGetUserId(key).ToString();
			return (UmkbDataSource.GetAdapters(mode).GetPharmsByKey(key).Mapping);
		}

		private void SendDataToUmkb(ItemData data, ConnectionMode mode)
		{
			//связь аптека-ФП: инструмент 1722
			//остаток = value_a
			//цена = value_b

			if (data.PackId == null)
				return;

			decimal? price = null;
			long? quantity = null;

			PriceItemData pd = data as PriceItemData;
			AssortmentItemData ad = data as AssortmentItemData;
			ModifyPriceItemData md = data as ModifyPriceItemData;
			RemovePriceItemData rd = data as RemovePriceItemData;

			if (pd != null)
				price = pd.SellingPrice;
			else if (ad != null)
				quantity = ad.Quantity;
			else if (md != null)
			{
				price = md.SellingPrice;
				quantity = md.Quantity;
			}
			else if (rd != null)
			{
				Concept link = GetLink(data.PharmId.Value, data.PackId.Value, mode);

				if (link != null)
				{
					link.Delete();
					UmkbDataSource.GetAdapters(mode).ConceptAdapter.Save(link);
				}

				return;
			}

			if (price != null || quantity != null)
			{
				Concept link = GetLink(data.PharmId.Value, data.PackId.Value, mode);

				if (link == null)
					link = UmkbDataSource.GetFp2DrugstoreInstrument(mode).CreateLink(new Pair(67, data.PackId.Value), new Pair(95, data.PharmId.Value), null,
						(quantity == null) ? (double?)null : (double)quantity.Value,
						(price == null) ? (double?)null : (double)price.Value,
						null,
						null);
				else
				{
					link.BeginUpdate();
					try
					{
						if (quantity != null)
							link.value_a = quantity.Value;

						if (price != null)
							link.value_b = (double)price.Value;
					}
					finally
					{
						link.CommitUpdate();
					}
				}

				UmkbDataSource.GetAdapters(mode).ConceptAdapter.Save(link);
			}
		}

		private IEnumerable<KeyValuePair<PharmPositionData, FpFinderResult>> Prepare(IEnumerable<PharmPositionData> data, string key, ConnectionMode mode)
		{
			Dictionary<string, uint> pharms = GetPharmMapping(key, mode);

			if (pharms.Count == 0)
				throw new UnauthorizedAccessException("Access denied for this auth_key");

			foreach (PharmPositionData dt in data)
			{
				FpFinderResult findres = new FpFinderResult(343278, 1, false, true);//null;
				//SMBarcode.Configuration.CONCEPT_KEY = "03741c4866069ae2";
				
			//	dt.PharmId = SMBarcode.Methods.SemanticS.ConceptSearch("95", dt.PharmLogin).Concepts[0].id;  //9027;
				//dt.PackId = 1485;
				//dt.PharmLogin = "6943_6946";
				//dt.PharmId = 343278;
				if (dt.PharmId == null)
				{
					// Пытаемся определить аптеку в пределах прав
					uint tmp;

					if (!String.IsNullOrEmpty(dt.PharmLogin) && pharms.TryGetValue(dt.PharmLogin, out tmp))
					{
						dt.PharmId = tmp;
					//	dt.PharmLogin = "6943_6946";
					}
				}
				else if (!pharms.ContainsValue(dt.PharmId.Value))
				{
					// Нет прав на эту аптеку - убиваем PharmId если он не пустой
					dt.PharmId = null;

					if (String.IsNullOrEmpty(dt.PharmLogin))
						dt.PharmLogin = "WRONG!!!";
				}

				if ((dt.PharmId != null) && (dt.PackId == null)) // Первое условие - чтобы не делать лишних телодвижений со строкой, которая по-любому не загрузится
				{
					findres = GetFinder(mode).GetFpIdByPharmPositionData(dt, true);

					if (findres != null)
						dt.PackId = findres.id;
				}

				yield return (new KeyValuePair<PharmPositionData, FpFinderResult>(dt, findres));
			}
		}

		private void Prepare(IEnumerable<ItemData> data, string key, ConnectionMode mode)
		{
			Dictionary<string, uint> pharms = GetPharmMapping(key, mode);

			if (pharms.Count == 0)
				throw new UnauthorizedAccessException("Access denied for this auth_key");

			foreach (ItemData dt in data)
			{
				if (dt.PharmId == null)
				{
					// Пытаемся определить аптеку в пределах прав
					uint tmp;
					if (!String.IsNullOrEmpty(dt.PharmLogin) && pharms.TryGetValue(dt.PharmLogin, out tmp))
						dt.PharmId = tmp;
				}
				else if (!pharms.ContainsValue(dt.PharmId.Value))
				{
					// Нет прав на эту аптеку
					dt.PharmId = null;

					if (String.IsNullOrEmpty(dt.PharmLogin))
						dt.PharmLogin = "WRONG!!!";
				}
			}
		}

		private void ResetFinders(ConnectionMode mode)
		{
			switch (mode)
			{
				case ConnectionMode.Work:
					if (_finfo.Lock())
					{
						_finfo.ResetWork = true;
						_finfo.Unlock();
					}
					break;
				case ConnectionMode.Test:
					if (_finfo.Lock())
					{
						_finfo.ResetTest = true;
						_finfo.Unlock();
					}
					break;
			}
		}

		#endregion

		private static void OnTimerElapsed(Object stateInfo)
		{
			bool rstTest = false;
			bool rstWork = false;
			ushort[] libs = { 62, 69, 75, 80, 67 };

			ResetFinderInfo finfo = stateInfo as ResetFinderInfo;
			if ((finfo != null) && finfo.Lock())
			{
				try
				{
					rstTest = finfo.ResetTest;
					rstWork = finfo.ResetWork;
				}
				finally
				{
					finfo.ResetTest = false;
					finfo.ResetWork = false;
					finfo.Unlock();
				}

				if (rstTest)
				{
					try
					{
						SphynxFinder.Reset(libs, true);
					}
					catch
					{
					}
				}

				if (rstWork)
				{
					try
					{
						SphynxFinder.Reset(libs, false);
					}
					catch
					{
					}
				}
			}
		}

		public void Dispose()
		{
			_timer.Dispose();
			_finfo.Dispose();
		}

	}

	internal sealed class ResetFinderInfo : IDisposable
	{
		private Mutex _mutex = new Mutex();
		private bool _resetWork = false;
		private bool _resetTest = false;

		public bool Lock()
		{
			return (_mutex.WaitOne());
		}

		public void Unlock()
		{
			_mutex.ReleaseMutex();
		}

		public bool ResetWork { get { return (_resetWork); } set { _resetWork = value; } }
		public bool ResetTest { get { return (_resetTest); } set { _resetTest = value; } }

		public void Dispose()
		{
			_mutex.Dispose();
		}
	}
}
