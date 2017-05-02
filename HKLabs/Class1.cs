namespace System.ComponentModel
{
    using Microsoft.Win32;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting;
    using System.Runtime.Serialization.Formatters;
    using System.Security.Permissions;
    using System.Reflection;
    /// <devdoc>
    ///    <para>Provides a type converter to convert object references to and from various
    ///       other representations.</para>
    /// </devdoc>
    [HostProtection(SharedState = true)]
    public class TypeConverterTable2String : TypeConverter
    {


        /// <internalonly/>
        /// <devdoc>
        ///    <para>Gets a value indicating whether this converter can convert an object in the
        ///       given source type to a reference object using the specified context.</para>
        /// </devdoc>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        /// <internalonly/>
        /// <devdoc>
        ///    <para>Converts the given object to the reference type.</para>
        /// </devdoc>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string text = ((string)value).Trim();

                if (!String.Equals(text, ""))
                {
                    Type[] t = Assembly.GetExecutingAssembly().GetTypes();
                    if (t != null)
                    {
                        int len = t.Length;
                        for (int i = 0; i < len; i++)
                        {
                            if (t[i].Name == text)
                            {
                                return t[i];
                            }
                        }
                    }
                }
                return null;
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <internalonly/>
        /// <devdoc>
        ///    <para>Converts the given value object to the reference type
        ///       using the specified context and arguments.</para>
        /// </devdoc>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    if (value is Type)
                    {
                        return ((Type)value).Name;
                    }
                }
                return "";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <internalonly/>
        /// <devdoc>
        ///    <para>Gets a collection of standard values for the reference data type.</para>
        /// </devdoc>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            System.Collections.Generic.List<Type> result = new Collections.Generic.List<Type>();
            Type[] t = Assembly.GetExecutingAssembly().GetTypes();
            if (t != null)
            {
                for (int i = 0; i < t.Length; i++)
                {
                    //if (t[i].BaseType.Name == typeof(HKLabs.Tabel).Name ||
                    //    t[i].Name == typeof(HKLabs.Tabel).Name)
                    //{
                        result.Add(t[i]);
                    //}
                }
            }
            result.Sort((itemName1, itemName2) =>
                {
                    return string.Compare(itemName1.Name, itemName2.Name, false, CultureInfo.InvariantCulture);
                });
            return new StandardValuesCollection(result.ToArray());
        }
        /// <internalonly/>
        /// <devdoc>
        ///    <para>Gets a value indicating whether the list of standard values returned from
        ///    <see cref='System.ComponentModel.ReferenceConverter.GetStandardValues'/> is an exclusive list. </para>
        /// </devdoc>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <internalonly/>
        /// <devdoc>
        ///    <para>Gets a value indicating whether this object supports a standard set of values
        ///       that can be picked from a list.</para>
        /// </devdoc>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <devdoc>
        ///    <para>Gets a value indicating whether a particular value can be added to
        ///       the standard values collection.</para>
        /// </devdoc>
        protected virtual bool IsValueAllowed(ITypeDescriptorContext context, object value)
        {
            return true;
        }
    }
}