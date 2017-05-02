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
    public class TypeConverterPro : TypeConverter
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
                    if (context.Instance is System.Windows.Forms.Control)
                    {
                        System.Windows.Forms.Control tmp = (System.Windows.Forms.Control)context.Instance;
                        System.Windows.Forms.Form frm = null;
                        while (frm == null && tmp.Parent != null)
                        {
                            if (tmp.Parent is System.Windows.Forms.Form)
                            {
                                frm = (System.Windows.Forms.Form)tmp.Parent;
                            }
                            else
                            {
                                tmp = tmp.Parent;
                            }
                        }
                        if (frm != null && frm is HKLabs.form3)
                        {
                            HKLabs.form3 f3 = (HKLabs.form3)frm;
                            if (f3.AAA != null)
                            {
                                PropertyInfo[] p = f3.AAA.GetProperties();
                                foreach (PropertyInfo tmpP in p)
                                {
                                    if (tmpP.CanWrite && tmpP.CanRead && tmpP.Name==text)
                                    {
                                        return tmpP;
                                    }
                                }
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
                    if (value is PropertyInfo)
                    {
                        return ((PropertyInfo)value).Name;
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
            System.Collections.Generic.List<PropertyInfo> result = new Collections.Generic.List<PropertyInfo>();

            if (context.Instance is System.Windows.Forms.Control)
            {
                System.Windows.Forms.Control tmp = (System.Windows.Forms.Control)context.Instance;
                System.Windows.Forms.Form frm = null;
                while (frm == null && tmp.Parent != null)
                {
                    if (tmp.Parent is System.Windows.Forms.Form)
                    {
                        frm = (System.Windows.Forms.Form)tmp.Parent;
                    }
                    else
                    {
                        tmp = tmp.Parent;
                    }
                }
                if (frm != null && frm is HKLabs.form3)
                {
                    HKLabs.form3 f3 = (HKLabs.form3)frm;
                    if (f3.AAA != null)
                    {
                        PropertyInfo[] p = f3.AAA.GetProperties();
                        foreach (PropertyInfo tmpP in p)
                        {
                            if (tmpP.CanWrite && tmpP.CanRead)
                            {
                                result.Add(tmpP);
                            }
                        }
                    }
                }
            }
            result.Sort((p1, p2) =>
                {
                    return String.Compare(p1.Name, p2.Name, false, CultureInfo.InvariantCulture);
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