namespace Agility.NET.FetchAPI.Models
{
    public class Setting
    {
        //text settings
        public string Required { get; set; }
        public string DefaultValue { get; set; }
        public string Unique { get; set; }
        public string CopyAcrossAllLanguages { get; set; }
        public string Length { get; set; }

        //linked content settings
        public string ContentDefinition { get; set; }
        public string RenderAs { get; set; }
        public string ContentView { get; set; }
        public string SharedContent { get; set; }
        public string LinkedContentType { get; set; }
        public string LinkeContentDropdownTextField { get; set; }
        public string LinkeContentDropdownValueField { get; set; }
        public string DisplayColumn { get; set; }
        public string ValueColumn { get; set; }
        public string SortColumn { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public string SortIDFieldName { get; set; }
        public string DefaultColumns { get; set; }
        public string LinkedContentNestedTypeID { get; set; }
        public string ColumnCount { get; set; }
        public string DefaultSearchValue { get; set; }

        //image settings
        public string ImageSizeCheck { get; set; }
        public string FileSize { get; set; }
        public string ImageWidthCheck { get; set; }
        public string ImageWidth { get; set; }
        public string ImageHeightCheck { get; set; }
        public string ImageHeight { get; set; }
        public string ValidFileTypes { get; set; }
        public string ValidFileTypesValidationMessage { get; set; }
        public string DefaultFilePath { get; set; }
        public string ThumbnailSettings { get; set; }
        public string DefaultUploadFolderPath { get; set; }
        public string AltTextRequired { get; set; }
    }
}
