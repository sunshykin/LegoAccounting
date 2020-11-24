using System.Runtime.Serialization;

namespace LegoAccounting.Domain.Enums
{
	public enum StateType
	{
		[EnumMember(Value = "N")]
		New,

		[EnumMember(Value = "U")]
		Used
	}
}