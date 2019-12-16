using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApi.Types;


namespace SimpleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
         
        private  SimpleType[] SimpleTypeList = new SimpleType[] {
                new SimpleType{ Id= 1, Name= "Erster Eintrag"},
                new SimpleType{ Id= 2, Name= "Zweiter Eintrag"},
                new SimpleType{ Id= 3, Name= "Dritter Eintrag von Vielen"},
                new SimpleType{ Id= 4, Name= "Vierter Eintrag"},
                new SimpleType{ Id= 5, Name= "Fünfter Eintrag"},
                new SimpleType{ Id= 6, Name= "Sechster Eintrag"},
                new SimpleType{ Id= 7, Name= "Siebter Eintrag"},
                new SimpleType{ Id= 8, Name= "Weiterer Eintrag"}
                };


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<SimpleType>> Get()
        {
            return this.ReadData();
        }


        [HttpGet]
        [Route("search/{term}")]
        public ActionResult<IEnumerable<SimpleType>> Search(string term)
        {
            return this.ReadData().Where(st => st.Name.ToLower().Contains(term.ToLower())).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<SimpleType> Get(int id)
        {
            return this.ReadData().FirstOrDefault(st => st.Id == id);
        }

        


        // POST api/values
        [HttpPost]
        public ActionResult<SimpleType> Post([FromBody] SimpleType sTypeNew)
        {
            if (sTypeNew.Id != -1)
            {
                return BadRequest();
            }
            
            SimpleType[] stList = this.ReadData();
            int maxId = stList.Max(st => st.Id);
            sTypeNew.Id = 1 + maxId;
            stList = stList.Concat(new SimpleType[] { sTypeNew })
                           .ToArray();
            this.WriteData(stList);
            return sTypeNew;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        //[HttpPut]
        public ActionResult<SimpleType> Put(int id, [FromBody] SimpleType sTypeUpdated)
        //public ActionResult<SimpleType> Put([FromBody] SimpleType sTypeUpdated)
        {
            if (id != sTypeUpdated.Id)
            {
                return BadRequest();
            }
            SimpleType[] list = this.ReadData();
            list.FirstOrDefault(st => st.Id == sTypeUpdated.Id).Name = sTypeUpdated.Name;
            this.WriteData(list);
            return list.FirstOrDefault(st => st.Id == sTypeUpdated.Id);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            SimpleType[] stList = this.ReadData();
            stList = stList.Where(x=>x.Id!=id).ToArray();
            this.WriteData(stList);
        }


        private SimpleType[] ReadData()
        {
            SimpleTypeDocument myDoc; 
            XmlSerializer ser = new XmlSerializer(typeof(SimpleTypeDocument));
            FileStream reader = new FileStream("./Data/SimpleTypes.xml", FileMode.Open);
            myDoc = (SimpleTypeDocument) ser.Deserialize(reader);
            reader.Close();
            return myDoc.SimpleTypes;
        }

        private void WriteData(SimpleType[] myList)
        {
            XmlSerializer ser = new XmlSerializer(typeof(SimpleTypeDocument));
            StreamWriter myWriter = new StreamWriter("./Data/SimpleTypes.xml");  
            SimpleTypeDocument toPersist = new SimpleTypeDocument {SimpleTypes = myList};
            ser.Serialize(myWriter, toPersist);  
            myWriter.Close();  
        }
    }
}
