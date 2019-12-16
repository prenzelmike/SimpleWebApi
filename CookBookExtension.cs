namespace SimpleWebApi.Types
{
    using System.Xml.Serialization;
        public partial class CookBook
        {
            [XmlAttribute("schemaLocation", Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
            public string xsiSchemaLocation = "mh.dev.cb.01 CookBook.xsd";
        }
}