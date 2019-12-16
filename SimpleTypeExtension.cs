namespace SimpleWebApi.Types {
    using System.Xml.Serialization;

    public partial class SimpleTypeDocument {
        [XmlAttribute("schemaLocation", Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = "mh.dev.st.01 SimpleType.xsd";
    }

}