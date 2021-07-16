using System;
using System.Collections.Generic;
using System.Dynamic;
using Agility.NET5.FetchAPI.Models.API;
using Agility.NET5.Shared.Models;
using Agility.NET5.Shared.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Agility.NET5.FetchAPI.Helpers
{
    public static class DynamicHelpers
    {
        public static List<ContentZone> DeserializeContentZones(string content)
        {
            var contentZones = new List<ContentZone>();

            try
            {
                var zones = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());

                if (zones == null) return contentZones;

                foreach (var (key, value) in zones)
                {
                    var modules = JsonSerializer.Serialize(value, Constants.JsonSerializerOptions);

                    var contentZone = new ContentZone()
                    {
                        ReferenceName = key,
                        Modules = JsonSerializer.Deserialize<List<Module_Model>>(modules, Constants.JsonSerializerOptions)
                    };

                    contentZones.Add(contentZone);
                }

                return contentZones;

            }
            catch (Exception)
            {
                return contentZones;
            }
        }

        public static List<SitemapPage> DeserializeSitemapFlat(string content)
        {
            var sitemapFlat = new List<SitemapPage>();

            try
            {
                var pages = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());

                if (pages == null) return sitemapFlat;

                foreach (var (key, value) in pages)
                {
                    var pageDetails = JsonSerializer.Serialize(value, Constants.JsonSerializerOptions);
                    var pageDetailsDeserialized = JsonSerializer.Deserialize<SitemapPage>(pageDetails, Constants.JsonSerializerOptions);

                    if (pageDetailsDeserialized == null) continue;
                    sitemapFlat.Add(pageDetailsDeserialized);
                }

                return sitemapFlat;

            }
            catch (Exception)
            {
                return sitemapFlat;
            }
        }

        public static List<SitemapPage> DeserializeSitemapNested(string content)
        {
            var sitemapNested = new List<SitemapPage>();

            try
            {
                var pages = JsonConvert.DeserializeObject<List<ExpandoObject>>(content, new ExpandoObjectConverter());

                if (pages == null) return sitemapNested;

                foreach (var page in pages)
                {
                    var pageDetails = JsonSerializer.Serialize(page, Constants.JsonSerializerOptions);
                    var pageDetailsDeserialized = JsonSerializer.Deserialize<SitemapPage>(pageDetails, Constants.JsonSerializerOptions);

                    if (pageDetailsDeserialized == null) continue;
                    sitemapNested.Add(pageDetailsDeserialized);
                }

                return sitemapNested;

            }
            catch (Exception)
            {
                return sitemapNested;
            }
        }

        public static ContentItemResponse<T> DeserializeContentItemTo<T>(string content)
        {
            try
            {
                return JsonSerializer.Deserialize<ContentItemResponse<T>>(content, Constants.JsonSerializerOptions);

            }
            catch (Exception)
            {
                return default;
            }
        }

        public static ContentListResponse<T> DeserializeContentListTo<T>(string content)
        {
            try
            {
                return JsonSerializer.Deserialize<ContentListResponse<T>>(content, Constants.JsonSerializerOptions);

            }
            catch (Exception)
            {
                return default;
            }
        }

        public static T DeserializeTo<T>(string content)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(content, Constants.JsonSerializerOptions);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static List<ContentItemResponse<T>> DeserializeSyncItemsTo<T>(IList<ContentItem> contentItems,
            string definitionName)
        {
            var deserializedContentItems = new List<ContentItemResponse<T>>();

            try
            {
                foreach (var contentItem in contentItems)
                {
                    if (contentItem.Properties.DefinitionName != definitionName) continue;

                    var deserializedContentItem = new ContentItemResponse<T>()
                    {
                        ContentID = contentItem.ContentID,
                        Fields = JsonSerializer.Deserialize<T>(contentItem.Fields.ToString(), Constants.JsonSerializerOptions),
                        Properties = contentItem.Properties
                    };
                    deserializedContentItems.Add(deserializedContentItem);
                }

                return deserializedContentItems;
            }
            catch (Exception)
            {
                return default;
            }

        }
    }
}
