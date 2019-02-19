using System.Collections;
using UnityEngine;
using System.Xml;

public class LanguageManager {

	public Hashtable strings;

	public LanguageManager (string language) {

		strings = new Hashtable ();
		SetLanguage (language);
	}

	public void SetLanguage(string language) {

		strings.Clear ();
		var textAsset = (TextAsset) Resources.Load ("languages");
		var xml = new XmlDocument ();
		xml.LoadXml (textAsset.text);

		var lang = xml.DocumentElement [language];
		var nodes = lang.ChildNodes;

		for (int i = 0; i < nodes.Count; i++) {
			strings.Add (nodes[i].Attributes["name"].Value, nodes[i].InnerText);
		}
	}
}
