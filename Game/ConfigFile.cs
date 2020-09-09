using System.Collections.Generic;
using System.Linq;

namespace TennisFightingGame
{
	/// <summary>
	/// Methods for extracting information from the configuration file.
	/// </summary>
    public class ConfigFile
	{
		public Dictionary<string, Dictionary<string, string>> configs =
            new Dictionary<string, Dictionary<string, string>>();

		// TODO Implement file reload
        public ConfigFile(IEnumerable<string> file)
        {
            List<string> lines = file.ToList();

            // Remove empty lines
            lines.RemoveAll(l => l == "" || l.StartsWith(";", System.StringComparison.Ordinal));

            string lastSectionName = "";
            for (int i = 0; i < lines.Count; i++)
            {
                // Remove comments
                if (lines[i].Contains(';'))
                {
                    lines[i] = lines[i].Substring(0, lines[i].IndexOf(';'));
                }

                if (lines[i].StartsWith("[", System.StringComparison.Ordinal) && 
                	lines[i].EndsWith("]", System.StringComparison.Ordinal))
                {
                    lastSectionName = lines[i].Split('[', ']')[1];
                    configs.Add(lastSectionName, new Dictionary<string, string>());
                }
                else //add item to last section
                {
                    string[] key = lines[i].Split('=');
                    configs[lastSectionName].Add(key[0], key[1]);
                }
            }
        }

		/// <summary>
		/// Try to get a boolean ("Yes" or "No") given a section and a key. Return it if valid, 
		/// otherwise return its default value.
		/// </summary>
		public bool Boolean(string section, string key)
        {
			if (configs[section][key] != null)
			{
				return configs[section][key] == "Yes";
			}

			return default(bool);
		}

		/// <summary>
		/// Try to get an number (integer) given a section and a key. Return it if valid, otherwise 
		/// return its default value.
		/// </summary>
		public int Number(string section, string key)
        {
			int number;
			bool valid = int.TryParse(configs[section][key], out number);

			if (valid)
			{
				return number;
			}

			return default(int);
        }

		/// <summary>
		/// Try to get a string given a section and a key. Return it if valid, otherwise return its
		/// default value.
		/// </summary>
        public string String(string section, string key)
        {
			if (configs[section][key] != null)
			{
				return configs[section][key];
			}

			return default(string);
		}
    }
}