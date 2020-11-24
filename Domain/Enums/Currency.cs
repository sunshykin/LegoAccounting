using System.Runtime.Serialization;

namespace LegoAccounting.Domain.Enums
{
	public enum Currency
	{
		[EnumMember(Value = "ARS")]
		ArgentinePeso,

		[EnumMember(Value = "AUD")]
		AustralianDollar,

		[EnumMember(Value = "BRL")]
		BrazilianReal,

		[EnumMember(Value = "BGN")]
		BulgarianLev,

		[EnumMember(Value = "CAD")]
		CanadianDollar,

		[EnumMember(Value = "CNY")]
		ChineseYuan,

		[EnumMember(Value = "HRK")]
		CroatianKuna,

		[EnumMember(Value = "CZK")]
		CzechKoruna,

		[EnumMember(Value = "DKK")]
		DanishKrone,

		[EnumMember(Value = "EUR")]
		Euro,

		[EnumMember(Value = "GTQ")]
		GuatemalanQuetzal,

		[EnumMember(Value = "HKD")]
		HongKongDollar,

		[EnumMember(Value = "HUF")]
		HungarianForint,

		[EnumMember(Value = "INR")]
		IndianRupee,

		[EnumMember(Value = "IDR")]
		IndonesianRupiah,

		[EnumMember(Value = "ILS")]
		IsraeliNewShekel,

		[EnumMember(Value = "JPY")]
		JapaneseYen,

		[EnumMember(Value = "MOP")]
		MacauPataca,

		[EnumMember(Value = "MYR")]
		MalaysianRinggit,

		[EnumMember(Value = "MXN")]
		MexicanPeso,

		[EnumMember(Value = "NZD")]
		NewZealandDollar,

		[EnumMember(Value = "NOK")]
		NorwegianKroner,

		[EnumMember(Value = "PHP")]
		PhilippinePeso,

		[EnumMember(Value = "PLN")]
		PolishZloty,

		[EnumMember(Value = "GBP")]
		PoundSterling,

		[EnumMember(Value = "RON")]
		RomanianNewLei,

		[EnumMember(Value = "RUB")]
		RussianRouble,

		[EnumMember(Value = "RSD")]
		SerbianDinar,

		[EnumMember(Value = "SGD")]
		SingaporeDollar,

		[EnumMember(Value = "ZAR")]
		SouthAfricanRand,

		[EnumMember(Value = "KRW")]
		SouthKoreanWon,

		[EnumMember(Value = "SEK")]
		SwedishKrona,

		[EnumMember(Value = "CHF")]
		SwissFranc,

		[EnumMember(Value = "TWD")]
		TaiwanNewDollar,

		[EnumMember(Value = "THB")]
		ThaiBaht,

		[EnumMember(Value = "TRY")]
		TurkishLira,

		[EnumMember(Value = "UAH")]
		UkraineHryvnia,

		[EnumMember(Value = "USD")]
		UitedStatesDollar,
	}
}