using API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.Common
{
    public class ReadMPFD : HttpResponseMessage//: System.Web.Http.ApiController
	{
		public static async Task<Dictionary<string, FormData>> ReadMultipartData()
		{
			ReadMPFD ap = new ReadMPFD();
			//System.Web.Http.ApiController ap=;// = new System.Web.Http.ApiController();
			//if (!ap.Request.Content.IsMimeMultipartContent())
			//	throw new Exception("IncorrectMimeType");

			Dictionary<string, FormData> result = new Dictionary<string, FormData>();
			MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();
			await ap.RequestMessage.Content.ReadAsMultipartAsync(provider);

			foreach (HttpContent content in provider.Contents)
			{
				ContentDispositionHeaderValue contentDisposition = content.Headers.ContentDisposition;
				string name = contentDisposition.Name.Trim('\"');

				//using (StreamWriter sw = new StreamWriter(_logFileName, true, Encoding.UTF8))
				//{
				//	sw.WriteLine(name);
				//}

				if (contentDisposition.FileName != null)
				{
					string fileName = contentDisposition.FileName.Trim('\"');
					string tmpFileName = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Guid.NewGuid().ToString(), Path.GetExtension(fileName)));

					using (Stream contentStream = await content.ReadAsStreamAsync())
					using (FileStream fs = File.Create(tmpFileName))
					{
						await contentStream.CopyToAsync(fs);
						fs.Close();
					}

					result.Add(name, new FormFile(name, fileName, tmpFileName));
				}
				else
				{
					result.Add(name, new FormField(name, await content.ReadAsStringAsync()));
				}
			}

			return (result);
		}
	}
}
