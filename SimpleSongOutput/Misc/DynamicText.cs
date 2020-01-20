using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SimpleSongOutput.Misc
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DynamicTextIgnoreAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class DynamicTextAttribute : Attribute
    {
        public string Name { get; set; }
        public string Format { get; set; }
    }

    public static class DynamicText
    {
        internal class DynamicTextFormat
        {
            public object Value { get; set; }
            public string Format { get; set; }

            public DynamicTextFormat(object value, string format = null)
            {
                Value = value;
                Format = format;
            }

            public override string ToString()
            {
                if (!string.IsNullOrEmpty(Format))
                {
                    return Value is IFormattable ? ((IFormattable)Value).ToString(Format, CultureInfo.InvariantCulture) : Value.ToString();
                }
                return Value.ToString();
            }
        }

        private static Dictionary<string, DynamicTextFormat> ParseObjects(object[] objects)
        {
            DateTime Now = DateTime.Now; //"MM/dd/yyyy hh:mm:ss.fffffff";  
            
            // add some static replacements
            var kvp = new Dictionary<string, DynamicTextFormat>
            {
                { "time", new DynamicTextFormat(Now, "hh:mm") },
                { "longtime", new DynamicTextFormat(Now, "hh:mm:ss") },
                { "date", new DynamicTextFormat(Now, "yyyy/MM/dd") },
                { "lf", new DynamicTextFormat(Environment.NewLine) }, // Allow carriage return
            };

            // iterate over parameter objects
            foreach (var obj in objects)
            {
                var type = obj.GetType();

                var properties = type.GetProperties();
                foreach (var prop in properties)
                {
                    // check if property is to be ignored
                    var ignoreAttributes = prop.GetCustomAttributes(typeof(DynamicTextIgnoreAttribute), false);
                    if (ignoreAttributes.Length > 0) continue;

                    // get the value of the property
                    var value = prop.GetValue(obj);

                    // special case check if its a difficulty enum, and resolve value to proper name
                    if (prop.PropertyType == typeof(BeatmapDifficulty))
                    {
                        value = Utilities.DifficultyToString((BeatmapDifficulty)value);
                    }

                    var nameAttribtues = prop.GetCustomAttributes(typeof(DynamicTextAttribute), false).Cast<DynamicTextAttribute>();
                    if (nameAttribtues.Count() == 0)
                    {
                        kvp.Add(prop.Name.ToLowerInvariant(), new DynamicTextFormat(value));
                    }
                    else
                    {
                        foreach (var nameAttribute in nameAttribtues)
                        {
                            var name = nameAttribute.Name;
                            kvp.Add((name != null) ? name.ToLowerInvariant() : prop.Name.ToLowerInvariant(), new DynamicTextFormat(value, nameAttribute.Format));
                        }
                    }
                }
            }

            return kvp;
        }

        public static string Parse(string text, params object[] objects)
        {
            StringBuilder output = new StringBuilder(text.Length);

            var kvp = ParseObjects(objects);

            for (int p = 0; p < text.Length; p++)
            {
                char c = text[p];
                if (c == '%')
                {
                    int keywordstart = p + 1;
                    int keywordlength = 0;

                    int end = Math.Min(p + 32, text.Length); // Limit the scan for the 2nd % to 32 characters, or the end of the string
                    for (int k = keywordstart; k < end; k++) // Pretty sure there's a function for this, I'll look it up later
                    {
                        if (text[k] == '%')
                        {
                            keywordlength = k - keywordstart;
                            break;
                        }
                    }

                    if (keywordlength > 0 && keywordlength != 0)
                    {
                        var keyword = text.Substring(keywordstart, keywordlength);
                        var prefix = string.Empty;
                        var prefixlength = 0;

                        for (int o = 0; o < keyword.Length; o++)
                        {
                            if (keyword[o] == ':')
                            {
                                prefixlength = o;
                                break;
                            }
                        }

                        if (prefixlength > 0)
                        {
                            prefix = keyword.Substring(prefixlength + 1);
                            keyword = keyword.Substring(0, prefixlength);
                        }

                        if (kvp.TryGetValue(keyword.ToLowerInvariant(), out DynamicTextFormat substitutetext))
                        {
                            var data = substitutetext.ToString();
                            output.Append($"{(!string.IsNullOrEmpty(data) ? prefix : string.Empty)}{data}");

                            p += keywordlength + 1; // Reset regular text
                            continue;
                        }
                    }
                }
                output.Append(c);
            }

            return output.ToString();
        }
    }
}
