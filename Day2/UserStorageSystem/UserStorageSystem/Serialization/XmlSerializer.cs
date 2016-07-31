using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace UserStorageSystem.Serialization.Xml
{
    public class XmlSerializer
    {
        Type type;

        public XmlSerializer(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type parameter is null");
            this.type = type;
        }

        public void Serialize(object data, string path)
        {
            if (data.GetType().GetConstructor(new Type[0]) == null)
                throw new SerializationException("Can't find constructor()");
            if (type != data.GetType())
                throw new SerializationException("Type mismatch");
            XmlTextWriter xt = new XmlTextWriter(path, Encoding.UTF8);

            xt.Formatting = Formatting.Indented;
            xt.WriteStartDocument();
            xt.WriteStartElement(data.GetType().Name);

            GenerateXmlBody(xt, data);

            xt.WriteEndElement();
            xt.WriteEndDocument();

            xt.Flush();
            xt.Close();
        }

        private void GenerateXmlBody(XmlTextWriter xtw, object obj)
        {
            Type t = obj.GetType();

            foreach (var property in t.GetProperties())
            {
                if (property.GetValue(obj) == null)
                    continue;
                if (property.PropertyType.Module.Name != typeof(int).Module.Name && property.PropertyType.BaseType.Name != "Enum")
                {
                    xtw.WriteStartElement(property.Name);
                    GenerateXmlBody(xtw, property.GetValue(obj));
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
                        GenerateXmlBody(xtw, element);
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

        public object Deserialize(string path)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = null;


            doc.Load(path);
            root = doc.DocumentElement;

            if (type.Name != root.Name)
                throw new SerializationException("Wrong data type in xml document.");

            ConstructorInfo cInfo = type.GetConstructor(new Type[0]);

            object result = cInfo.Invoke(new object[0]);

            if (result == null)
                throw new SerializationException("Couldn't Create instance of type " + type.Name + ".");

            InitObject(result, root);

            Console.WriteLine();

            return result;
        }

        private void InitObject(object result, XmlNode root)
        {
            Type t = result.GetType();

            foreach (var property in t.GetProperties())
            {
                if (property.GetSetMethod() == null)
                    continue;
                if (property.PropertyType.Module.Name != typeof(int).Module.Name && property.PropertyType.BaseType.Name != "Enum")
                {
                    int index;
                    if (RootHasChild(root, property.Name, out index))
                    {
                        ConstructorInfo c = property.PropertyType.GetConstructor(new Type[0]);
                        object obj = c.Invoke(new object[0]);
                        if (obj == null)
                            throw new SerializationException("Couldn't Create instance of t " + property.PropertyType.Name + ".");

                        XmlNode node = root.ChildNodes.Item(index);
                        InitObject(obj, node);
                        property.SetValue(result, obj);
                    }
                }
                else
                if (property.PropertyType.Name == typeof(List<>).Name)
                {
                    int index;
                    if (RootHasChild(root, property.Name, out index))
                    {
                        XmlNode node = root.ChildNodes.Item(index);
                        var objType = property.PropertyType.GenericTypeArguments[0];
                        var objList = (IList)Activator.CreateInstance(property.PropertyType);
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            var obj = Activator.CreateInstance(objType);

                            if (obj == null)
                                throw new SerializationException("Couldn't Create instance of type " + type.Name + ".");

                            InitObject(obj, node.ChildNodes.Item(i));
                            objList.Add(Convert.ChangeType(obj, objType));
                        }
                        property.SetValue(result, objList);
                    }
                }
                else
                {
                    int index;
                    if (RootHasChild(root, property.Name, out index))
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
    }
}
