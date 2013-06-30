using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Diagnostics;

namespace Monocle
{
    static public class Calc
    {
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
        private const string HEX = "0123456789ABCDEF";

        static public byte HexToByte(char c)
        {
            return (byte)HEX.IndexOf(char.ToUpper(c));
        }

        static public Vector2 EightWayNormal(this Vector2 vec)
        {
            float angle = vec.Angle();
            angle = (float)Math.Floor((angle + MathHelper.PiOver4 / 2f) / MathHelper.PiOver4) * MathHelper.PiOver4;
            return AngleToVector(angle, 1f);
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

        static public float ReflectAngle(float angle, Vector2 axis)
        {
            return ReflectAngle(angle, axis.Angle());
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

        static public Vector2 AngleToVector(float angle, float length)
        {
            return new Vector2((float)Math.Cos(angle) * length, (float)Math.Sin(angle) * length);
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

        static public Vector2 Floor(Vector2 val)
        {
            return new Vector2((int)val.X, (int)val.Y);
        }

        static public Vector2 Approach(Vector2 val, Vector2 target, float maxMove)
        {
            if (maxMove == 0 || val == target)
                return val;

            Vector2 diff = target - val;
            float length = diff.Length();

            return val + ((diff / length) * Math.Min(length, maxMove));
        }

        static public Vector2 Clamp(Vector2 val, float minX, float minY, float maxX, float maxY)
        {
            return new Vector2(MathHelper.Clamp(val.X, minX, maxX), MathHelper.Clamp(val.Y, minY, maxY));
        }

        static public float AngleDifference(float angleA, float angleB)
        {
            float diff = angleB - angleA;

            while (diff > MathHelper.Pi) { diff -= MathHelper.TwoPi; }
            while (diff <= -MathHelper.Pi) { diff += MathHelper.TwoPi; }

            return diff;
        }

        static public float Angle(Vector2 from, Vector2 to)
        {
            return (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
        }

        static public float Angle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
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

        static public int[,] ReadCSV(string csv)
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
                for (x = 0; x < lines[y].Length; x++)
                    data[x, y] = Convert.ToInt32(lines[y][x]);
                for (; x < longest; x++)
                    data[x, y] = -1;
            }

            return data;
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

        #endregion

        #region XML

        static public XmlDocument GetXML(string filename)
        {
            if (File.Exists(filename))
            {
                var xml = new XmlDocument();
                xml.Load(filename);
                return xml;
            }
            else
                return null;
        }

        static public string Attr(this XmlElement xml, string attributeName)
        {
#if DEBUG
            if (xml.Attributes[attributeName] == null)
                throw new Exception("Element does not contain the attribute \"" + attributeName + "\"");
#endif
            return xml.Attributes[attributeName].InnerText;
        }

        static public int AttrInt(this XmlElement xml, string attributeName)
        {
#if DEBUG
            if (xml.Attributes[attributeName] == null)
                throw new Exception("Element does not contain the attribute \"" + attributeName + "\"");
#endif
            return Convert.ToInt32(xml.Attributes[attributeName].InnerText);
        }

        static public float AttrFloat(this XmlElement xml, string attributeName)
        {
#if DEBUG
            if (xml.Attributes[attributeName] == null)
                throw new Exception("Element does not contain the attribute \"" + attributeName + "\"");
#endif
            return Convert.ToSingle(xml.Attributes[attributeName].InnerText);
        }

        static public bool AttrBool(this XmlElement xml, string attributeName)
        {
#if DEBUG
            if (xml.Attributes[attributeName] == null)
                throw new Exception("Element does not contain the attribute \"" + attributeName + "\"");
#endif
            return Convert.ToBoolean(xml.Attributes[attributeName].InnerText);
        }

        static public T AttrEnum<T>(this XmlElement xml, string attributeName) where T : struct
        {
#if DEBUG
            if (xml.Attributes[attributeName] == null)
                throw new Exception("Element does not contain the attribute \"" + attributeName + "\"");
#endif
            if (Enum.IsDefined(typeof(T), xml.Attributes[attributeName].InnerText))
                return (T)Enum.Parse(typeof(T), xml.Attributes[attributeName].InnerText);
            else
                throw new Exception("The attribute value cannot be converted to the enum type.");
        }

        static public T AttrEnum<T>(this XmlElement xml, string attributeName, T def) where T : struct
        {
            if (xml.Attributes[attributeName] == null)
                return def;
            else
                return xml.AttrEnum<T>(attributeName);
        }

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

        static public Vector2 Position(this XmlElement xml)
        {
            return new Vector2(xml.AttrFloat("x"), xml.AttrFloat("y"));
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

        static public int InnerInt(this XmlElement xml)
        {
            return Convert.ToInt32(xml.InnerText);
        }

        static public bool InnerBool(this XmlElement xml)
        {
            return Convert.ToBoolean(xml.InnerText);
        }

        #endregion

        #region Debug

#if DEBUG
        static public void Log(object obj)
        {
            Debug.WriteLine(obj.ToString());
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
            Commands.Trace("Timer: " + stopwatch.ElapsedTicks + " ticks");
            stopwatch = null;
        }
#endif

        #endregion
    }
}
