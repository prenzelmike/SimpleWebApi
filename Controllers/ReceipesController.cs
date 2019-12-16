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
    public class ReceipesController : ControllerBase
    {
        //get the lookup values for the unit select
        [HttpGet("{action}")]
        public UnitLookup[] GetUnits()
        {
            return Enum.GetValues(typeof(UnitType))
               .Cast<UnitType>()
               .Select(t => new UnitLookup
               {
                   Id = ((int)t),
                   DisplayValue = t.ToString()
               }).ToArray();
        }

        // GET api/receipes
        [HttpGet]
        public ActionResult<IEnumerable<Receipe>> Get()
        {
            return this.ReadData();
        }


        [HttpGet]
        [Route("search/{term}")]
        public ActionResult<IEnumerable<ReceipeSearchResult>> Search(string term)
        {
            IEnumerable<Receipe> receipes = this.ReadData().Where(rc => rc.Code.ToLower().Contains(term.ToLower()));
            return receipes.Select(rec => new ReceipeSearchResult
            {
                Id = rec.Id,
                Abstract = String.Format("{0}: {1}..., {2}", rec.Code,
                    rec.Workflow.Substring(0, 20),
                    string.Join(' ', rec.Ingredients.Select(r => r.Name).ToArray()))
            }).ToList();
        }

        // GET api/receipes/5
        [HttpGet("{id}")]
        public ActionResult<Receipe> Get(int id)
        {
            return this.ReadData().FirstOrDefault(st => st.Id == id);
        }

        


        // POST api/receipes
        [HttpPost]
        public ActionResult<Receipe> Post([FromBody] Receipe receipeNew)
        {
            if (receipeNew.Id != -1)
            {
                return BadRequest();
            }
            
            Receipe[] rcList = this.ReadData();
            int maxId = rcList.Max(st => st.Id);
            receipeNew.Id = 1 + maxId;
            rcList = rcList.Concat(new Receipe[] { receipeNew })
                           .ToArray();
            this.WriteData(rcList);
            return receipeNew;
        }

        // PUT api/receipes/5
        [HttpPut("{id}")]
        //[HttpPut]
        public ActionResult<Receipe> Put(int id, [FromBody] Receipe receipeUpdated)
        {
            if (id != receipeUpdated.Id)
            {
                return BadRequest();
            }
            Receipe[] list = this.ReadData();
            int idx = Array.IndexOf(list, list.FirstOrDefault(st => st.Id == receipeUpdated.Id));
            list[idx] = receipeUpdated;

            this.WriteData(list);
            return list.FirstOrDefault(st => st.Id == receipeUpdated.Id);
        }

        // DELETE api/receipes/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Receipe[] rcList = this.ReadData();
            rcList = rcList.Where(x=>x.Id!=id).ToArray();
            this.WriteData(rcList);
        }


        private Receipe[] ReadData()
        {
            CookBook myDoc; 
            XmlSerializer ser = new XmlSerializer(typeof(CookBook));
            FileStream reader = new FileStream("./Data/Receipes.xml", FileMode.Open);
            myDoc = (CookBook) ser.Deserialize(reader);
            reader.Close();
            return myDoc.Receipes;
        }

        private void WriteData(Receipe[] myList)
        {
            XmlSerializer ser = new XmlSerializer(typeof(CookBook));
            StreamWriter myWriter = new StreamWriter("./Data/Receipes.xml");  
            CookBook toPersist = new CookBook {Receipes = myList};
            ser.Serialize(myWriter, toPersist);  
            myWriter.Close();  
        }
    }
}
