using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ExCSS;
using System.Collections;

namespace MidnightWax.TextStyler.Core
{
	public static class TextStyleParser
	{
		public static Dictionary<string, TextStyleParameters> Parse (string css)
		{
			var parser = new Parser ();
			var stylesheet = parser.Parse (css);

			var textProperties = typeof(TextStyleParameters).GetRuntimeProperties ()
				.Select (p => new {	p,	attr = p.GetCustomAttributes (typeof(CssAttribute), true)})
				.Where (prop => prop.attr.Count () == 1)
				.Select (obj => new {Property = obj.p, Attribute = obj.attr.First () as CssAttribute})
				.ToDictionary (t => t.Attribute.Name, t => t.Property);

			var textStyles = new Dictionary<string, TextStyleParameters> ();

			foreach (var styleRule in stylesheet.StyleRules) {
//				var selectors = (SelectorList)styleRule.Selector;
//				for (int i = 0; i < selectors.Length; i++) {
//					var selectorName = selectors [i].ToString ();
//				}

				var curStyle = new TextStyleParameters (styleRule.Selector.ToString ());
				foreach (var declaration in styleRule.Declarations.Properties) {
					if (textProperties.ContainsKey (declaration.Name)) {
						var term = declaration.Term;

						 
//						var cleanedValue = declaration.Term.Replace ("\"", "");
						var prop = textProperties [declaration.Property];
						switch (prop.PropertyType.Name) {
						case "String":
							curStyle [prop.Name] = cleanedValue;
							break;
						case "Int32":
							int numInt;
							if (int.TryParse (cleanedValue, out numInt)) {
								curStyle [prop.Name] = numInt;
							} 
							//								else 
							//									Console.WriteLine ("Failed to Parse Int32 value {0}", cleanedValue);
							break;
						case "Single":
							cleanedValue = cleanedValue.Replace ("px", "");
							float numFloat;
							if (float.TryParse (cleanedValue, out numFloat)) {
								curStyle [prop.Name] = numFloat;
							} else
								throw new Exception ("Failed to Parse Single value " + cleanedValue);
							break;
						case "Single[]":
							var parts = cleanedValue.Split (new char[]{ ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
							var parsedValues = new float[parts.Length];
							for (int i = 0; i < parts.Length; i++) {
								float numArrayFloat;
								if (float.TryParse (parts [i], out numArrayFloat)) {
									parsedValues [i] = numArrayFloat;
								}
							}
							curStyle [prop.Name] = parsedValues;
							break;
						case "TextStyleAlign":
							curStyle.TextAlign = EnumUtils.FromDescription<TextStyleAlign> (cleanedValue);
							break;
						case "TextStyleDecoration":
							curStyle.TextDecoration = EnumUtils.FromDescription<TextStyleDecoration> (cleanedValue);
							break;
						case "TextStyleTextTransform":
							curStyle.TextTransform = EnumUtils.FromDescription<TextStyleTextTransform> (cleanedValue);
							break;
						case "TextStyleTextOverflow":
							curStyle.TextOverflow = EnumUtils.FromDescription<TextStyleTextOverflow> (cleanedValue);
							break;
						default:
							throw new InvalidCastException ("Could not find the appropriate type " + prop.PropertyType.Name);
						}
					}
				}

				// Add it to the dictionary
				textStyles [curStyle.Name] = curStyle;
			}

			return textStyles;
		}

		private static void ParseRule ()
		{
			
		}

		public static string ParseToCSSString (string tagName, TextStyleParameters style)
		{
			var builder = new StringBuilder ();
			builder.Append (tagName + "{");

			foreach (var prop in style.GetType().GetRuntimeProperties()) {
				try {
					var value = prop.GetValue (style, null);

					if (value != null) {
						string parsedValue = null;
						switch (prop.PropertyType.Name) {
						case "String":
							if ((value as string).StartsWith ("#"))
								parsedValue = (string)value;
							else
								parsedValue = "'" + value + "'";
							break;
						case "Single":
							if (Convert.ToSingle (value) > float.MinValue) {
								parsedValue = Convert.ToString (value);
								if (prop.Name == "FontSize") // Dirty, I really need a list of things that can be set in pixel values
									parsedValue += "px";
							}
							break;
						case "Int32":
							if (Convert.ToInt32 (value) > int.MinValue)
								parsedValue = Convert.ToString (value);
							break;
						case "Single[]":
							parsedValue = Convert.ToString (value);
							break;
						case "TextStyleAlign":
						case "TextStyleDecoration":
						case "TextStyleTextTransform":
						case "TextStyleTextOverflow":
							var cast = Convert.ToString (value);
							if (cast != "None")
								parsedValue = cast.ToLower ();
							break;
						default:
							throw new InvalidCastException ("Could not find the appropriate type " + prop.PropertyType.Name);
						}

						var attributes = (CssAttribute[])prop.GetCustomAttributes (
							                 typeof(CssAttribute), false);

						if (attributes.Length > 0 && parsedValue != null)
							builder.Append (attributes [0].Name + ":" + parsedValue + ";");
					}
				} catch (Exception ex) {
					//throw ex;
				}
			}

			builder.Append ("}");

			return builder.ToString ();
		}
	}
}

