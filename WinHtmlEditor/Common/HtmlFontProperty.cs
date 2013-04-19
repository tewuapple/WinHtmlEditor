#region Using directives

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;

using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;

#endregion

namespace WinHtmlEditor
{

    #region HtmlFontSize enumeration

    /// <summary>
    /// Enum used to modify the font size
    /// </summary>
    public enum HtmlFontSize
    {
        Default	= 0,
        xxSmall	= 1,		// 8 points
        xSmall	= 2,		// 10 points
        Small	= 3,		// 12 points
        Medium	= 4,		// 14 points
        Large	= 5,		// 18 points
        xLarge	= 6,		// 24 points
        xxLarge	= 7			// 36 points

    } //HtmlFontSize

    #endregion

    #region HtmlFontProperty struct

    /// <summary>
    /// Struct used to define a Html Font
    /// Supports Name Size Bold Italic Subscript Superscript Strikeout
    /// Specialized TypeConvertor used for Designer Support
    /// If Name is Empty or Null Struct is consider Null
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(HtmlFontPropertyConverter))]
    public struct HtmlFontProperty
    {
        // properties defined for the Font
        private string			_name;
        private HtmlFontSize	_size;
        private bool			_bold;
        private bool			_italic;
        private bool			_underline;
        private bool			_strikeout;
        private bool			_subscript;
        private bool			_superscript;


        /// <summary>
        /// Property for the Name of the Font
        /// </summary>
        [Description("The Name of the Font")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        } //Name

        /// <summary>
        /// Property for the Size of the Font
        /// </summary>
        [Description("The Size of the Font")]
        public HtmlFontSize Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        } //Size

        /// <summary>
        /// Property for the Bold Indication of the Font
        /// </summary>
        [Description("Indicates if the font is Bold")]
        public bool Bold
        {
            get
            {
                return _bold;
            }
            set
            {
                _bold = value;
            }
        } //Bold

        /// <summary>
        /// Property for the Italics Indication of the Font
        /// </summary>
        [Description("Indicates if the font is Italic")]
        public bool Italic
        {
            get
            {
                return _italic;
            }
            set
            {
                _italic = value;
            }
        } //Italic

        /// <summary>
        /// Property for the Underline Indication of the Font
        /// </summary>
        [Description("Indicates if the font is Underline")]
        public bool Underline
        {
            get
            {
                return _underline;
            }
            set
            {
                _underline = value;
            }
        } //Underline

        /// <summary>
        /// Property for the Strikeout Indication of the Font
        /// </summary>
        [Description("Indicates if the font is Strikeout")]
        public bool Strikeout
        {
            get
            {
                return _strikeout;
            }
            set
            {
                _strikeout = value;
            }
        } //Strikeout

        /// <summary>
        /// Property for the Subscript Indication of the Font
        /// </summary>
        [Description("Indicates if the font is Subscript")]
        public bool Subscript
        {
            get
            {
                return _subscript;
            }
            set 
            {
                _subscript = value;
            }
        } //Subscript

        /// <summary>
        /// Property for the Superscript Indication of the Font
        /// </summary>
        [Description("Indicates if the font is Superscript")]
        public bool Superscript
        {
            get
            {
                return _superscript;
            }
            set
            {
                _superscript = value;
            }
        } //Superscript


        /// <summary>
        /// Public constrctor for name only
        /// </summary>
        public HtmlFontProperty(string name)
        {
            _name = name;
            _size = HtmlFontSize.Default;
            _bold = false;
            _italic = false;
            _underline = false;
            _strikeout = false;
            _subscript = false;
            _superscript = false;

        } //HtmlFontProperty

        /// <summary>
        /// Public constrctor for name and size only
        /// </summary>
        public HtmlFontProperty(string name, HtmlFontSize size)
        {
            _name = name;
            _size = size;
            _bold = false;
            _italic = false;
            _underline = false;
            _strikeout = false;
            _subscript = false;
            _superscript = false;

        } //HtmlFontProperty

        /// <summary>
        /// Public constrctor for all standard attributes
        /// </summary>
        public HtmlFontProperty(string name, HtmlFontSize size, bool bold, bool italic, bool underline)
        {
            _name = name;
            _size = size;
            _bold = bold;
            _italic = italic;
            _underline = underline;
            _strikeout = false;
            _subscript = false;
            _superscript = false;

        } //HtmlFontProperty

        /// <summary>
        /// Public constrctor for all attributes
        /// </summary>
        public HtmlFontProperty(string name, HtmlFontSize size, bool bold, bool italic, bool underline, bool strikeout, bool subscript, bool superscript)
        {
            _name = name;
            _size = size;
            _bold = bold;
            _italic = italic;
            _underline = underline;
            _strikeout = strikeout;
            _subscript = subscript;
            _superscript = superscript;

        } //HtmlFontProperty

        /// <summary>
        /// Public constructor given a system Font
        /// </summary>
        public HtmlFontProperty(System.Drawing.Font font)
        {
            _name = font.Name;
            _size = HtmlFontConversion.FontSizeToHtml(font.SizeInPoints);
            _bold = font.Bold;
            _italic = font.Italic;
            _underline = font.Underline;
            _strikeout = font.Strikeout;
            _subscript = false;
            _superscript = false;

        } //HtmlFontProperty


        /// <summary>
        /// Public method to convert the html into a readable format
        /// Used by the designer to display the font name
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, Size);

        } //ToString

        /// <summary>
        /// Compares two Html Fonts for equality
        /// Equality opertors not defined (Design Time issue with override of Equals)
        /// </summary>
        public static bool IsEqual(HtmlFontProperty font1, HtmlFontProperty font2)
        {
            // assume not equal
            bool equals = false;
            
            // perform the comparsion
            if (HtmlFontProperty.IsNotNull(font1) && HtmlFontProperty.IsNotNull(font2))
            {
                if (font1.Name == font2.Name &&
                    font1.Size == font2.Size &&
                    font1.Bold == font2.Bold && 
                    font1.Italic == font2.Italic &&
                    font1.Underline == font2.Underline &&
                    font1.Strikeout == font2.Strikeout &&
                    font1.Subscript == font2.Subscript &&
                    font1.Superscript == font2.Superscript)
                {
                    equals = true;
                }
            }
            
            // return the calculated value
            return equals;

        } //IsEquals

        /// <summary>
        /// Compares two Html Fonts for equality
        /// Equality opertors not defined (Design Time issue with override of Equals)
        /// </summary>
        public static bool IsNotEqual(HtmlFontProperty font1, HtmlFontProperty font2)
        {
            return (!HtmlFontProperty.IsEqual(font1, font2));

        } //IsNotEqual

        /// <summary>
        /// Based on a font name being null the font can be assumed to be null
        /// Default constructor will give a null object
        /// </summary>
        public static bool IsNull(HtmlFontProperty font)
        {
            return (font.Name == null || font.Name.Trim() == string.Empty);

        } //IsNull

        /// <summary>
        /// Based on a font name being null the font can be assumed to be null
        /// Default constructor will give a null object
        /// </summary>
        public static bool IsNotNull(HtmlFontProperty font)
        {
            return (!HtmlFontProperty.IsNull(font));

        } //IsNull

    } // HtmlFontProperty

    #endregion

    #region HtmlFontConversion utilities

    /// <summary>
    /// Utility Class to perform Font Attribute conversions
    /// Takes data to and from the expected Html Format
    /// </summary>
    public class HtmlFontConversion
    {

        /// <summary>
        /// Returns the correct string size description from a HtmlFontSize
        /// </summary>
        public static string HtmlFontSizeString(HtmlFontSize fontSize)
        {
            // set the size to blank as the default
            // this will ensure font size blanked out if not set
            string size = string.Empty;

            switch (fontSize)
            {
                case HtmlFontSize.xxSmall:
                    size = "xx-small";
                    break;
                case HtmlFontSize.xSmall:
                    size = "x-small";
                    break;
                case HtmlFontSize.Small:
                    size = "small";
                    break;
                case HtmlFontSize.Medium:
                    size = "medium";
                    break;
                case HtmlFontSize.Large:
                    size = "large";
                    break;
                case HtmlFontSize.xLarge:
                    size = "x-large";
                    break;
                case HtmlFontSize.xxLarge:
                    size = "xx-large";
                    break;
                case HtmlFontSize.Default:
                    size = string.Empty; //small
                    break;
            }

            // return the calculated size
            return size;

        } //HtmlFontSizeString

        /// <summary>
        /// Returns the correct bold description for the bold attribute
        /// </summary>
        public static string HtmlFontBoldString(bool fontBold)
        {
            return (fontBold?"Bold":"Normal");

        } //HtmlFontBoldString

        /// <summary>
        /// Returns the correct bold description for the bold attribute
        /// </summary>
        public static string HtmlFontItalicString(bool fontItalic)
        {
            return (fontItalic?"Italic":"Normal");

        } //HtmlFontItalicString


        /// <summary>
        /// Determines the font size given a selected font in points
        /// </summary>
        public static HtmlFontSize FontSizeToHtml(float fontSize)
        {
            // make the following mapping
            // 1:8pt
            // 2:10pt
            // 3:12pt
            // 4:14pt
            // 5:18pt
            // 6:24pt
            // 7:36pt
            int calcFont = 0;

            if (fontSize < 10) calcFont = 1;		// 8pt
            else if (fontSize < 12) calcFont = 2;	// 10pt
            else if (fontSize < 14) calcFont = 3;	// 12pt
            else if (fontSize < 18) calcFont = 4;	// 14pt
            else if (fontSize < 24) calcFont = 5;	// 24pt
            else if (fontSize < 36) calcFont = 6;	// 36pt
            else calcFont = 7;
        
            return (HtmlFontSize)calcFont;

        } //FontSizeToHtml


        /// <summary>
        /// Determines the font size given the html font size
        /// </summary>
        public static float FontSizeFromHtml(HtmlFontSize fontSize)
        {
            return HtmlFontConversion.FontSizeFromHtml((int)fontSize);

        } //FontSizeFromHtml

        /// <summary>
        /// Determines the font size given the html int size
        /// </summary>
        public static float FontSizeFromHtml(int fontSize)
        {
            // make the following mapping
            // 1:8pt
            // 2:10pt
            // 3:12pt
            // 4:14pt
            // 5:18pt
            // 6:24pt
            // 7:36pt
            float calcFont = 0;

            switch (fontSize)
            {
                case 1:
                    calcFont = 8F;
                    break;
                case 2:
                    calcFont = 10F;
                    break;
                case 3:
                    calcFont = 12F;
                    break;
                case 4:
                    calcFont = 14F;
                    break;
                case 5:
                    calcFont = 18F;
                    break;
                case 6:
                    calcFont = 24F;
                    break;
                case 7:
                    calcFont = 36F;
                    break;
                default:
                    calcFont = 12F;
                    break;
            }
        
            return calcFont;

        } //FontSizeFromHtml


        /// <summary>
        /// Determines the HtmlFontSize from a style attribute
        /// </summary>
        public static HtmlFontSize StyleSizeToHtml(string sizeDesc)
        {
            // currently assume the value is a fixed point
            // should take into account relative and absolute values
            float size;
            try
            {
                size = Single.Parse(Regex.Replace(sizeDesc, @"[^\d|\.]", ""));
            }
            catch (Exception)
            {
                // set size to zero to return HtmlFontSize.Default
                size = 0;
            }

            // return value as a HtmlFontSize
            return HtmlFontConversion.FontSizeToHtml(size);

        } //StyleSizeToHtml

        /// <summary>
        /// Determines if the style attribute is for Bold
        /// </summary>
        public static bool IsStyleBold(string style)
        {
            return Regex.IsMatch(style, "bold|bolder|700|800|900", RegexOptions.IgnoreCase);

        } //IsStyleBold

        /// <summary>
        /// Determines if the style attribute is for Italic
        /// </summary>
        public static bool IsStyleItalic(string style)
        {
            return Regex.IsMatch(style, "style|oblique", RegexOptions.IgnoreCase);

        } //IsStyleItalic

    } //HtmlFontConversion

    #endregion

    #region HtmlFontPropertyConverter class

    /// <summary>
    /// Expandable object converter for the HtmlFontProperty
    /// Allows it to be viewable from the property browser
    /// String format based on "Name, FontSize"
    /// </summary>
    public class HtmlFontPropertyConverter : ExpandableObjectConverter
    {
        // constants used for the property names
        private const string PROP_NAME = "Name";
        private const string PROP_SIZE = "Size";
        private const string PROP_BOLD = "Bold";
        private const string PROP_ITALIC = "Italic";
        private const string PROP_UNDERLINE = "Underline";
        private const string PROP_STRIKEOUT = "Strikeout";
        private const string PROP_SUBSCRIPT = "Subscript";
        private const string PROP_SUPERSCRIPT = "Superscript";

        // regular expression parse 
        private const string FONT_PARSE_EXPRESSION = @"^(?<name>(\w| )+)((\s*,\s*)?)(?<size>\w*)";
        private const string FONT_PARSE_NAME = @"${name}";
        private const string FONT_PARSE_SIZE = @"${size}";

        /// <summary>
        /// Allows expansion sub property change to have string updated
        /// </summary>
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            // always return a new instance
            return true;

        } //GetCreateInstanceSupported

        /// <summary>
        /// Creates a new HtmlFontProperty from a series of values
        /// </summary>
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary values)
        {
            // obtain the HtmlFontProperty properties
            string name = (string)values[PROP_NAME];
            HtmlFontSize size = (HtmlFontSize)values[PROP_SIZE];
            bool bold = (bool)values[PROP_BOLD];
            bool italic = (bool)values[PROP_ITALIC];
            bool underline = (bool)values[PROP_UNDERLINE];
            bool strikeout = (bool)values[PROP_STRIKEOUT];
            bool subscript = (bool)values[PROP_SUBSCRIPT];
            bool superscript = (bool)values[PROP_SUPERSCRIPT];
            // return the new HtmlFontProperty
            return new HtmlFontProperty(name, size, bold, italic, underline, strikeout, subscript, superscript);

        } //CreateInstance


        /// <summary>
        /// Indicates if a conversion can take place from a HtmlFontProperty
        /// </summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) 
        {
            if (destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }

        } //CanConvertTo

        /// <summary>
        /// Performs the conversion from HtmlFontProperty to a string (only)
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) 
        {
            // ensure working with the intented type HtmlFontProperty
            if (value is HtmlFontProperty)
            {
                HtmlFontProperty font = (HtmlFontProperty)value;
                if (destinationType == typeof(string))
                {
                    return font.ToString();
                }
                if (destinationType == typeof(InstanceDescriptor))
                {
                    // define array to hold the properties and values
                    Object[] properties = new Object[8];
                    Type[] types = new Type[8];
                    // Name property
                    properties[0] = font.Name;
                    types[0] = typeof(string);
                    // Size property
                    properties[1] = font.Size;
                    types[1] = typeof(HtmlFontSize);
                    // Bold property
                    properties[2] = font.Bold;
                    types[2] = typeof(bool);
                    // Italic property
                    properties[3] = font.Italic;
                    types[3] = typeof(bool);
                    // Underline property
                    properties[4] = font.Underline;
                    types[4] = typeof(bool);
                    // Strikeout property
                    properties[5] = font.Strikeout;
                    types[5] = typeof(bool);
                    // Subscript property
                    properties[6] = font.Subscript;
                    types[6] = typeof(bool);
                    // Superscript property
                    properties[7] = font.Superscript;
                    types[7] = typeof(bool);
                    // create the instance constructor to return
                    ConstructorInfo ci = typeof(HtmlFontProperty).GetConstructor(types);
                    return new InstanceDescriptor(ci, properties);
                }
            }

            // have something other than InstanceDescriptor or sting
            return base.ConvertTo(context, culture, value, destinationType);

        } //ConvertTo


        /// <summary>
        /// Indicates if a conversion can take place from s string
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) 
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }

        } //CanConvertFrom

        /// <summary>
        /// Performs the conversion from string to a HtmlFontProperty (only)
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) 
        {
            if (value is string) 
            {
                // define a new font property
                string fontString = (string)value;
                HtmlFontProperty font = new HtmlFontProperty(string.Empty);;
                try 
                {
                    // parse the contents of the given string using a regex
                    string fontName = string.Empty;
                    string fontSize = string.Empty;
                    Regex expression = new Regex(FONT_PARSE_EXPRESSION,  RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
                    Match match = expression.Match(fontString);
                    // see if a match was found
                    if (match.Success)
                    {
                        // extract the content type elements
                        fontName = match.Result(FONT_PARSE_NAME);
                        fontSize = match.Result(FONT_PARSE_SIZE);
                        // set the fontname
                        TextInfo text = Thread.CurrentThread.CurrentCulture.TextInfo;
                        font.Name = text.ToTitleCase(fontName);
                        // determine size from given string using Small if blank
                        if (fontSize == string.Empty) fontSize = "Small";
                        font.Size = (HtmlFontSize)Enum.Parse(typeof(HtmlFontSize), fontSize, true);
                    }
                }
                catch (Exception)
                {
                    // do nothing but ensure font is a null font
                    font.Name = string.Empty;
                }
                if (HtmlFontProperty.IsNull(font))
                {
                    // error performing the string conversion so throw exception given possible format
                    string error = string.Format(@"Cannot convert '{0}' to Type HtmlFontProperty. Format: 'FontName, HtmlSize', Font Size values: {1}", fontString, string.Join(", ", Enum.GetNames(typeof(HtmlFontSize))));
                    throw new ArgumentException(error);
                }
                else
                {
                    // return the font
                    return font;
                }
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }

        } //ConvertFrom

    } //HtmlFontPropertyConverter

    #endregion
    
}