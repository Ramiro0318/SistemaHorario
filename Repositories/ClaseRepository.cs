using SistemaHorario.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaHorario.Repositories
{
    public class ClaseRepository
    {
        SQLiteConnection conexion;

        public ClaseRepository()
        {
            
            var ruta = "clases.sqlite";
            conexion = new SQLiteConnection(ruta);
            conexion.CreateTable<Clase>();
        }

        public IEnumerable<Clase> GetAll()
        {
            return conexion.Table<Clase>().OrderBy(x => x.HoraInicio);
        }

        public void Insert(Clase clase)
        {
            conexion.Insert(clase);
        }

        public void Update(Clase clase)
        {
            conexion.Update(clase);
        }

        public void Delete(Clase clase)
        {
            conexion.Delete(clase);
        }
    }
}
