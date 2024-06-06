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

        public IEnumerable<Clase> GetDomingo()
        {
            return conexion.Table<Clase>().Where(x => x.Dia == "dom");
        }
        public IEnumerable<Clase> GetLunes()
        {
            return conexion.Table<Clase>().Where(x => x.Dia == "lun");
        }
        public IEnumerable<Clase> GetMartes()
        {
            return conexion.Table<Clase>().Where(x => x.Dia == "mar");
        }
        public IEnumerable<Clase> GetMiercoles()
        {
            return conexion.Table<Clase>().Where(x => x.Dia == "mie");
        }
        public IEnumerable<Clase> GetJueves()
        {
            return conexion.Table<Clase>().Where(x => x.Dia == "jue");
        }
        public IEnumerable<Clase> GetViernes()
        {
            return conexion.Table<Clase>().Where(x => x.Dia == "vie");
        }
        public IEnumerable<Clase> GetSabado()
        {
            return conexion.Table<Clase>().Where(x => x.Dia == "sab");
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
