﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BlackMesa.Website.Main.Utilities
{
    public static class Utilities
    {


        public static HtmlHelper GetHtmlHelper(this Controller controller)
        {
            var viewContext = new ViewContext(controller.ControllerContext, new FakeView(), controller.ViewData, controller.TempData, TextWriter.Null);
            return new HtmlHelper(viewContext, new ViewPage());
        }

        public class FakeView : IView
        {
            public void Render(ViewContext viewContext, TextWriter writer)
            {
                throw new InvalidOperationException();
            }
        }

        private static readonly long DatetimeMinTimeTicks =
           (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

        public static long ToJavaScriptMilliseconds(this DateTime dt)
        {
            return (long)((dt.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
        }

        public static string ToJson(this object obj)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static string ToJson(this object obj, int recursionDepth)
        {
            var serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Serialize(obj);
        }


        public static void EnumerateAllIncludesList(DbContext context, IEnumerable entities, List<object> entitiesLoaded = null)
        {
            if (entitiesLoaded == null)
                entitiesLoaded = new List<object>();

            foreach (var entity in entities)
                EnumerateAllIncludesEntity(context, entity, entitiesLoaded);

        }

        public static void EnumerateAllIncludesEntity(DbContext context, object entity, List<object> entitiesLoaded)
        {
            if (entitiesLoaded.Contains(entity))
                return;

            entitiesLoaded.Add(entity);

            Type type = entity.GetType();
            var properties = type.GetProperties();

            foreach (var propertyInfo in properties)
            {
                var propertyType = propertyInfo.PropertyType;

                bool isCollection = propertyType.GetInterfaces().Any(x => x == typeof(IEnumerable)) &&
                                    !propertyType.Equals(typeof(string));

                if (isCollection)
                {
                    context.Entry(entity).Collection(propertyInfo.Name).Load();

                    var propertyValue = propertyInfo.GetValue(entity);

                    if (propertyValue == null)
                        continue;

                    EnumerateAllIncludesList(context, (IEnumerable)propertyValue, entitiesLoaded);
                }
                else if ((!propertyType.IsValueType && !propertyType.Equals(typeof(string))))
                {
                    context.Entry(entity).Reference(propertyInfo.Name).Load();

                    var propertyValue = propertyInfo.GetValue(entity);

                    if (propertyValue == null)
                        continue;

                    EnumerateAllIncludesEntity(context, propertyValue, entitiesLoaded);
                }
                else
                    continue;
            }
        }



        private static String ReplaceGermanUmlauts(String s)
        {
            String t = s;
            t = t.Replace("ä", "ae");
            t = t.Replace("ö", "oe");
            t = t.Replace("ü", "ue");
            t = t.Replace("Ä", "Ae");
            t = t.Replace("Ö", "Oe");
            t = t.Replace("Ü", "Ue");
            t = t.Replace("ß", "ss");
            return t;
        }


        /// <summary>
        /// Produces optional, URL-friendly version of a title, "like-this-one". 
        /// hand-tuned for speed, reflects performance refactoring contributed
        /// by John Gietzen (user otac0n) 
        /// </summary>
        public static string MakeUrlFriendly(string title)
        {
            if (title == null) return "";
            title = ReplaceGermanUmlauts(title);
            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }


        private static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }


    }
}