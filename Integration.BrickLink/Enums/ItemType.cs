using System.Runtime.Serialization;

namespace LegoAccounting.Integration.BrickLink.Enums
{
	public enum ItemType
	{
		[EnumMember(Value = "MINIFIG")]
		Minifigure,

		[EnumMember(Value = "PART")]
		Part,

		[EnumMember(Value = "SET")]
		Set,

		[EnumMember(Value = "BOOK")]
		Book,

		[EnumMember(Value = "GEAR")]
		Gear,

		[EnumMember(Value = "CATALOG")]
		Catalog,

		[EnumMember(Value = "INSTRUCTION")]
		Instruction,

		[EnumMember(Value = "UNSORTED_LOT")]
		UnsortedLot,

		[EnumMember(Value = "ORIGINAL_BOX")]
		OriginalBox
	}
}