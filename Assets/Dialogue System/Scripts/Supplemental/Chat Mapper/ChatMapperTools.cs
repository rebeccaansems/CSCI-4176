using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.ChatMapper {

	/// <summary>
	/// To allow for platform-dependent compilation, these methods have been moved
	/// out of ChatMapperProject.cs, which is precompiled into a DLL.
	/// </summary>
	public static class ChatMapperTools {

		/// <summary>
		/// Creates a ChatMapperProject loaded from an XML file.
		/// </summary>
		/// <param name="xmlFile">XML file asset.</param>
		public static ChatMapperProject Load(TextAsset xmlFile) {
			#if UNITY_METRO
			Debug.LogWarning("ChatMapperTools.Load() is not supported in Windows Store apps.");
			return null;
			#else
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ChatMapperProject));
			return xmlSerializer.Deserialize(new StringReader(xmlFile.text)) as ChatMapperProject;
			#endif
		}

		/// <summary>
		/// Creates a ChatMapperProject loaded from an XML file.
		/// </summary>
		/// <param name="filename">Filename of an XML file.</param>
		public static ChatMapperProject Load(string filename) {
			#if UNITY_METRO
			Debug.LogWarning("ChatMapperTools.Load() is not supported in Windows Store apps.");
			return null;
			#else
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ChatMapperProject));
			return xmlSerializer.Deserialize(new StreamReader(filename)) as ChatMapperProject;
			#endif
		}

		/// <summary>
		/// Saves a ChatMapperProject as XML with the specified filename.
		/// </summary>
		/// <param name="filename">Filename to save.</param>
		public static void Save(ChatMapperProject chatMapperProject, string filename) {
			#if UNITY_METRO
			Debug.LogWarning("ChatMapperTools.Save() is not supported in Windows Store apps.");
			#else
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ChatMapperProject));
			StreamWriter streamWriter = new StreamWriter(filename, false, System.Text.Encoding.Unicode);
			xmlSerializer.Serialize(streamWriter, chatMapperProject);
			streamWriter.Close();
			#endif
		}

	}

}
