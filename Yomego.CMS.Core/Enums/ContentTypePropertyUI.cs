namespace Yomego.CMS.Core.Enums
{
    // --------------------------------------------
    // [LG] NOTE - Clearly, this enum is currently strong tied to Umbraco as the enum values are id's. This
    // could be abstracted out at some point.
    // --------------------------------------------

    /// <summary>
    /// Document type property Data Type (as defined in Umbraco Data Types section)
    /// </summary>
    public enum ContentTypePropertyUI
    {
        /// <summary>
        /// Use this property type in case you implemented you own, custom type.
        /// </summary>
        Other = 0,
        /// <summary>
        /// Approved color
        /// </summary>
        ApprovedColor = -37,
        /// <summary>
        /// Checkbox list
        /// </summary>
        CheckboxList = -43,
        /// <summary>
        /// Content picker
        /// </summary>
        ContentPicker = 1034,
        /// <summary>
        /// Date picker with time
        /// </summary>
        DatePickerWithTime = -36,
        /// <summary>
        /// Date picker (only date without time)
        /// </summary>
        DatePicker = -41,
        /// <summary>
        /// Dropdown multiple
        /// </summary>
        DropdownMultiple = -39,
        /// <summary>
        /// Drop down
        /// </summary>
        Dropdown = -42,
        /// <summary>
        /// Folder browser
        /// </summary>
        FolderBrowser = -38,
        /// <summary>
        /// Label
        /// </summary>
        Label = -92,
        /// <summary>
        /// Media picker
        /// </summary>
        MediaPicker = 1035,
        /// <summary>
        /// Member picker
        /// </summary>
        MemberPicker = 1036,
        /// <summary>
        /// Numeric
        /// </summary>
        Numeric = -51,
        /// <summary>
        /// Radio box
        /// </summary>
        Radiobox = -40,
        /// <summary>
        /// Related links
        /// </summary>
        RelatedLinks = 1040,
        /// <summary>
        /// Richtext editor
        /// </summary>
        RichtextEditor = -87,
        /// <summary>
        /// Simple editor
        /// </summary>
        SimpleEditor = 1038,
        /// <summary>
        /// Tags
        /// </summary>
        Tags = 1041,
        /// <summary>
        /// Multi line textbox
        /// </summary>
        TextboxMultiple = -89,
        /// <summary>
        /// One line of text
        /// </summary>
        Textstring = -88,
        /// <summary>
        /// Boolean (true/false)
        /// </summary>
        TrueFalse = -49,
        /// <summary>
        /// Ultimate picker
        /// </summary>
        UltimatePicker = 1039,
        /// <summary>
        /// Upload
        /// </summary>
        Upload = -90
    }
}
