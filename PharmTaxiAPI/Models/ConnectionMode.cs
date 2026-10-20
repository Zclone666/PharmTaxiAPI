using System.Runtime.Serialization;

namespace Core.Data
{
	/// <summary>
	/// Режим подключения
	/// </summary>
	[DataContract]
	public enum ConnectionMode
	{
		/// <summary>
		/// Рабочая база
		/// </summary>
		[EnumMember]
		Work = 1,

		/// <summary>
		/// Тестовая база
		/// </summary>
		[EnumMember]
		Test = 2
	}
}