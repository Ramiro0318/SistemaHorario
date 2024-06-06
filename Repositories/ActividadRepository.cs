using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using SistemaHorario.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaHorario.Repositories
{
    public class ActividadRespository
    {
        SQLiteConnection conexion;

        public ActividadRespository()
        {
            //conexion = new("data/actividades.sqlite");
            var ruta = "actividades.sqlite";
            conexion = new SQLiteConnection(ruta);
            conexion.CreateTable<Actividad>();
        }

        public IEnumerable<Actividad> GetAll()
        {
            return conexion.Table<Actividad>().OrderBy(x => x.HoraInicio);
        }

        public IEnumerable<Actividad> GetDomingo() 
        {
            return conexion.Table<Actividad>().Where(x => x.Dia == "dom");
        }
        public IEnumerable<Actividad> GetLunes()
        {
            return conexion.Table<Actividad>().Where(x => x.Dia == "lun");
        }
        public IEnumerable<Actividad> GetMartes()
        {
            return conexion.Table<Actividad>().Where(x => x.Dia == "mar");
        }
        public IEnumerable<Actividad> GetMiercoles()
        {
            return conexion.Table<Actividad>().Where(x => x.Dia == "mie");
        }
        public IEnumerable<Actividad> GetJueves()
        {
            return conexion.Table<Actividad>().Where(x => x.Dia == "jue");
        }
        public IEnumerable<Actividad> GetViernes()
        {
            return conexion.Table<Actividad>().Where(x => x.Dia == "vie");
        }
        public IEnumerable<Actividad> GetSabado()
        {
            return conexion.Table<Actividad>().Where(x => x.Dia == "sab");
        }


        public void Insert(Actividad actividad)
        {
            conexion.Insert(actividad);
        }

        public void Update(Actividad actividad) 
        { 
            conexion.Update(actividad);
        }

        public void Delete(Actividad actividad) 
        {
            conexion.Delete(actividad);
        }
    }
}
