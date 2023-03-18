using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SWP391.Project.Extensions
{
    public static class ObjectExtension
    {
        public static T MergeInto<T>(this T source, T? destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                return source;
                // throw new ArgumentNullException(nameof(destination));
            }

            PropertyInfo[] sourceProperties = source.GetType().GetProperties();
            PropertyInfo[] destinationProperties = destination.GetType().GetProperties();

            JsonSerializerOptions options = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };
            string jsonSource = JsonSerializer.Serialize(source, options);
            string jsonDestination = JsonSerializer.Serialize(destination, options);

            JsonDocument sourceDoc = JsonDocument.Parse(jsonSource);
            JsonDocument destinationDoc = JsonDocument.Parse(jsonDestination);

            JsonElement sourceElement = sourceDoc.RootElement;
            JsonElement destinationElement = destinationDoc.RootElement;

            foreach (JsonProperty property in sourceElement.EnumerateObject())
            {
                PropertyInfo? matchingProperty = destinationProperties.FirstOrDefault(x => x.Name == property.Name);
                matchingProperty?.SetValue(destination, Convert.ChangeType(property.Value, matchingProperty.PropertyType));
            }

            return destination;
        }
    }
}