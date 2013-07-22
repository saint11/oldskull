using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Monocle
{
    static public class Calc
    {
#if DESKTOP
        public const string LOADPATH = @"Assets\";
#elif OUYA
        public const string LOADPATH = "";
#endif


        #region Enums

        static public int EnumLength(Type e)
        {
            return Enum.GetNames(e).Length;
        }

        static public T StringToEnum<T>(string str) where T : struct
        {
            if (Enum.IsDefined(typeof(T), str))
                return (T)Enum.Parse(typeof(T), str);
            else
                throw new Exception("The string cannot be converted to the enum type.");
        }

        #endregion

        #region Strings

        static public bool StartsWith(this string str, string match)
        {
            return str.IndexOf(match) == 0;
        }

        static public bool EndsWith(this string str, string match)
        {
            return str.IndexOf(match) == str.Length - match.Length;
        }

        static public string ToString(this int num, int minDigits)
        {
            string ret = num.ToString();
            while (ret.Length < minDigits)
                ret = "0" + ret;
            return ret;
        }

        #endregion

        #region Give Me

        static public T GiveMe<T>(int index, T a, T b)
        {
            switch (index)
            {
                default:
                    throw new Exception("Index was out of range!");

                case 0:
                    return a;
                case 1:
                    return b;
            }
        }

        static public T GiveMe<T>(int index, T a, T b, T c)
        {
            switch (index)
            {
                default:
                    throw new Exception("Index was out of range!");

                case 0:
                    return a;
                case 1:
                    return b;
                case 2:
                    return c;
            }
        }

        static public T GiveMe<T>(int index, T a, T b, T c, T d)
        {
            switch (index)
            {
                default:
                    throw new Exception("Index was out of range!");

                case 0:
                    return a;
                case 1:
                    return b;
                case 2:
                    return c;
                case 3:
                    return d;
            }
        }

        static public T GiveMe<T>(int index, T a, T b, T c, T d, T e)
        {
            switch (index)
            {
                default:
                    throw new Exception("Index was out of range!");

                case 0:
                    return a;
                case 1:
                    return b;
                case 2:
                    return c;
                case 3:
                    return d;
                case 4:
                    return e;
            }
        }

        #endregion

        #region Random

        static public Random Random = new Random();
        static private Stack<Random> randomStack = new Stack<Random>();

        static public void PushRandom(int newSeed)
        {
            randomStack.Push(Calc.Random);
            Calc.Random = new Random(newSeed);
        }

        static public void PushRandom()
        {
            randomStack.Push(Calc.Random);
            Calc.Random = new Random();
        }

        static public void PopRandom()
        {
            Calc.Random = randomStack.Pop();
        }

        static public T Choose<T>(this Random random, params T[] choices)
        {
            return choices[random.Next(choices.Length)];
        }

        static public T Choose<T>(this Random random, List<T> choices)
        {
            return choices[random.Next(choices.Count)];
        }

        static public int Range(this Random random, int min, int add)
        {
            return min + random.Next(add);
        }

        static public float Range(this Random random, float min, float add)
        {
            return min + add * random.NextFloat();
        }

        static public Vector2 Range(this Random random, Vector2 min, Vector2 add)
        {
            return min + new Vector2(add.X * random.NextFloat(), add.Y * random.NextFloat());
        }

        static public bool Chance(this Random random, float chance)
        {
            return random.NextFloat() < chance;
        }

        static public float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        static public float NextFloat(this Random random, float max)
        {
            return random.NextFloat() * max;
        }

        static public float NextAngle(this Random random)
        {
            return random.NextFloat() * MathHelper.TwoPi;
        }

        #endregion

        #region Lists

        static public void Shuffle<T>(this List<T> list, Random random)
        {
            int i = list.Count;
            int j;
            T t;

            while (--i > 0)
            {
                t = list[i];
                list[i] = list[j = random.Next(i + 1)];
                list[j] = t;
            }
        }

        static public void Shuffle<T>(this List<T> list)
        {
            list.Shuffle(Random);
        }

        #endregion

        #region Colors

        static public Color Invert(this Color color)
        {
            return new Color(255 - color.R, 255 - color.G, 255 - color.B, color.A);
        }

        static public Color HexToColor(string hex)
        {
            float r = (HexToByte(hex[0]) * 16 + HexToByte(hex[1])) / 255.0f;
            float g = (HexToByte(hex[2]) * 16 + HexToByte(hex[3])) / 255.0f;
            float b = (HexToByte(hex[4]) * 16 + HexToByte(hex[5])) / 255.0f;

            return new Color(r, g, b);
        }

        #endregion

        #region Math

        public const float RIGHT = 0;
        public const float UP = MathHelper.Pi * -.5f;
        public const float LEFT = MathHelper.Pi;
        public const float DOWN = MathHelper.Pi * .5f;
        public const float UP_RIGHT = MathHelper.Pi * -.25f;
        public const float UP_LEFT = MathHelper.Pi * -.75f;
        public const float DOWN_RIGHT = MathHelper.Pi * .25f;
        public const float DOWN_LEFT = MathHelper.Pi * .75f;
        public const float DEG_TO_RAD = (float)Math.PI / 180f;
        public const float RAD_TO_DEG = 180f / (float)Math.PI;
        public const float DtR = DEG_TO_RAD;
        public const float RtD = RAD_TO_DEG;
        private const string HEX = "0123456789ABCDEF";

        static public byte HexToByte(char c)
        {
            return (byte)HEX.IndexOf(char.ToUpper(c));
        }

        static public float Min(params float[] values)
        {
            float min = values[0];
            for (int i = 1; i < values.Length; i++)
                min = MathHelper.Min(values[i], min);
            return min;
        }

        static public float Max(params float[] values)
        {
            float max = values[0];
            for (int i = 1; i < values.Length; i++)
                max = MathHelper.Max(values[i], max);
            return max;
        }

        static public float ToRad(this float f)
        {
            return f * DEG_TO_RAD;
        }

        static public float ToDeg(this float f)
        {
            return f * RAD_TO_DEG;
        }

        static public int Clamp(int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        static public float YoYo(float value)
        {
            if (value <= .5f)
                return value * 2;
            else
                return 1 - ((value - .5f) * 2);
        }

        static public float LerpSnap(float value1, float value2, float amount, float snapThreshold = .1f)
        {
            float ret = MathHelper.Lerp(value1, value2, amount);
            if (Math.Abs(ret - value2) < snapThreshold)
                return value2;
            else
                return ret;
        }

        static public Vector2 LerpSnap(Vector2 value1, Vector2 value2, float amount, float snapThresholdSq = .1f)
        {
            Vector2 ret = Vector2.Lerp(value1, value2, amount);
            if ((ret - value2).LengthSquared() < snapThresholdSq)
                return value2;
            else
                return ret;
        }

        static public float ReflectAngle(float angle, float axis = 0)
        {
            return -(angle + axis) - axis;
        }

        static public float ReflectAngle(float angleRadians, Vector2 axis)
        {
            return ReflectAngle(angleRadians, axis.Angle());
        }

        static public Vector2 ClosestPointOnLine(Vector2 lineA, Vector2 lineB, Vector2 closestTo)
        {
            Vector2 v = lineB - lineA;
            Vector2 w = closestTo - lineA;
            float t = Vector2.Dot(w, v) / Vector2.Dot(v, v);
            t = MathHelper.Clamp(t, 0, 1);

            return lineA + v * t;
        }

        static public float Snap(float value, float increment)
        {
            return (float)Math.Round(value / increment) * increment;
        }

        static public float Snap(float value, float increment, float offset)
        {
            return ((float)Math.Round((value - offset) / increment) * increment) + offset;
        }

        static public float WrapAngleDeg(float angleDegrees)
        {
            return (((angleDegrees * Math.Sign(angleDegrees) + 180) % 360) - 180) * Math.Sign(angleDegrees);
        }

        static public float WrapAngle(float angleRadians)
        {
            return (((angleRadians * Math.Sign(angleRadians) + MathHelper.Pi) % (MathHelper.Pi * 2)) - MathHelper.Pi) * Math.Sign(angleRadians);
        }

        static public Vector2 AngleToVector(float angleRadians, float length)
        {
            return new Vector2((float)Math.Cos(angleRadians) * length, (float)Math.Sin(angleRadians) * length);
        }

        static public float AngleApproach(float val, float target, float maxMove)
        {
            return val + MathHelper.Clamp(AngleDifference(val, target), -maxMove, maxMove);
        }

        static public float AngleLerp(float startAngle, float endAngle, float percent)
        {
            return startAngle + AngleDifference(startAngle, endAngle) * percent;
        }

        static public float Approach(float val, float target, float maxMove)
        {
            return val > target ? Math.Max(val - maxMove, target) : Math.Min(val + maxMove, target);
        }

        static public float AngleDifference(float radiansA, float radiansB)
        {
            float diff = radiansB - radiansA;

            while (diff > MathHelper.Pi) { diff -= MathHelper.TwoPi; }
            while (diff <= -MathHelper.Pi) { diff += MathHelper.TwoPi; }

            return diff;
        }

        static public float Angle(Vector2 from, Vector2 to)
        {
            return (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
        }

        static public Color ToggleColors(Color current, Color a, Color b)
        {
            if (current == a)
                return b;
            else
                return a;
        }

        static public float ShorterAngleDifference(float currentAngle, float angleA, float angleB)
        {
            if (Math.Abs(Calc.AngleDifference(currentAngle, angleA)) < Math.Abs(Calc.AngleDifference(currentAngle, angleB)))
                return angleA;
            else
                return angleB;
        }

        #endregion

        #region Vector2

        static public float Angle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        static public Vector2 Clamp(this Vector2 val, float minX, float minY, float maxX, float maxY)
        {
            return new Vector2(MathHelper.Clamp(val.X, minX, maxX), MathHelper.Clamp(val.Y, minY, maxY));
        }

        static public Vector2 Floor(this Vector2 val)
        {
            return new Vector2((int)val.X, (int)val.Y);
        }

        static public Vector2 Ceiling(this Vector2 val)
        {
            return new Vector2((int)Math.Ceiling(val.X), (int)Math.Ceiling(val.Y));
        }

        static public Vector2 Abs(this Vector2 val)
        {
            return new Vector2(Math.Abs(val.X), Math.Abs(val.Y));
        }

        static public Vector2 Approach(this Vector2 val, Vector2 target, float maxMove)
        {
            if (maxMove == 0 || val == target)
                return val;

            Vector2 diff = target - val;
            float length = diff.Length();

            return val + ((diff / length) * Math.Min(length, maxMove));
        }

        static public Vector2 EightWayNormal(this Vector2 vec)
        {
            float angle = vec.Angle();
            angle = (float)Math.Floor((angle + MathHelper.PiOver4 / 2f) / MathHelper.PiOver4) * MathHelper.PiOver4;
            return AngleToVector(angle, 1f);
        }

        static public Vector2 Rotate(this Vector2 vec, float angleRadians)
        {
            return AngleToVector(vec.Angle() + angleRadians, vec.Length());
        }

        #endregion

        #region Data Parse

        static public string[] SplitLines(string text, SpriteFont font, int maxLineWidth, char newLine = '\n')
        {
            List<string> lines = new List<string>();

            foreach (var forcedLine in text.Split(newLine))
            {
                string line = "";

                foreach (string word in forcedLine.Split(' '))
                {
                    if (font.MeasureString(line + " " + word).X > maxLineWidth)
                    {
                        lines.Add(line);
                        line = word;
                    }
                    else
                    {
                        if (line != "")
                            line += " ";
                        line += word;
                    }
                }

                lines.Add(line);
            }

            return lines.ToArray();
        }

        static public int[,] ReadCSVGrid(string csv)
        {
            int longest = 0;
            List<string[]> lines = new List<string[]>();
            foreach (string line in csv.Split('\n'))
            {
                string[] tiles = line.Split(',');
                lines.Add(tiles);
                longest = Math.Max(longest, tiles.Length);
            }

            int[,] data = new int[longest, lines.Count];
            for (int y = 0; y < lines.Count; y++)
            {
                int x = 0;
                if (lines[y][x] != "")
                    for (x = 0; x < lines[y].Length; x++)
                        data[x, y] = Convert.ToInt32(lines[y][x]);
                for (; x < longest; x++)
                    data[x, y] = -1;
            }

            return data;
        }

        static public int[] ReadCSV(string csv)
        {
            if (csv == "")
                return new int[0];

            string[] values = csv.Split(',');
            int[] ret = new int[values.Length];

            for (int i = 0; i < values.Length; i++)
                ret[i] = Convert.ToInt32(values[i].Trim());

            return ret;
        }

        static public bool[,] GetBitData(string data, char rowSep = '\n')
        {
            int lengthX = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == '1' || data[i] == '0')
                    lengthX++;
                else if (data[i] == rowSep)
                    break;
            }

            int lengthY = data.Count(c => c == '\n') + 1;

            bool[,] bitData = new bool[lengthX, lengthY];
            int x = 0;
            int y = 0;
            for (int i = 0; i < data.Length; i++)
            {
                switch (data[i])
                {
                    case '1':
                        bitData[x, y] = true;
                        x++;
                        break;

                    case '0':
                        bitData[x, y] = false;
                        x++;
                        break;

                    case '\n':
                        x = 0;
                        y++;
                        break;

                    default:
                        break;
                }
            }

            return bitData;
        }

        static public void AddBitData(bool[,] bitData, string data, char rowSep = '\n')
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < data.Length; i++)
            {
                switch (data[i])
                {
                    case '1':
                        bitData[x, y] = true;
                        x++;
                        break;

                    case '0':
                        x++;
                        break;

                    case '\n':
                        x = 0;
                        y++;
                        break;

                    default:
                        break;
                }
            }
        }

        static public int[] ConvertStringArrayToInt(string[] strings)
        {
            int[] ret = new int[strings.Length];
            for (int i = 0; i < strings.Length; i++)
                ret[i] = Convert.ToInt32(strings[i]);
            return ret;
        }

        static public float[] ConvertStringArrayToFloat(string[] strings)
        {
            float[] ret = new float[strings.Length];
            for (int i = 0; i < strings.Length; i++)
                ret[i] = Convert.ToSingle(strings[i], CultureInfo.InvariantCulture);
            return ret;
        }

        #endregion

        #region XML

        static public XmlDocument LoadXML(string filename)
        {
            XmlDocument xml = new XmlDocument();
#if DESKTOP
            xml.Load(filename);
#elif OUYA
            Stream stream = TitleContainer.OpenStream(filename);
            xml.Load(stream);
            stream.Close();    
#endif
            return xml;
        }

        #region Attributes

        static public bool HasAttr(this XmlElement xml, string attributeName)
        {
            return xml.Attributes[attributeName] != null;
        }

        static public string Attr(this XmlElement xml, string attributeName)
        {
#if DEBUG
            if (!xml.HasAttr(attributeName))
                throw new Exception("Element does not contain the attribute \"" + attributeName + "\"");
#endif
            return xml.Attributes[attributeName].InnerText;
        }

        static public string Attr(this XmlElement xml, string attributeName, string defaultValue)
        {
            if (!xml.HasAttr(attributeName))
                return defaultValue;
            else
                return xml.Attributes[attributeName].InnerText;
        }

        static public int AttrInt(this XmlElement xml, string attributeName)
        {
#if DEBUG
            if (!xml.HasAttr(attributeName))
                throw new Exception("Element does not contain the attribute \"" + attributeName + "\"");
#endif
            return Convert.ToInt32(xml.Attributes[attributeName].InnerText);
        }

        static public int AttrInt(this XmlElement xml, string attributeName, int defaultValue)
        {
            if (!xml.HasAttr(attributeName))
                return defaultValue;
            else
                return Convert.ToInt32(xml.Attributes[attributeName].InnerText);
        }

        static public float AttrFloat(this XmlElement xml, string attributeName)
        {
#if DEBUG
            if (!xml.HasAttr(attributeName))
                throw new Exception("Element does not contain the attribute \"" + attributeName + "\"");
#endif
            return Convert.ToSingle(xml.Attributes[attributeName].InnerText, CultureInfo.InvariantCulture);
        }

        static public float AttrFloat(this XmlElement xml, string attributeName, float defaultValue)
        {
            if (!xml.HasAttr(attributeName))
                return defaultValue;
            else
                return Convert.ToSingle(xml.Attributes[attributeName].InnerText, CultureInfo.InvariantCulture);
        }

        static public bool AttrBool(this XmlElement xml, string attributeName)
        {
#if DEBUG
            if (!xml.HasAttr(attributeName))
                throw new Exception("Element does not contain the attribute \"" + attributeName + "\"");
#endif
            return Convert.ToBoolean(xml.Attributes[attributeName].InnerText);
        }

        static public bool AttrBool(this XmlElement xml, string attributeName, bool defaultValue)
        {
            if (!xml.HasAttr(attributeName))
                return defaultValue;
            else
                return AttrBool(xml, attributeName);
        }

        static public T AttrEnum<T>(this XmlElement xml, string attributeName) where T : struct
        {
#if DEBUG
            if (!xml.HasAttr(attributeName))
                throw new Exception("Element does not contain the attribute \"" + attributeName + "\"");
#endif
            if (Enum.IsDefined(typeof(T), xml.Attributes[attributeName].InnerText))
                return (T)Enum.Parse(typeof(T), xml.Attributes[attributeName].InnerText);
            else
                throw new Exception("The attribute value cannot be converted to the enum type.");
        }

        static public T AttrEnum<T>(this XmlElement xml, string attributeName, T defaultValue) where T : struct
        {
            if (!xml.HasAttr(attributeName))
                return defaultValue;
            else
                return xml.AttrEnum<T>(attributeName);
        }

        static public Color AttrHexColor(this XmlElement xml, string attributeName)
        {
#if DEBUG
            if (!xml.HasAttr(attributeName))
                throw new Exception("Element does not contain the attribute \"" + attributeName + "\"");
#endif
            return Calc.HexToColor(xml.Attr(attributeName));
        }

        static public Color AttrHexColor(this XmlElement xml, string attributeName, Color defaultValue)
        {
            if (!xml.HasAttr(attributeName))
                return defaultValue;
            else
                return AttrHexColor(xml, attributeName);
        }

        static public Color AttrHexColor(this XmlElement xml, string attributeName, string defaultValue)
        {
            if (!xml.HasAttr(attributeName))
                return Calc.HexToColor(defaultValue);
            else
                return AttrHexColor(xml, attributeName);
        }

        static public Vector2 Position(this XmlElement xml)
        {
            return new Vector2(xml.AttrFloat("x"), xml.AttrFloat("y"));
        }

        static public Vector2 Position(this XmlElement xml, Vector2 defaultPosition)
        {
            return new Vector2(xml.AttrFloat("x", defaultPosition.X), xml.AttrFloat("y", defaultPosition.Y));
        }

        #endregion

        #region Inner Text

        static public int InnerInt(this XmlElement xml)
        {
            return Convert.ToInt32(xml.InnerText);
        }

        static public float InnerFloat(this XmlElement xml)
        {
            return Convert.ToSingle(xml.InnerText, CultureInfo.InvariantCulture);
        }

        static public bool InnerBool(this XmlElement xml)
        {
            return Convert.ToBoolean(xml.InnerText);
        }

        static public T InnerEnum<T>(this XmlElement xml) where T : struct
        {
            if (Enum.IsDefined(typeof(T), xml.InnerText))
                return (T)Enum.Parse(typeof(T), xml.InnerText);
            else
                throw new Exception("The attribute value cannot be converted to the enum type.");
        }

        static public Color InnerHexColor(this XmlElement xml)
        {
            return Calc.HexToColor(xml.InnerText);
        }

        #endregion

        #region Child Inner Text

        static public bool HasChild(this XmlElement xml, string childName)
        {
            return xml[childName] != null;
        }

        static public string ChildText(this XmlElement xml, string childName)
        {
#if DEBUG
            if (!xml.HasChild(childName))
                throw new Exception("Cannot find child xml tag with name '" + childName + "'.");
#endif
            return xml[childName].InnerText;
        }

        static public string ChildText(this XmlElement xml, string childName, string defaultValue)
        {
            if (xml.HasChild(childName))
                return xml[childName].InnerText;
            else
                return defaultValue;
        }

        static public int ChildInt(this XmlElement xml, string childName)
        {
#if DEBUG
            if (!xml.HasChild(childName))
                throw new Exception("Cannot find child xml tag with name '" + childName + "'.");
#endif
            return xml[childName].InnerInt();
        }

        static public int ChildInt(this XmlElement xml, string childName, int defaultValue)
        {
            if (xml.HasChild(childName))
                return xml[childName].InnerInt();
            else
                return defaultValue;
        }

        static public float ChildFloat(this XmlElement xml, string childName)
        {
#if DEBUG
            if (!xml.HasChild(childName))
                throw new Exception("Cannot find child xml tag with name '" + childName + "'.");
#endif
            return xml[childName].InnerFloat();
        }

        static public float ChildFloat(this XmlElement xml, string childName, float defaultValue)
        {
            if (xml.HasChild(childName))
                return xml[childName].InnerFloat();
            else
                return defaultValue;
        }

        static public bool ChildBool(this XmlElement xml, string childName)
        {
#if DEBUG
            if (!xml.HasChild(childName))
                throw new Exception("Cannot find child xml tag with name '" + childName + "'.");
#endif
            return xml[childName].InnerBool();
        }

        static public bool ChildBool(this XmlElement xml, string childName, bool defaultValue)
        {
            if (xml.HasChild(childName))
                return xml[childName].InnerBool();
            else
                return defaultValue;
        }

        static public T ChildEnum<T>(this XmlElement xml, string childName) where T : struct
        {
#if DEBUG
            if (!xml.HasChild(childName))
                throw new Exception("Cannot find child xml tag with name '" + childName + "'.");
#endif
            if (Enum.IsDefined(typeof(T), xml[childName].InnerText))
                return (T)Enum.Parse(typeof(T), xml[childName].InnerText);
            else
                throw new Exception("The attribute value cannot be converted to the enum type.");
        }

        static public T ChildEnum<T>(this XmlElement xml, string childName, T defaultValue) where T : struct
        {
            if (xml.HasChild(childName))
            {
                if (Enum.IsDefined(typeof(T), xml[childName].InnerText))
                    return (T)Enum.Parse(typeof(T), xml[childName].InnerText);
                else
                    throw new Exception("The attribute value cannot be converted to the enum type.");
            }
            else
                return defaultValue;
        }

        static public Color ChildHexColor(this XmlElement xml, string childName)
        {
#if DEBUG
            if (!xml.HasChild(childName))
                throw new Exception("Cannot find child xml tag with name '" + childName + "'.");
#endif
            return Calc.HexToColor(xml[childName].InnerText);
        }

        static public Color ChildHexColor(this XmlElement xml, string childName, Color defaultValue)
        {
            if (xml.HasChild(childName))
                return Calc.HexToColor(xml[childName].InnerText);
            else
                return defaultValue;
        }

        static public Color ChildHexColor(this XmlElement xml, string childName, string defaultValue)
        {
            if (xml.HasChild(childName))
                return Calc.HexToColor(xml[childName].InnerText);
            else
                return Calc.HexToColor(defaultValue);
        }

        #endregion

        #region Ogmo Nodes

        static public Vector2 FirstNode(this XmlElement xml)
        {
            if (xml["node"] == null)
                return Vector2.Zero;
            else
                return new Vector2(xml["node"].AttrInt("x"), xml["node"].AttrInt("y"));
        }

        static public Vector2? FirstNodeNullable(this XmlElement xml)
        {
            if (xml["node"] == null)
                return null;
            else
                return new Vector2(xml["node"].AttrInt("x"), xml["node"].AttrInt("y"));
        }

        static public Vector2[] Nodes(this XmlElement xml, bool includePosition = false)
        {
            XmlNodeList nodes = xml.GetElementsByTagName("node");
            if (nodes == null)
                return includePosition ? new Vector2[] { xml.Position() } : new Vector2[0];

            Vector2[] ret;
            if (includePosition)
            {
                ret = new Vector2[nodes.Count + 1];
                ret[0] = xml.Position();
                for (int i = 0; i < nodes.Count; i++)
                    ret[i + 1] = new Vector2(Convert.ToInt32(nodes[i].Attributes["x"].InnerText), Convert.ToInt32(nodes[i].Attributes["y"].InnerText));
            }
            else
            {
                ret = new Vector2[nodes.Count];
                for (int i = 0; i < nodes.Count; i++)
                    ret[i] = new Vector2(Convert.ToInt32(nodes[i].Attributes["x"].InnerText), Convert.ToInt32(nodes[i].Attributes["y"].InnerText));
            }

            return ret;
        }

        static public Vector2 GetNode(this XmlElement xml, int nodeNum)
        {
            if (xml.Nodes().Length > nodeNum)
                return xml.Nodes()[nodeNum];
            else
                return Vector2.Zero;
        }

        static public Vector2? GetNodeNullable(this XmlElement xml, int nodeNum)
        {
            if (xml.Nodes().Length > nodeNum)
                return xml.Nodes()[nodeNum];
            else
                return null;
        }

        #endregion

        #region Add Stuff

        static public void SetAttr(this XmlElement xml, string attributeName, Object setTo)
        {
            XmlAttribute attr;

            if (xml.HasAttr(attributeName))
                attr = xml.Attributes[attributeName];
            else
            {
                attr = xml.OwnerDocument.CreateAttribute(attributeName);
                xml.Attributes.Append(attr);
            }

            attr.Value = setTo.ToString();
        }

        static public void SetChild(this XmlElement xml, string childName, Object setTo)
        {
            XmlElement ele;

            if (xml.HasChild(childName))
                ele = xml[childName];
            else
            {
                ele = xml.OwnerDocument.CreateElement(childName);
                xml.AppendChild(ele);
            }

            ele.InnerText = setTo.ToString();
        }

        static public XmlElement CreateChild(this XmlDocument doc, string childName)
        {
            XmlElement ele = doc.CreateElement(childName);
            doc.AppendChild(ele);
            return ele;
        }

        static public XmlElement CreateChild(this XmlElement xml, string childName)
        {
            XmlElement ele = xml.OwnerDocument.CreateElement(childName);
            xml.AppendChild(ele);
            return ele;
        }

        #endregion

        #endregion

        #region Sorting

        static public int SortLeftToRight(Entity a, Entity b)
        {
            return (int)((a.X - b.X) * 100);
        }

        static public int SortRightToLeft(Entity a, Entity b)
        {
            return (int)((b.X - a.X) * 100);
        }

        static public int SortTopToBottom(Entity a, Entity b)
        {
            return (int)((a.Y - b.Y) * 100);
        }

        static public int SortBottomToTop(Entity a, Entity b)
        {
            return (int)((b.Y - a.Y) * 100);
        }

        static public int SortByDepth(Entity a, Entity b)
        {
            return a.Depth - b.Depth;
        }

        static public int SortByDepthReversed(Entity a, Entity b)
        {
            return b.Depth - a.Depth;
        }

        #endregion

        #region Debug

        static public void Log(params object[] obj)
        {
            foreach (var o in obj)
            {
                if (o == null)
                    Debug.WriteLine("null");
                else
                    Debug.WriteLine(o.ToString());
            }
        }

        static public void LogEach<T>(IEnumerable<T> collection)
        {
            foreach (var o in collection)
                Debug.WriteLine(o.ToString());
        }

        static public void Dissect(Object obj)
        {
            Debug.Write(obj.GetType().Name + " { ");
            foreach (var v in obj.GetType().GetFields())
                Debug.Write(v.Name + ": " + v.GetValue(obj) + ", ");
            Debug.WriteLine(" }");
        }

        static private Stopwatch stopwatch;

        static public void StartTimer()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        static public void EndTimer()
        {
            stopwatch.Stop();
            Debug.WriteLine("Timer: " + stopwatch.ElapsedTicks + " ticks");
#if DESKTOP && DEBUG
            Commands.Trace("Timer: " + stopwatch.ElapsedTicks + " ticks");
#endif
            stopwatch = null;
        }

        #endregion
    }
}
