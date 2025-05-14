using FootballServerGood.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballServerGood.DataAccess
{
    public delegate string CreateOleDb(BaseEntity entity);
    public class ChangeEntity
    {
        private BaseEntity entity;
        private CreateOleDb createOleDb;

        public ChangeEntity(CreateOleDb createOleDb, BaseEntity entity)
        {
            this.createOleDb = createOleDb;
            this.entity = entity;
        }

        public BaseEntity Entity { get => entity; set => entity = value; }
        public CreateOleDb CreateOleDb { get => createOleDb; set => createOleDb = value; }
    }
}

