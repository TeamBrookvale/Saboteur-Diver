using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Levels")]
public class DeserializedLevels
{
	[XmlElement ("Developer")]
	public Developer developer;
	public class Developer
	{
		[XmlAttribute ("StartLevel")]
		public string startLevel;
	}

	[XmlElement ("Level")]
	public Level[] levels;
	public class Level
	{
		[XmlAttribute ("playerx")]
		public string playerx;
		
		[XmlAttribute ("playery")]
		public string playery;

		[XmlAttribute ("bound_x_min")]
		public string bound_x_min;

		[XmlAttribute ("bound_y_min")]
		public string bound_y_min;

		[XmlAttribute ("bound_x_max")]
		public string bound_x_max;

		[XmlAttribute ("bound_y_max")]
		public string bound_y_max;

		[XmlElement("Item")]
		public Item[] items;
	}

	public class Item
	{
		[XmlAttribute ("prefab")]
		public string prefab;
		
		[XmlAttribute ("x")]
		public string x;
		
		[XmlAttribute ("y")]
		public string y;
		
		[XmlAttribute ("rot")]
		public string rot;
		
		[XmlAttribute ("scale_x")]
		public string scale_x;
		
		[XmlAttribute ("scale_y")]
		public string scale_y;

		[XmlAttribute ("speed")]
		public string speed;

		[XmlAttribute ("spotmedianangle")]
		public string spotmedianangle;
	}
}

