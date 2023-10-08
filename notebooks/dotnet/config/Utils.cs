using System.IO;
using System.Text.Json;

public static class Utils
{
    // Function used to wrap long lines of text
    public static string WordWrap(string text, int maxLineLength)
    {
        var result = new StringBuilder();
        int i;
        var last = 0;
        var space = new[] { ' ', '\r', '\n', '\t' };
        do
        {
            i = last + maxLineLength > text.Length
                ? text.Length
                : (text.LastIndexOfAny(new[] { ' ', ',', '.', '?', '!', ':', ';', '-', '\n', '\r', '\t' }, Math.Min(text.Length - 1, last + maxLineLength)) + 1);
            if (i <= last) i = Math.Min(last + maxLineLength, text.Length);
            result.AppendLine(text.Substring(last, i - last).Trim(space));
            last = i;
        } while (i < text.Length);

        return result.ToString();
    }

    public static void Print(string text)
    {
        Console.WriteLine(WordWrap(text, 80));
    }

    public static async Task<List<string>> GetChunksAsync(string filePath)
    {
        const int maxTokensPerParagraph = 160;
        const int maxTokensPerLine = 60;

        // Check if file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }
        
        // Read file from local file system        
        var streamReader = new StreamReader(filePath);
        var text = await streamReader.ReadToEndAsync();

        var lines = TextChunker.SplitPlainTextLines(text, maxTokensPerLine);
        return TextChunker.SplitPlainTextParagraphs(lines, maxTokensPerParagraph);
    }

    public static string FormatJsonString(string jsonString)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(jsonString);
        return System.Text.Json.JsonSerializer.Serialize(jsonElement, options);
    }   
}