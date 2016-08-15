using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace UserStorageSystem.Serialization.Xml
{
    /// <summary>
    ///     Custom Xml Serializer. Uses reflection to serialize objects.
    /// </summary>
    public class XmlSerializer
    {
        private Type type;
        /// <summary>
        ///     Initializes XmlSerializer with type param
        /// </summary>
        /// <param name="type">
        ///     Type of data, whitch will be serialized.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     thrown when type param is null.
        /// </exception>
        public XmlSerializer(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("Type parameter is null");
            }

            this.type = type;
        }

        /// <summary>
        ///     Enables to serialize data to xml file.
        /// </summary>
        /// <param name="data">
        ///     data that will be serialized.
        /// </param>
        /// <param name="path">
        ///     path to xml file.
        /// </param>
        /// <exception cref="SerializationException">
        ///     thrown when object doesn't contains default constructor or data type, passed as parameter, and type, passed to serializer constructor don't match.
        /// </exception>
        public void Serialize(object data, string path)
        {
            if (data.GetType().GetConstructor(new Type[0]) == null)
            {
                throw new SerializationException("Can't find constructor()");
            }

            if (this.type != data.GetType())
            {
                throw new SerializationException("Type mismatch");
            }

            XmlTextWriter xt = new XmlTextWriter(path, Encoding.UTF8);

            xt.Formatting = Formatting.Indented;
            xt.WriteStartDocument();
            xt.WriteStartElement(data.GetType().Name);

            this.GenerateXmlBody(xt, data);

            xt.WriteEndElement();
            xt.WriteEndDocument();

            xt.Flush();
            xt.Close();
        }


        /// <summary>
        ///     Deserializes object from xml file.
        /// </summary>
        /// <param name="path">
        ///     path to xml file.
        /// </param>
        /// <returns>
        ///     deserialized object.
        /// </returns>
        /// <exception cref="SerializationException">
        ///     thrown when object doesn't contains default constructor or data type, passed as parameter, and type, passed to serializer constructor don't match.
        /// </exception>
        public object Deserialize(string path)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = null;

            doc.Load(path);
            root = doc.DocumentElement;

            if (this.type.Name != root.Name)
            {
                throw new SerializationException("Wrong data type in xml document.");
            }

            ConstructorInfo cInfo = this.type.GetConstructor(new Type[0]);

            object result = cInfo.Invoke(new object[0]);

            if (result == null)
            {
                throw new SerializationException("Couldn't Create instance of type " + this.type.Name + ".");
            }

            this.InitObject(result, root);

            Console.WriteLine();

            return result;
        }

        private void InitObject(object result, XmlNode root)
        {
            Type t = result.GetType();

            foreach (var property in t.GetProperties())
            {
                if (property.GetSetMethod() == null)
                {
                    continue;
                }

                if (property.PropertyType.Module.Name != typeof(int).Module.Name && property.PropertyType.BaseType.Name != "Enum")
                {
                    int index;
                    if (this.RootHasChild(root, property.Name, out index))
                    {
                        ConstructorInfo c = property.PropertyType.GetConstructor(new Type[0]);
                        object obj = c.Invoke(new object[0]);
                        if (obj == null)
                        {
                            throw new SerializationException("Couldn't Create instance of t " + property.PropertyType.Name + ".");
                        }

                        XmlNode node = root.ChildNodes.Item(index);
                        this.InitObject(obj, node);
                        property.SetValue(result, obj);
                    }
                }
                else
                if (property.PropertyType.Name == typeof(List<>).Name)
                {
                    int index;
                    if (this.RootHasChild(root, property.Name, out index))
                    {
                        XmlNode node = root.ChildNodes.Item(index);
                        var objType = property.PropertyType.GenericTypeArguments[0];
                        var objList = (IList)Activator.CreateInstance(property.PropertyType);
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            var obj = Activator.CreateInstance(objType);

                            if (obj == null)
                            {
                                throw new SerializationException("Couldn't Create instance of type " + this.type.Name + ".");
                            }

                            this.InitObject(obj, node.ChildNodes.Item(i));
                            objList.Add(Convert.ChangeType(obj, objType));
                        }

                        property.SetValue(result, objList);
                    }
                }
                else
                {
                    int index;
                    if (this.RootHasChild(root, property.Name, out index))
                    {
                        XmlNode node = root.ChildNodes.Item(index);
                        if (property.PropertyType.BaseType.Name == "Enum")
                        {
                            property.SetValue(result, Enum.Parse(property.PropertyType, node.InnerText));
                        }
                        else
                        {
                            property.SetValue(result, Convert.ChangeType(node.InnerText, property.PropertyType));
                        }
                    }
                }
            }
        }

        private bool RootHasChild(XmlNode root, string name, out int index)
        {
            bool result = false;

            index = -1;
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                if (root.ChildNodes.Item(i).Name == name)
                {
                    result = true;
                    index = i;
                    break;
                }
            }

            return result;
        }

        private void GenerateXmlBody(XmlTextWriter xtw, object obj)
        {
            Type t = obj.GetType();

            foreach (var property in t.GetProperties())
            {
                if (property.GetValue(obj) == null)
                {
                    continue;
                }

                if (property.PropertyType.Module.Name != typeof(int).Module.Name && property.PropertyType.BaseType.Name != "Enum")
                {
                    xtw.WriteStartElement(property.Name);
                    this.GenerateXmlBody(xtw, property.GetValue(obj));
                    xtw.WriteEndElement();
                }
                else
                    if (property.PropertyType.Name == typeof(List<>).Name)
                {
                    var objectCollection = (dynamic)property.GetValue(obj);
                    xtw.WriteStartElement(property.Name);
                    foreach (var element in objectCollection)
                    {
                        xtw.WriteStartElement(element.GetType().Name);
                        this.GenerateXmlBody(xtw, element);
                        xtw.WriteEndElement();
                    }

                    xtw.WriteEndElement();
                }
                else
                {
                    xtw.WriteStartElement(property.Name);
                    xtw.WriteString(property.GetValue(obj).ToString());
                    xtw.WriteEndElement();
                }
            }
        }
    }
}
