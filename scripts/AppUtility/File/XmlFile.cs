using System;
using System.Collections;
using System.Xml;
using UnityEngine;

namespace common
{
	public class XmlFile
	{
        protected   XmlDocument     m_doc;
        protected   string          m_path;


        /// <summary>
        /// XmlFile
        /// </summary>


        public XmlFile()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Release()
        {
            m_doc = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>bool</returns>
        public bool OpenFile(string path)
        {
            try
            {
                m_doc = new XmlDocument();
                m_doc.Load(path);
            }
            catch
            {
                //LogDebug.GetInstance().LogError("打开文件失败:" + path);
                return false;
            }
            m_path = path;
            //LogDebug.GetInstance().LogPrint("打开文件成功:" + path);
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>bool</returns>
        public bool LoadXml(string xml)
        {
            try
            {
                m_doc = new XmlDocument();
                m_doc.LoadXml(xml);
            }
            catch
            {
                //LogDebug.GetInstance().LogError("打开文件失败:" + xml);
				debug.GetInstance().Error("load xml failed");
                return false;
            }

            //LogDebug.GetInstance().LogPrint("打开文件成功:" + xml);
			debug.GetInstance().Error("load xml ok ");
            return true;
        }
        
        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attribute"></param>
        /// <returns>string</returns>
        /**************************************************
            *
            * XmlFile.Read(path, "/Node", "")
            * XmlFile.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
            ************************************************/
        public string Read(string node, string attribute)
        {
            string value = "";
            try
            {
                XmlNode xn = m_doc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch 
            {
                //LogDebug.GetInstance().LogError("读取节点数据失败,节点:" + node + "，变量:" + attribute);
                return null;
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="attribute"></param>
        /// <returns>string</returns>
        /**************************************************
            *
            * XmlFile.ReadInt("Node", "")
            * XmlFile.ReadInt("/Node/Element[@Attribute='Name']", "Attribute")
            ************************************************/
        public int ReadInt(string node, string attribute)
        {
            string data = Read(node, attribute);
            try
            {
                return Convert.ToInt32(data);
            }
            catch
            {
                //LogDebug.GetInstance().logWarning("xml字符转整型失败");
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="attribute"></param>
        /// <returns>string</returns>
        /**************************************************
            *
            * XmlFile.ReadFloat("Node", "")
            * XmlFile.ReadFloat("/Node/Element[@Attribute='Name']", "Attribute")
            ************************************************/
        public float ReadFloat(string node, string attribute)
        {
            string data = Read(node, attribute);
            try
            {
                return Convert.ToSingle(data);
            }
            catch
            {
                //LogDebug.GetInstance().logWarning("xml字符转整型失败");
                return 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /**************************************************
            * 
            * XmlHelper.Insert(path, "/Node", "Element", "", "Value")
            * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")
            * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")
            ************************************************/
        public void Insert(string node, string element, string attribute, string value)
        {
            try
            {
                XmlNode xn = m_doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = m_doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                m_doc.Save(m_path);
            }
            catch 
            {
                //LogDebug.GetInstance().LogError("插入节点数据失败,节点:" + node + "，变量:" + attribute);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <param name="node"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /**************************************************
            * :
            * XmlHelper.Insert(path, "/Node", "", "Value")
            * XmlHelper.Insert(path, "/Node", "Attribute", "Value")
            ************************************************/
        public void Update(string node, string attribute, string value)
        {
            try
            {
                XmlNode xn = m_doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xe.InnerText = value;
                else
                    xe.SetAttribute(attribute, value);
                m_doc.Save(m_path);
            }
            catch 
            {
                //LogDebug.GetInstance().LogError("更新节点数据失败,节点:" + node + "，变量:" + attribute);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <param name="node"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /**************************************************
            *
            * XmlHelper.Delete(path, "/Node", "")
            * XmlHelper.Delete(path, "/Node", "Attribute")
            ************************************************/
        public void Delete(string node, string attribute)
        {
            try
            {
                XmlNode xn = m_doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
                m_doc.Save(m_path);
            }
            catch 
            {
                //LogDebug.GetInstance().LogError("删除节点数据失败,节点:" + node + "，变量:" + attribute);
            }
        }
		
		public int GetNodeCount(){
			return m_doc.ChildNodes[1].ChildNodes.Count ;
		}
		
		public string ReadByIndex(int index,string attribute){

			string value = "";
            try
            {
                XmlNode xn = m_doc.ChildNodes[1];
				if(index >= xn.ChildNodes.Count)
					return null ;
				//int i = xn.ChildNodes.Count ;
				xn = xn.ChildNodes[index];
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch 
            {
                //LogDebug.GetInstance().LogError("读取节点数据失败,节点:" + node + "，变量:" + attribute);
                return null;
            }
            return value;
		}
		
		public int ReadIntByIndex(int index,string attribute){
			string data = ReadByIndex(index, attribute);
            try
            {
                return Convert.ToInt32(data);
            }
            catch
            {
                //LogDebug.GetInstance().logWarning("xml字符转整型失败");
                return 0;
            }
		}
		
		public float ReadFloat(int index,string attribute)
        {
            string data = ReadByIndex(index, attribute);
            try
            {
                return Convert.ToSingle(data);
            }
            catch
            {
                //LogDebug.GetInstance().logWarning("xml字符转整型失败");
                return 0;
            }
        }
	}
}
