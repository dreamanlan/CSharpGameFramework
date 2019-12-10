using System.Text;
using Unity.MemoryProfilerForExtension.Editor.Extensions.String;

namespace Unity.MemoryProfilerForExtension.Editor.Legacy
{
    /// <summary>
    /// Format conversion class from Newtonsoft's Json format.
    /// Used for .memsnap3 file data conversion to Unity's JsonUtil json format
    /// </summary>
    internal class JsonNetConverter
    {
        readonly string k_BytesToken = @"""m_Bytes"":";
        readonly string k_StaticFieldBytesToken = @"""m_StaticFieldBytes"":";
        const char k_ArrayStartToken = '[';
        const char k_ArrayEndToken = ']';
        const char k_ElementDelimiterToken = ',';
        readonly string k_ArrayDelimiterToken = @"""";

        private string ConvertByteArrayToJsonUtilSerialization(StringBuilder builder, string source, string token)
        {
            int endIndex = 0;

            while (true)
            {
                int startIndex = source.IndexOf(endIndex, token);
                if (startIndex == -1)
                {
                    break;
                }

                startIndex += token.Length; //jump over the current occurrence of the token
                ++startIndex; // include token delimiter
                builder.Append(source, endIndex, startIndex - endIndex);
                builder[builder.Length - 1] = k_ArrayStartToken;
                endIndex = source.IndexOf(startIndex, k_ArrayDelimiterToken);

                var data = System.Convert.FromBase64String(source.Substring(startIndex, endIndex - startIndex));

                if (data.Length == 0)
                {
                    builder.Append(k_ElementDelimiterToken); //append a delimiter character to be overridden in case we do not have any data in the byte array
                }
                else
                {
                    for (int i = 0; i < data.Length; ++i)
                    {
                        builder.Append(data[i]);
                        builder.Append(k_ElementDelimiterToken);
                    }
                }

                builder[builder.Length - 1] = k_ArrayEndToken;
                ++endIndex; //jump one position to skip the token delimiter
            }

            builder.Append(source, endIndex, source.Length - endIndex);
            return builder.ToString();
        }

        public string Convert(string newtonsoftJsonFormat)
        {
            StringBuilder builder = new StringBuilder(newtonsoftJsonFormat.Length * 2);

            string result = ConvertByteArrayToJsonUtilSerialization(builder, newtonsoftJsonFormat, k_BytesToken);
            builder.Remove(0, builder.Length);
            result = ConvertByteArrayToJsonUtilSerialization(builder, result, k_StaticFieldBytesToken);

            return result;
        }
    }

    internal static class JsonUtil
    {
        public static string JsonFindAndReplace(string json, string oldToken, string newToken)
        {
            int foundIndex = json.IndexOf(0, oldToken);
            if (foundIndex == -1)
            {
                return json;
            }

            StringBuilder builder = new StringBuilder(json.Length + newToken.Length - oldToken.Length);

            builder.Append(json, 0, foundIndex);
            builder.Append(newToken);
            int endIndex = foundIndex + oldToken.Length;
            builder.Append(json, endIndex, json.Length - endIndex);

            return builder.ToString();
        }
    }
}
